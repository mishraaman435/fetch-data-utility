using Dapper;
using Model;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace DAL
{
    public class ExpiryQuizHelper : BaseDBHelper
    {
        private readonly IConfig _config;
        private readonly QuizHelper _quizHelper;


        public ExpiryQuizHelper(IConfig config) : base(config)
        {
            _config = config;
            _quizHelper = new QuizHelper(config);

        }

        // =========================================================
        // 1️⃣ STUDENT-WISE EXPIRY CHECK
        // Safe to call multiple times (idempotent)
        // =========================================================
        public async Task EnsureStudentAttemptFinalized(long quizId, long studentId)
        {
            var attempt = await GetStudentAttemptMeta(quizId, studentId);

            if (attempt == null)
                return;

            //if (attempt.submission_status != 4) // 4=IN_PROGRESS
            //    return;

            if (DateTime.UtcNow >= attempt.autosubmit) //when auto submit is on
            {
                await ForceSubmitInternal(quizId, studentId);

                await _quizHelper.GenerateQuizRankCache(
                  new GenerateQuizRankCacheEntity { quiz_id = (int)quizId }
              );
            }
        }



        public async Task FinalizeExpiredAttemptsForQuiz(long quizId)
        {
            var expiredAttempts = await GetExpiredAttemptsForQuiz(quizId);

            bool anySubmitted = false;

            foreach (var attempt in expiredAttempts)
            {
                if (attempt.submission_status != 4)
                    continue;

                await ForceSubmitInternal(attempt.quiz_id, attempt.student_id);
                anySubmitted = true;
            }

            // 🔥 SINGLE CALL
            if (anySubmitted)
            {
                await _quizHelper.GenerateQuizRankCache(
                    new GenerateQuizRankCacheEntity { quiz_id = (int)quizId }
                );
            }
        }


        // =========================================================
        // 🔒 INTERNAL FORCE SUBMIT (SINGLE TRANSACTION)
        // =========================================================
        private async Task ForceSubmitInternal(long quizId, long studentId)
        {
            try
            {
                await _con.OpenAsync();
                using var tx = _con.BeginTransaction();

                // 1️⃣ Force Submit
                await ExecuteUpdate(
                    "ForceSubmitQuiz",
                    new[] { quizId.ToString(), studentId.ToString() },
                    tx
                );

                // 2️⃣ Calculate Result
                await ExecuteUpdate(
                    "CalculateQuizResult",
                    new[] { quizId.ToString(), studentId.ToString() },
                    tx
                );

                // 3️⃣ Generate Rank Cache
                //await ExecuteInsert(
                //    "GenerateQuizRankCache",
                //    new[] { quizId.ToString() },
                //    tx
                //);

                tx.Commit();
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                    "ExpiryQuizHelper.ForceSubmitInternal",
                    $"{quizId},{studentId}",
                    "Auto submit orchestration",
                    ex.ToString()
                );

                throw;
            }
            finally
            {
                if (_con.State == ConnectionState.Open)
                    await _con.CloseAsync();
            }
        }

        // =========================================================
        // 🔍 FETCH SINGLE STUDENT ATTEMPT META
        // =========================================================
        private async Task<dynamic?> GetStudentAttemptMeta(long quizId, long studentId)
        {
            await _con.OpenAsync();
            using var tx = _con.BeginTransaction();

            var p = new DynamicParameters();
            p.Add("@querytype", "GetStudentQuizAttemptMeta");
            p.Add("@param_array", new[] { quizId.ToString(), studentId.ToString() });

            var cursor = await _con.ExecuteScalarAsync<string>(
                "SELECT public.fn_get_general_multipurpose(@querytype, @param_array)",
                p, tx);

            if (cursor == null)
            {
                tx.Commit();
                await _con.CloseAsync();
                return null;
            }

            var result = (await _con.QueryAsync<dynamic>(
                $"FETCH ALL FROM \"{cursor}\";", transaction: tx)).FirstOrDefault();

            await _con.ExecuteAsync($"CLOSE \"{cursor}\";", transaction: tx);
            tx.Commit();
            await _con.CloseAsync();

            return result;
        }

        // =========================================================
        // 🔍 FETCH ALL EXPIRED ATTEMPTS (QUIZ-WISE)
        // =========================================================
        private async Task<IEnumerable<dynamic>> GetExpiredAttemptsForQuiz(long quizId)
        {
            await _con.OpenAsync();
            using var tx = _con.BeginTransaction();

            var p = new DynamicParameters();
            p.Add("@querytype", "GetExpiredAttemptsForQuiz");
            p.Add("@param_array", new[] { quizId.ToString() });

            var cursor = await _con.ExecuteScalarAsync<string>(
                "SELECT public.fn_get_general_multipurpose(@querytype, @param_array)",
                p, tx);

            if (cursor == null)
            {
                tx.Commit();
                await _con.CloseAsync();
                return Enumerable.Empty<dynamic>();
            }

            var result = await _con.QueryAsync<dynamic>(
                $"FETCH ALL FROM \"{cursor}\";", transaction: tx);

            await _con.ExecuteAsync($"CLOSE \"{cursor}\";", transaction: tx);
            tx.Commit();
            await _con.CloseAsync();

            return result;
        }

        // =========================================================
        // 🔧 COMMON DB HELPERS
        // =========================================================
        private async Task ExecuteUpdate(string queryType, string[] param, IDbTransaction tx)
        {
            var p = new DynamicParameters();
            p.Add("@querytype", queryType);
            p.Add("@param_array", param);

            await _con.ExecuteAsync(
                "select public.fn_set_update_multipurpose(@querytype, @param_array)",
                p, tx
            );
        }

        private async Task ExecuteInsert(string queryType, string[] param, IDbTransaction tx)
        {
            var p = new DynamicParameters();
            p.Add("@querytype", queryType);
            p.Add("@param_array", param);

            await _con.ExecuteAsync(
                "select public.fn_set_insert_multipurpose(@querytype, @param_array)",
                p, tx
            );
        }
    }
}

