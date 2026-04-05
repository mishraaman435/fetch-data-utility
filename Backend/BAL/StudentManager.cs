using DAL;
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

namespace BAL
{
    public class StudentManager : IDisposable
    {
        private readonly IConfig _config;
        private bool _disposed;

        public StudentManager(IConfig config)
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

                // Dispose unmanaged resources if any
                _disposed = true;
            }
        }

        ~StudentManager()
        {
            Dispose(false);
        }

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentCarts(GetStudentCartsEntity InputParam)
        {
            using (var db = new StudentHelper(_config))
            {
                return await db.GetStudentCarts(InputParam);
            }
        }
 
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentAssignedQuizzes(GetStudentAssignedQuizzesEntity InputParam)
        {
            using (var db = new StudentHelper(_config))
            {
                return await db.GetStudentAssignedQuizzes(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> ValidateQuizAccessForStudent(ValidateQuizAccessForStudentEntity InputParam)
        {
            using (var db = new StudentHelper(_config))
            {
                return await db.ValidateQuizAccessForStudent(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizSectionSummary(GetQuizSectionSummaryEntity InputParam)
        {
            using (var db = new StudentHelper(_config))
            {
                return await db.GetQuizSectionSummary(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizMetaForRules(GetQuizMetaForRulesEntity InputParam)
        {
            using (var db = new StudentHelper(_config))
            {
                return await db.GetQuizMetaForRules(InputParam);
            }
        }
        public async Task<string> StartQuizAttempt(StartQuizAttemptEntity InputParam)
        {
            using (var db = new StudentHelper(_config))
            {
                return await db.StartQuizAttempt(InputParam);
            }
        }

    }
}
