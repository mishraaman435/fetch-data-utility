using Dapper;
using Model;
using Model.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace DAL
{
    public class CoachingHelper : BaseDBHelper
    {
        private readonly IConfig _config;

        private readonly string _filePath;
        public CoachingHelper(IConfig config) : base(config)
        {
            _config = config;

            _filePath = config.FilePath;
        }



        public async Task<string> OnboardStudent(List<OnboardStudentEntity> InputParam, int user_id, int coaching_id)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "InsertStudent";
                DynamicParameters parameters = new DynamicParameters();

                string JsonData = JsonConvert.SerializeObject(InputParam);

                string[] param = { JsonData, user_id.ToString(), coaching_id.ToString() };

                parameters.Add("@querytype", querytype);
                parameters.Add("@param_array", param);

                var _result = await _con.QueryFirstOrDefaultAsync<string>("select public.fn_set_insert_multipurpose(@querytype, @param_array)", parameters, dbTransaction);
                dbTransaction.Commit();
                _con.Close();

                return _result;
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                   "OnboardStudent",
                   InputParam[0]?.ToString() ?? string.Empty,
                   "fn_set_insert_multipurpose(InsertStudent)",
                   ex.ToString());
                return ex.ToString();
            }
            finally
            {
                if (_con.State == ConnectionState.Open)
                {
                    _con.Close();
                }
            }
            return "something went wrong !!";
        }


        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentList(GetStudentListEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetStudentList");
                parameters.Add("@param_array", new[] { InputParam.coaching_id.ToString(), InputParam.course_id.ToString(), InputParam.status, InputParam.search_text });

                var cursorName = await _con.ExecuteScalarAsync<string>("SELECT public.fn_get_general_multipurpose(@querytype, @param_array)", parameters, dbTransaction);

                if (cursorName != null)
                {
                    objResponse.Data = await _con.QueryAsync<dynamic>($"FETCH ALL FROM \"{cursorName}\";", transaction: dbTransaction);

                    await _con.ExecuteAsync($"CLOSE \"{cursorName}\";", transaction: dbTransaction);

                }
                dbTransaction.Commit();


                if (objResponse.Data?.Any() == true)
                {
                    objResponse.StatusCode = 200;
                    objResponse.IsSuccess = true;
                    objResponse.DisplayMessage = "Get Student Details";
                    objResponse.DataLength = objResponse.Data.Count();
                }
                else
                {
                    objResponse.StatusCode = 401;
                    objResponse.IsSuccess = false;
                    objResponse.DisplayMessage = "Student List Not Found";
                    objResponse.DataLength = 0;
                }
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                    "GetStudentList",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetStudentList)",
                    ex.ToString());

                objResponse.StatusCode = 400;
                objResponse.IsSuccess = false;
                objResponse.DisplayMessage = "Data Not Available";
                objResponse.ErrorMessage = ex.Message;
                objResponse.DataLength = 0;
            }
            finally
            {
                await _con.CloseAsync();
            }

            return objResponse;
        }


    }
}