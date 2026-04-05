using Dapper;
using Model.DTO;
using Model.Entities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using Utility;

namespace DAL
{
    public class HomeHelper : BaseDBHelper
    {
        private IConfig _config;
        string _log = "";
        string _ReturnException = "";
        public HomeHelper(IConfig config) : base(config)
        {
            _config = config;
            _log = config.Log;
            _ReturnException = config.ReturnException;
        }

        public async Task<Response<IEnumerable<dynamic>>> GetSchemas(SchemaEntity _InputParam)
        {
            Response<IEnumerable<dynamic>> objResponse = new Response<IEnumerable<dynamic>>();
            try
            {
                // Dynamically update the connection string
                string connectionString = $"{_config.ConnectionString}{_InputParam.Database}";
                using (_con = new Npgsql.NpgsqlConnection(connectionString))
                {
                    _con.Open();
                    string query = "SELECT schema_name::character VARYING FROM information_schema.schemata;";
                    var result = await _con.QueryAsync<dynamic>(query);

                    objResponse.Data = result;
                    if (objResponse.Data != null && objResponse.Data.Count() > 0)
                    {
                        objResponse.IsSuccess = true;
                        objResponse.Message = "Schemas retrieved successfully";
                        objResponse.DataLength = objResponse.Data.Count();
                    }
                    else
                    {
                        objResponse.IsSuccess = false;
                        objResponse.Message = "Records Not Found";
                        objResponse.DataLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteErrorLog("GetSchemas", ex.Message, _log);
                objResponse.IsSuccess = false;
                if (_ReturnException == "0")
                    objResponse.Message = "Data Not Available";
                else
                    objResponse.Message = ex.Message;
                objResponse.DataLength = 0;
                return objResponse;
            }
            return objResponse;
        }

        public async Task<Response<IEnumerable<dynamic>>> GetTable(TableEntity _InputParam)
        {
            Response<IEnumerable<dynamic>> objResponse = new Response<IEnumerable<dynamic>>();
            try
            {
                string connectionString = $"{_config.ConnectionString}{_InputParam.Database}";
                using (_con = new Npgsql.NpgsqlConnection(connectionString))
                {
                    _con.Open();
                    string query = $"SELECT table_name FROM information_schema.tables WHERE table_schema = '{_InputParam.Schema}';";

                    var result = await _con.QueryAsync<dynamic>(query);

                    objResponse.Data = result;
                    if (objResponse.Data != null && objResponse.Data.Count() > 0)
                    {
                        objResponse.IsSuccess = true;
                        objResponse.Message = "Table retrieved successfully";
                        objResponse.DataLength = objResponse.Data.Count();
                    }
                    else
                    {
                        objResponse.IsSuccess = false;
                        objResponse.Message = "Records Not Found";
                        objResponse.DataLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteErrorLog("GetTable", ex.Message, _log);
                objResponse.IsSuccess = false;
                if (_ReturnException == "0")
                    objResponse.Message = "Data Not Available";
                else
                    objResponse.Message = ex.Message;
                objResponse.DataLength = 0;
                return objResponse;
            }
            return objResponse;
        }

        public async Task<Response<IEnumerable<dynamic>>> GetQuery(QueryEntity _InputParam)
        {
            Response<IEnumerable<dynamic>> objResponse = new Response<IEnumerable<dynamic>>();
            try
            {
                string query = _InputParam.Query;
                VerifyQuery vq = new VerifyQuery();
                var vr = vq.IsQueryUnSafe(query);
                if (vr.isUnSafe)
                {
                    objResponse.DataLength = 0;
                    objResponse.IsSuccess = false;
                    objResponse.Message = vr.msg;
                    return objResponse;
                }
                else
                {
                    var fch = await checkFun(_InputParam);
                    if (fch.isUnSafe)
                    {
                        objResponse.DataLength = 0;
                        objResponse.IsSuccess = false;
                        objResponse.Message = fch.msg;
                        return objResponse;
                    }
                    List<InsertQueryEntity> list = new List<InsertQueryEntity>();
                    InsertQueryEntity k = new InsertQueryEntity
                    {
                        user_id = _InputParam.user_id,
                        query_val = _InputParam.Query
                    };
                    list.Add(k);
                    var kr = InsertQuery(list);
                    string connectionString = $"{_config.ConnectionString}{_InputParam.Database}";
                    using (_con = new Npgsql.NpgsqlConnection(connectionString))
                    {
                        _con.Open();
                        using (var dBTransaction = _con.BeginTransaction())
                        {
                            try
                            {
                                var result = await _con.QueryAsync<dynamic>(query, transaction: dBTransaction);
                                objResponse.Data = result;
                                if (objResponse.Data != null && objResponse.Data.Count() > 0)
                                {
                                    objResponse.IsSuccess = true;
                                    objResponse.Message = "Query Data retrieved successfully";
                                    objResponse.DataLength = objResponse.Data.Count();
                                }
                                else
                                {
                                    objResponse.IsSuccess = false;
                                    objResponse.Message = "Records Not Found";
                                    objResponse.DataLength = 0;
                                }
                            }
                            catch (Exception ex)
                            {
                                objResponse.IsSuccess = false;
                                objResponse.Message = ex.Message.ToString();
                                objResponse.DataLength = 0;
                            }
                            finally
                            {
                                dBTransaction.Rollback();
                                _con.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.IsSuccess = false;
                objResponse.Message = ex.Message.ToString();
                objResponse.DataLength = 0;
                return objResponse;
            }
            return objResponse;
        }
        public async Task<(bool isUnSafe, string msg)> checkFun(QueryEntity _InputParam)
        {

            try
            {
                string connectionString = $"{_config.ConnectionString}{_InputParam.Database}";
                using (_con = new Npgsql.NpgsqlConnection(connectionString))
                {
                    _con.Open();

                    string pattern = @"(?:FROM|SELECT|EXEC)\s+(\w+)\.(\w+)";

                    //string pattern = @"(?:FROM|SELECT)\s+(\w+)\.(\w+)";
                    
                    var match = Regex.Match(_InputParam.Query, pattern, RegexOptions.IgnoreCase);

                    if (!match.Success || match.Groups.Count < 3)
                    {
                        return (true, "The provided query does not contain a valid schema and function name.");
                    }

                    string schemaName = match.Groups[1].Value;
                    string routineName = match.Groups[2].Value;

                    if (string.IsNullOrWhiteSpace(schemaName) || string.IsNullOrWhiteSpace(routineName))
                    {
                        return (true, "given schema or function name is invalid.");
                    }

                    string funQuery = @"SELECT COUNT(*) FROM information_schema.routines 
                            WHERE routine_schema = @SchemaName AND routine_name = @RoutineName";

                    var result = await _con.ExecuteScalarAsync<object>(funQuery, new
                    {
                        SchemaName = schemaName,
                        RoutineName = routineName
                    });

                    int count = Convert.ToInt32(result);

                 //   int count = await _con.ExecuteScalarAsync<int>(funQuery, new { SchemaName = schemaName, RoutineName = routineName });
                    if (count > 0)
                    {
                        return (true, $"Function is not allowed, i.e.'{routineName}'.");
                    }

                    return (false, "");


                }
            }
            catch (Exception ex)
            {
                LogService.WriteErrorLog("CheckFun", ex.Message, _log);
                return (true, ex.Message);

            }

        }

        public async Task<dynamic> InsertQuery(List<InsertQueryEntity> _InputParam)
        {
            try
            {
                string querytype = "InsertQuery";
                string funName = "gis_db_access_users.fn_get_general_multipurpose";
                String Database = "adminboundary";
                string connectionString = $"{_config.ConnectionString}{Database}";
                _con = new Npgsql.NpgsqlConnection(connectionString);
                _con.Open();
                IDbTransaction dbTransactiondbTransaction = _con.BeginTransaction();
                string Json_Data = JsonConvert.SerializeObject(_InputParam);
                string[] param = { Json_Data };
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@querytype", querytype);
                parameters.Add("@param_array", param);
                var _result = await _con.QueryFirstOrDefaultAsync<dynamic>(funName, param: parameters, dbTransactiondbTransaction, commandType: System.Data.CommandType.StoredProcedure);
                dbTransactiondbTransaction.Commit();
                _con.Close();
                return _result;
            }
            catch (Exception ex)
            {
                LogService.WriteErrorLog("InsertQuery", ex.ToString(), _log);
            }
            return 0;
        }

        public async Task<Response<IEnumerable<dynamic>>> GetDataBase()
        {
            Response<IEnumerable<dynamic>> objResponse = new Response<IEnumerable<dynamic>>();
            try
            {
                String Database = "adminboundary";
                string connectionString = $"{_config.ConnectionString}{Database}";
                using (_con = new Npgsql.NpgsqlConnection(connectionString))
                {
                    _con.Open();
                    string query = "SELECT datname FROM pg_database WHERE datistemplate = false;";
                    var result = await _con.QueryAsync<dynamic>(query);

                    objResponse.Data = result;
                    if (objResponse.Data != null && objResponse.Data.Count() > 0)
                    {
                        objResponse.IsSuccess = true;
                        objResponse.Message = "DataBase list retrieved successfully";
                        objResponse.DataLength = objResponse.Data.Count();
                    }
                    else
                    {
                        objResponse.IsSuccess = false;
                        objResponse.Message = "Records Not Found";
                        objResponse.DataLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteErrorLog("GetDataBase", ex.Message, _log);
                objResponse.IsSuccess = false;
                if (_ReturnException == "0")
                    objResponse.Message = "Data Not Available";
                else
                    objResponse.Message = ex.Message;
                objResponse.DataLength = 0;
                return objResponse;
            }
            return objResponse;
        }

        public async Task<Response<IEnumerable<dynamic>>> GetTransaction(TransactionEntity _InputParam)
        {
            Response<IEnumerable<dynamic>> objResponse = new Response<IEnumerable<dynamic>>();

            try
            {
                string querytype = "GetTransaction";
                string funName = "gis_db_access_users.fn_get_general_multipurpose";
                String Database = "adminboundary";
                string connectionString = $"{_config.ConnectionString}{Database}";
                _con = new Npgsql.NpgsqlConnection(connectionString);
                _con.Open();
                IDbTransaction dbTransactiondbTransaction = _con.BeginTransaction();
                string[] param = { _InputParam.user_id };
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@querytype", querytype);
                parameters.Add("@param_array", param);
                objResponse.Data = await _con.QueryAsync<dynamic>(funName, param: parameters, dbTransactiondbTransaction, commandType: System.Data.CommandType.StoredProcedure);
                dbTransactiondbTransaction.Commit();
                _con.Close();

                if (objResponse.Data != null && objResponse.Data.Count() > 0)
                {
                    objResponse.IsSuccess = true;
                    objResponse.Message = "Query Data retrieved successfully";
                    objResponse.DataLength = objResponse.Data.Count();
                }
                else
                {
                    objResponse.IsSuccess = false;
                    objResponse.Message = "Records Not Found";
                    objResponse.DataLength = 0;
                }

            }
            catch (Exception ex)
            {
                objResponse.IsSuccess = false;
                objResponse.Message = ex.Message.ToString();
                objResponse.DataLength = 0;
                return objResponse;
            }
            return objResponse;
        }

        public async Task<Response<IEnumerable<dynamic>>> GetFunctions(FunctionEntity _InputParam)
        {
            Response<IEnumerable<dynamic>> objResponse = new Response<IEnumerable<dynamic>>();
            try
            {
                string connectionString = $"{_config.ConnectionString}{_InputParam.Database}";
                using (_con = new Npgsql.NpgsqlConnection(connectionString))
                {
                    _con.Open();

                    string query = "";
                    if (_InputParam.qid == 3)
                    {
                        query = $"SELECT routine_name as scriptval FROM information_schema.routines WHERE routine_schema = '{_InputParam.Schema}' and routine_type <> 'null' order by routine_name;";
                    }
                    else if (_InputParam.qid == 7) 
                    {
                        query = $"SELECT table_name as scriptval FROM information_schema.views WHERE table_schema = '{_InputParam.Schema}' order by table_name;";
                    }
                    var result = await _con.QueryAsync<dynamic>(query);

                    objResponse.Data = result;
                    if (objResponse.Data != null && objResponse.Data.Count() > 0)
                    {
                        objResponse.IsSuccess = true;
                        objResponse.Message = "Function/Views retrieved successfully";
                        objResponse.DataLength = objResponse.Data.Count();
                    }
                    else
                    {
                        objResponse.IsSuccess = false;
                        objResponse.Message = "Records Not Found";
                        objResponse.DataLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteErrorLog("GetFunctions", ex.Message, _log);
                objResponse.IsSuccess = false;
                if (_ReturnException == "0")
                    objResponse.Message = "Data Not Available";
                else
                    objResponse.Message = ex.Message;
                objResponse.DataLength = 0;
                return objResponse;
            }
            return objResponse;
        }

        public async Task<Response<IEnumerable<dynamic>>> GetScript(ScriptEntity _InputParam)
        {
            Response<IEnumerable<dynamic>> objResponse = new Response<IEnumerable<dynamic>>();
            try
            {
                string connectionString = $"{_config.ConnectionString}{_InputParam.Database}";
                using (_con = new Npgsql.NpgsqlConnection(connectionString))
                {
                    _con.Open();

                    string query = "";
                    if (_InputParam.qid == 3)
                    {
                        query = $"SELECT routine_definition as script_defination FROM information_schema.routines WHERE routine_schema = '{_InputParam.Schema}' and routine_name='{_InputParam.name}' order by routine_name;";
                    }
                    else if (_InputParam.qid == 7)
                    {
                        query = $"SELECT view_definition as script_defination FROM information_schema.views WHERE table_schema = '{_InputParam.Schema}' and table_name='{_InputParam.name}'";
                    }
                    var result = await _con.QueryAsync<dynamic>(query);

                    objResponse.Data = result;
                    if (objResponse.Data != null && objResponse.Data.Count() > 0)
                    {
                        objResponse.IsSuccess = true;
                        objResponse.Message = "Function/Views retrieved successfully";
                        objResponse.DataLength = objResponse.Data.Count();
                    }
                    else
                    {
                        objResponse.IsSuccess = false;
                        objResponse.Message = "Records Not Found";
                        objResponse.DataLength = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteErrorLog("GetFunctions", ex.Message, _log);
                objResponse.IsSuccess = false;
                if (_ReturnException == "0")
                    objResponse.Message = "Data Not Available";
                else
                    objResponse.Message = ex.Message;
                objResponse.DataLength = 0;
                return objResponse;
            }
            return objResponse;
        }


    }
}
