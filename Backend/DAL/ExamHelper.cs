using DAL;
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

    public class ExamHelper : BaseDBHelper
    {
        private readonly IConfig _config;

        private readonly string _filePath;
        private readonly ExpiryQuizHelper _expiryQuizHelper;

        public ExamHelper(IConfig config) : base(config)
        {
            _config = config;
            _expiryQuizHelper = new ExpiryQuizHelper(config);

            _filePath = config.FilePath;
        }

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizQuestionsForStudent(GetQuizQuestionsForStudentEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetQuizQuestionsForStudent");
                parameters.Add("@param_array", new[] { InputParam.quiz_id.ToString(), InputParam.student_id.ToString(), InputParam.serial_no.ToString(), InputParam.section });

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
                    "GetQuizQuestionsForStudent",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetQuizQuestionsForStudent)",
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

        public async Task<string> SaveQuestionResponse(SaveQuestionResponseEntity InputParam)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "SaveQuestionResponse";
                DynamicParameters parameters = new DynamicParameters();

                string[] param = { InputParam.quiz_id.ToString(), InputParam.student_id.ToString(), InputParam.question_id.ToString(), InputParam.course_id.ToString(), InputParam.coaching_id.ToString(), InputParam.answer_value, InputParam.response_type.ToString(), InputParam.duration.ToString(), InputParam.section };

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
                   "SaveQuestionResponse",
                   InputParam?.ToString() ?? string.Empty,
                   "fn_set_insert_multipurpose(SaveQuestionResponse)",
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

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetResumeExamState(GetResumeExamStateEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _expiryQuizHelper.EnsureStudentAttemptFinalized(InputParam.quiz_id, (int)InputParam.student_id);


                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetResumeExamState");
                parameters.Add("@param_array", new[] { InputParam.quiz_id.ToString(), InputParam.student_id.ToString(), });

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
                    "GetResumeExamState",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetResumeExamState)",
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

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetRemainingExamTime(GetRemainingExamTimeEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _expiryQuizHelper.EnsureStudentAttemptFinalized(InputParam.quiz_id, (int)InputParam.student_id);

                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetRemainingExamTime");
                parameters.Add("@param_array", new[] { InputParam.quiz_id.ToString(), InputParam.student_id.ToString(), });

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
                    "GetRemainingExamTime",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetRemainingExamTime)",
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

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuestionPaletteStatus(GetQuestionPaletteStatusEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetQuestionPaletteStatus");
                parameters.Add("@param_array", new[] { InputParam.quiz_id.ToString(), InputParam.student_id.ToString(), InputParam.section });

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
                    "GetQuestionPaletteStatus",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetQuestionPaletteStatus)",
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

        public async Task<string> SubmitQuiz(SubmitQuizEntity InputParam)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "SubmitQuiz";
                DynamicParameters parameters = new DynamicParameters();

                string[] param = { InputParam.quiz_id.ToString(), InputParam.student_id.ToString(), InputParam.submission_status.ToString() };

                parameters.Add("@querytype", querytype);
                parameters.Add("@param_array", param);

                var _result = await _con.QueryFirstOrDefaultAsync<string>("select public.fn_set_update_multipurpose(@querytype, @param_array)", parameters, dbTransaction);
                dbTransaction.Commit();
                _con.Close();

                return _result;
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                   "SubmitQuiz",
                   InputParam?.ToString() ?? string.Empty,
                   "fn_set_update_multipurpose(SubmitQuiz)",
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
