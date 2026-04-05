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
using static System.Collections.Specialized.BitVector32;

namespace DAL
{
    public class QuizHelper : BaseDBHelper
    {
        private readonly IConfig _config;

        private readonly string _filePath;
        public QuizHelper(IConfig config) : base(config)
        {
            _config = config;

            _filePath = config.FilePath;
        }


        public async Task<string> InsertQuiz(List<InsertQuizEntity> InputParam, int user_id, int coaching_id)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "InsertQuizInfo";
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
                   "InsertQuiz",
                   InputParam[0]?.ToString() ?? string.Empty,
                   "fn_set_insert_multipurpose(InsertQuizInfo)",
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

        public async Task<string> MapQuizQuestion(List<MapQuizQuestionEntity> InputParam, int user_id, int coaching_id)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "InsertQuizMapping";
                DynamicParameters parameters = new DynamicParameters();

                string JsonData = JsonConvert.SerializeObject(InputParam);

                string[] param = { JsonData, user_id.ToString(), coaching_id.ToString(), InputParam[0].quiz_id.ToString() };

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
                   "MapQuizQuestion",
                   InputParam[0]?.ToString() ?? string.Empty,
                   "fn_set_insert_multipurpose(InsertQuizMapping)",
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

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizInfoList(GetQuizInfoEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetQuizInfoList");
                parameters.Add("@param_array", new[] { InputParam.coaching_id.ToString() });

                var cursorName = await _con.ExecuteScalarAsync<string>("SELECT public.fn_get_general_multipurpose(@querytype, @param_array)", parameters, dbTransaction);

                objResponse.Data = await _con.QueryAsync<dynamic>($"FETCH ALL FROM \"{cursorName}\";", transaction: dbTransaction);
                await _con.ExecuteAsync($"CLOSE \"{cursorName}\";", transaction: dbTransaction);

                dbTransaction.Commit();


                if (objResponse.Data?.Any() == true)
                {
                    objResponse.StatusCode = 200;
                    objResponse.IsSuccess = true;
                    objResponse.DisplayMessage = "Get Quiz Details";
                    objResponse.DataLength = objResponse.Data.Count();
                }
                else
                {
                    objResponse.StatusCode = 401;
                    objResponse.IsSuccess = false;
                    objResponse.DisplayMessage = "Quiz List Not Found";
                    objResponse.DataLength = 0;
                }
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                    "GetQuizInfo",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetQuizInfo)",
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

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizForReview(GetQuizForReviewEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetQuizForReview");
                parameters.Add("@param_array", new[] { InputParam.quiz_id.ToString(), InputParam.section });

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
                    "GetQuizForReview",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetQuizQuestionsByQuizId_ForInstructor)",
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

        public async Task<string> ReviewAndScheduleQuiz(ReviewAndScheduleQuizEntity InputParam, int user_id, int coaching_id)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "Submit_and_Schedule";
                DynamicParameters parameters = new DynamicParameters();

                string[] param = { InputParam.quiz_id.ToString(), user_id.ToString(), coaching_id.ToString() };

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
                   "ReviewAndScheduleQuiz",
                   InputParam?.ToString() ?? string.Empty,
                   "fn_set_update_multipurpose(Submit_and_Schedule)",
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

        public async Task<string> LogTabSwitchEvent(LogTabSwitchEventEntity InputParam)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "LogTabSwitchEvent";
                DynamicParameters parameters = new DynamicParameters();

                string[] param = { InputParam.quiz_id.ToString(), InputParam.student_id.ToString(), InputParam.event_type.ToString(), InputParam.duration.ToString() };

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
                   "LogTabSwitchEvent",
                   InputParam?.ToString() ?? string.Empty,
                   "fn_set_update_multipurpose(LogTabSwitchEvent)",
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

        public async Task<string> UpdateMappedQuizQuestion(UpdateMappedQuizQuestionEntity InputParam, int user_id)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "UpdateMappedQuizQuestion";
                DynamicParameters parameters = new DynamicParameters();

                string[] param = { InputParam.mapping_id.ToString(), user_id.ToString(), InputParam.question_mark.ToString(), InputParam.negative_mark.ToString(), InputParam.section, InputParam.question_order.ToString(), InputParam.quiz_id.ToString() };

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
                   "UpdateMappedQuizQuestion",
                   InputParam?.ToString() ?? string.Empty,
                   "fn_set_update_multipurpose(UpdateMappedQuizQuestion)",
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

        public async Task<string> RemoveMappedQuizQuestion(RemoveMappedQuizQuestionEntity InputParam, int user_id)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "RemoveMappedQuizQuestion";
                DynamicParameters parameters = new DynamicParameters();

                string[] param = { InputParam.mapping_id.ToString(), user_id.ToString(),InputParam.quiz_id.ToString() };

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
                   "RemoveMappedQuizQuestion",
                   InputParam?.ToString() ?? string.Empty,
                   "fn_set_update_multipurpose(RemoveMappedQuizQuestion)",
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

        public async Task<string> GenerateQuizRankCache(GenerateQuizRankCacheEntity InputParam)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "GenerateQuizRankCache";
                DynamicParameters parameters = new DynamicParameters();
                string[] param = { InputParam.quiz_id.ToString() };
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
                   "GenerateQuizRankCache",
                   InputParam?.ToString() ?? string.Empty,
                   "fn_set_insert_multipurpose(GenerateQuizRankCache)",
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

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentRankFromCache(GetStudentRankFromCacheEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetStudentRankFromCache");
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
                    "GetStudentRankFromCache",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetStudentRankFromCache)",
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