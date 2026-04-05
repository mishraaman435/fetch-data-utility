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
    public class QuizAnalyticHelper : BaseDBHelper
    {
        private readonly IConfig _config;

        private readonly string _filePath;
        private readonly ExpiryQuizHelper _expiryQuizHelper;



        public QuizAnalyticHelper(IConfig config) : base(config)
        {
            _config = config;
            _expiryQuizHelper = new ExpiryQuizHelper(config);


            _filePath = config.FilePath;
        }
        public async Task<string> CalculateQuizResult(CalculateQuizResultEntity InputParam)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "CalculateQuizResult";
                DynamicParameters parameters = new DynamicParameters();
                string[] param = { InputParam.quiz_id.ToString(), InputParam.student_id.ToString() };
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentQuizResult(GetStudentQuizResultEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _expiryQuizHelper.EnsureStudentAttemptFinalized(InputParam.quiz_id, (int)InputParam.student_id);

                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetStudentQuizResult");
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
                    "GetStudentQuizResult",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetStudentQuizResult)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentQuestionWiseResult(GetStudentQuestionWiseResultEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _expiryQuizHelper.EnsureStudentAttemptFinalized(InputParam.quiz_id, (int)InputParam.student_id);

                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetStudentQuestionWiseResult");
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
                    "GetStudentQuestionWiseResult",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetStudentQuestionWiseResult)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentOverallPerformance(GetStudentOverallPerformanceEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                //await _expiryQuizHelper.EnsureStudentAttemptFinalized(InputParam.quiz_id, (int)InputParam.student_id);

                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetStudentOverallPerformance");
                parameters.Add("@param_array", new[] { InputParam.student_id.ToString() });

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
                    "GetStudentOverallPerformance",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetStudentOverallPerformance)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizSectionWiseScore(GetQuizSectionWiseScoreEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _expiryQuizHelper.EnsureStudentAttemptFinalized(InputParam.quiz_id, (int)InputParam.student_id);

                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetQuizSectionWiseScore");
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
                    "GetQuizSectionWiseScore",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetQuizSectionWiseScore)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetCourseWisePerformance(GetCourseWisePerformanceEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetCourseWisePerformance");
                parameters.Add("@param_array", new[] { InputParam.student_id.ToString() });

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
                    "GetCourseWisePerformance",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetCourseWisePerformance)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizRankList(GetQuizRankListEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _expiryQuizHelper.FinalizeExpiredAttemptsForQuiz(InputParam.quiz_id);

                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetQuizRankList");
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
                    "GetQuizRankList",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetQuizRankList)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentRankInQuiz(GetStudentRankInQuizEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetStudentRankInQuiz");
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
                    "GetStudentRankInQuiz",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetStudentRankInQuiz)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizPercentileList(GetQuizPercentileListEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _expiryQuizHelper.FinalizeExpiredAttemptsForQuiz(InputParam.quiz_id);

                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetQuizPercentileList");
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
                    "GetQuizPercentileList",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetQuizPercentileList)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentPercentileInQuiz(GetStudentPercentileInQuizEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetStudentPercentileInQuiz");
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
                    "GetStudentPercentileInQuiz",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetStudentPercentileInQuiz)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetDifficultyWisePerformance(GetDifficultyWisePerformanceEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                //await _expiryQuizHelper.FinalizeExpiredAttemptsForQuiz(InputParam.quiz_id);
                await _expiryQuizHelper.EnsureStudentAttemptFinalized(InputParam.quiz_id, (int)InputParam.student_id);


                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetDifficultyWisePerformance");
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
                    "GetDifficultyWisePerformance",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetDifficultyWisePerformance)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetDifficultyWiseAccuracy(GetDifficultyWiseAccuracyEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                //await _expiryQuizHelper.FinalizeExpiredAttemptsForQuiz(InputParam.quiz_id);
                await _expiryQuizHelper.EnsureStudentAttemptFinalized(InputParam.quiz_id, (int)InputParam.student_id);

                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetDifficultyWiseAccuracy");
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
                    "GetDifficultyWiseAccuracy",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetDifficultyWiseAccuracy)",
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
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetSectionWiseProgress(GetSectionWiseProgressEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _expiryQuizHelper.EnsureStudentAttemptFinalized(InputParam.quiz_id, (int)InputParam.student_id);

                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetSectionWiseProgress");
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
                    "GetSectionWiseProgress",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetSectionWiseProgress)",
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
      
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizLeaderboardFromCache(GetQuizLeaderboardFromCacheEntity InputParam)
        {
            var objResponse = new DQLResponse<IEnumerable<dynamic>>();

            try
            {
                await _expiryQuizHelper.FinalizeExpiredAttemptsForQuiz(InputParam.quiz_id);

                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetQuizLeaderboardFromCache");
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
                    "GetQuizLeaderboardFromCache",
                    InputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetQuizLeaderboardFromCache)",
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
