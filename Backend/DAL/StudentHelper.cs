using Dapper;
using Model;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace DAL
{
    public class StudentHelper : BaseDBHelper
    {
        private readonly IConfig _config;

        private readonly string _filePath;
        public StudentHelper(IConfig config) : base(config)
        {
            _config = config;

            _filePath = config.FilePath;
        }

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentCarts(GetStudentCartsEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetStudentCarts");
                parameters.Add("@param_array", new[] { InputParam.coaching_id.ToString(), InputParam.subscription_id.ToString(), InputParam.course_id.ToString() });

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
                    objResponse.DisplayMessage = "Get Student Cart Details";
                    objResponse.DataLength = objResponse.Data.Count();
                }
                else
                {
                    objResponse.StatusCode = 401;
                    objResponse.IsSuccess = false;
                    objResponse.DisplayMessage = "Student Cart List Not Found";
                    objResponse.DataLength = 0;
                }
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                    "GetStudentCarts",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetStudentCarts)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentAssignedQuizzes(GetStudentAssignedQuizzesEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetStudentAssignedQuizzes");
                parameters.Add("@param_array", new[] { InputParam.coaching_id.ToString(), InputParam.course_id.ToString() });

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
                    objResponse.DisplayMessage = "Get Quiz Question Details";
                    objResponse.DataLength = objResponse.Data.Count();
                }
                else
                {
                    objResponse.StatusCode = 401;
                    objResponse.IsSuccess = false;
                    objResponse.DisplayMessage = "Quiz Question List Not Found";
                    objResponse.DataLength = 0;
                }
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                    "GetStudentAssignedQuizzes",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetStudentAssignedQuizzes)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> ValidateQuizAccessForStudent(ValidateQuizAccessForStudentEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "ValidateQuizAccessForStudent");
                parameters.Add("@param_array", new[] { InputParam.quiz_id.ToString(), InputParam.student_id.ToString() });

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
                    objResponse.DisplayMessage = "Get Quiz Question Details";
                    objResponse.DataLength = objResponse.Data.Count();
                }
                else
                {
                    objResponse.StatusCode = 401;
                    objResponse.IsSuccess = false;
                    objResponse.DisplayMessage = "Quiz Question List Not Found";
                    objResponse.DataLength = 0;
                }
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                    "ValidateQuizAccessForStudent",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(ValidateQuizAccessForStudent)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizSectionSummary(GetQuizSectionSummaryEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetQuizSectionSummary");
                parameters.Add("@param_array", new[] { InputParam.quiz_id.ToString() });

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
                    objResponse.DisplayMessage = "Get Quiz Question Details";
                    objResponse.DataLength = objResponse.Data.Count();
                }
                else
                {
                    objResponse.StatusCode = 401;
                    objResponse.IsSuccess = false;
                    objResponse.DisplayMessage = "Quiz Question List Not Found";
                    objResponse.DataLength = 0;
                }
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                    "GetQuizSectionSummary",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetQuizSectionSummary)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizMetaForRules(GetQuizMetaForRulesEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetQuizMetaForRules");
                parameters.Add("@param_array", new[] { InputParam.quiz_id.ToString() });

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
                    objResponse.DisplayMessage = "Get Quiz Question Details";
                    objResponse.DataLength = objResponse.Data.Count();
                }
                else
                {
                    objResponse.StatusCode = 401;
                    objResponse.IsSuccess = false;
                    objResponse.DisplayMessage = "Quiz Question List Not Found";
                    objResponse.DataLength = 0;
                }
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                    "GetQuizMetaForRules",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetQuizMetaForRules)",
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
        public async Task<string> StartQuizAttempt(StartQuizAttemptEntity InputParam)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "StartQuizAttempt";
                DynamicParameters parameters = new DynamicParameters();


                string[] param = { InputParam.quiz_id.ToString(), InputParam.student_id.ToString() };

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
                   "StartQuizAttempt",
                   InputParam.ToString() ?? string.Empty,
                   "fn_set_insert_multipurpose(StartQuizAttempt)",
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

    }
}