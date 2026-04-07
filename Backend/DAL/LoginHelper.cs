using Dapper;
using Model.DTO;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace DAL
{

    public class LoginHelper : BaseDBHelper
    {
        private IConfig _config;
        string _log = "";
        string _filePath = "";
        string _Skip_Otp = "";

        public LoginHelper(IConfig conifg) : base(conifg)
        {
            _config = conifg;
            _log = conifg.Log;
            _filePath = conifg.FilePath;
            _Skip_Otp = conifg.OTP_Validation;
        }
        public async Task<Response<dynamic>> Login(LoginEntity webLogin)
        {
            Response<dynamic> objResponse = new Response<dynamic>();
            eTokenService _eTokenService = new eTokenService();
            String Database = "adminboundary";
            string connectionString = $"{_config.ConnectionString}{Database}";
            _con = new Npgsql.NpgsqlConnection(connectionString);
            try
            {
                _con.Open();

                string querytype = "GetUserId";
                string[] param = { webLogin.user_name, webLogin.password };
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@querytype", querytype);
                parameters.Add("@param_array", param);
                IDbTransaction dbTransactiondbTransaction = _con.BeginTransaction();
                var _result = await _con.QueryFirstOrDefaultAsync<dynamic>("gis_db_access_users.fn_get_general_multipurpose", param: parameters, dbTransactiondbTransaction, commandType: CommandType.StoredProcedure);
                dbTransactiondbTransaction.Commit();
                if (_result != null && _result.user_name.Trim() == webLogin.user_name.Trim())
                {
                    var _eToken = _eTokenService.AccessTokenGenerate();
                    var _userId = _result.user_id;
                    var _user_name = _result.user_name;

                    querytype = "UpdateUserToken";
                    string[] param1 = { _eToken, _userId.ToString() };
                    DynamicParameters parameters1 = new DynamicParameters();
                    parameters1.Add("@querytype", querytype);
                    parameters1.Add("@param_array", param1);

                    IDbTransaction dbTransactiondbTransaction2 = _con.BeginTransaction();
                    var _ret_eToken = await _con.QueryFirstOrDefaultAsync<dynamic>("gis_db_access_users.fn_get_general_multipurpose", param: parameters1, dbTransactiondbTransaction2, commandType: CommandType.StoredProcedure);
                    dbTransactiondbTransaction2.Commit();

                    if (!string.IsNullOrEmpty(_ret_eToken.tokens))
                    {
                        querytype = "GetVarifirdToken";
                        string[] param2 = { _ret_eToken.tokens };
                        DynamicParameters parameters2 = new DynamicParameters();

                        parameters2.Add("@querytype", querytype);
                        parameters2.Add("@param_array", param2);

                        IDbTransaction dbTransactiondbTransaction1 = _con.BeginTransaction();
                        objResponse.Data = await _con.QueryFirstOrDefaultAsync<dynamic>("gis_db_access_users.fn_get_general_multipurpose", param: parameters2, dbTransactiondbTransaction1, commandType: CommandType.StoredProcedure);
                        dbTransactiondbTransaction1.Commit();
                        if (objResponse.Data != null)
                        {
                            objResponse.IsSuccess = true;
                            objResponse.Message = "Access Token  Verify Successfully";
                            objResponse.ResponseCode = 200;
                            objResponse.DataLength = 1;
                        }
                        else
                        {
                            objResponse.IsSuccess = false;
                            objResponse.Message = "Session Expired";
                            objResponse.ResponseCode = 440;
                        }
                    }
                    else
                    {
                        objResponse.IsSuccess = false;
                        objResponse.Message = "Login Faild";
                    }


                }
                else
                {
                    objResponse.IsSuccess = false;
                    objResponse.Message = "Incorrect Login ID or Password";
                }

                return objResponse;
            }
            catch (Exception ex)
            {
                Utility.LogService.WriteErrorLog("Login", ex.Message, _log);
                objResponse.IsSuccess = false;
                objResponse.Message = ex.Message;
            }
            return null;
        }


        public async Task<Response<dynamic>> RegisterUser(RegisterEntity webLogin)
        {
            Response<dynamic> objResponse = new Response<dynamic>();
            String Database = "adminboundary";
            string connectionString = $"{_config.ConnectionString}{Database}";
            _con = new Npgsql.NpgsqlConnection(connectionString);
            try
            {
                _con.Open();
                string querytype = "InsertUser";
                string[] param = { webLogin.first_name, webLogin.last_name, webLogin.mobile_number, webLogin.contact_email, webLogin.user_name, webLogin.password, webLogin.roll, webLogin.designation };
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@querytype", querytype);
                parameters.Add("@param_array", param);
                IDbTransaction dbTransactiondbTransaction = _con.BeginTransaction();
                var _result = await _con.QueryFirstOrDefaultAsync<dynamic>("gis_db_access_users.fn_get_general_multipurpose", param: parameters, dbTransactiondbTransaction, commandType: CommandType.StoredProcedure);
                dbTransactiondbTransaction.Commit();
                if (_result != null && _result.out_put_result == -2)
                {
                    objResponse.IsSuccess = false;
                    objResponse.Message = "Username Alredy Exist";

                }
                else if (_result != null && _result.out_put_result > 0)
                {
                    objResponse.IsSuccess = true;
                    objResponse.Message = "Register Successfully";
                }
                else
                {
                    objResponse.IsSuccess = false;
                    objResponse.Message = "Unable to register, pleae try again";
                }

                return objResponse;
            }
            catch (Exception ex)
            {
                Utility.LogService.WriteErrorLog("Login", ex.Message, _log);
                objResponse.IsSuccess = false;
                objResponse.Message = ex.Message;
            }
            return null;
        }

    }
}

