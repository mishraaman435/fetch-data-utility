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

namespace BAL
{
    public class ExamManager : IDisposable
    {
        private readonly IConfig _config;
        private bool _disposed;

        public ExamManager(IConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources if any
                }
                _disposed = true;
            }
        }

        ~ExamManager()
        {
            Dispose(false);
        }


        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizQuestionsForStudent(GetQuizQuestionsForStudentEntity InputParam)
        {
            using (var db = new ExamHelper(_config))
            {
                return await db.GetQuizQuestionsForStudent(InputParam);
            }
        }

        public async Task<string> SaveQuestionResponse(SaveQuestionResponseEntity InputParam)
        {
            using (var db = new ExamHelper(_config))
            {
                return await db.SaveQuestionResponse(InputParam);
            }
        }

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetResumeExamState(GetResumeExamStateEntity InputParam)
        {
            using (var db = new ExamHelper(_config))
            {
                return await db.GetResumeExamState(InputParam);
            }
        }


        public async Task<string> SubmitQuiz(SubmitQuizEntity InputParam)
        {
            using (var db = new ExamHelper(_config))
            {
                return await db.SubmitQuiz(InputParam);
            }
        }

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetRemainingExamTime(GetRemainingExamTimeEntity InputParam)
        {
            using (var db = new ExamHelper(_config))
            {
                return await db.GetRemainingExamTime(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuestionPaletteStatus(GetQuestionPaletteStatusEntity InputParam)
        {
            using (var db = new ExamHelper(_config))
            {
                return await db.GetQuestionPaletteStatus(InputParam);
            }
        }
    }
}
