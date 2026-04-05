using DAL;
using Model;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class QuestionManager : IDisposable
    {
        private readonly IConfig _config;
        private bool _disposed;

        public QuestionManager(IConfig config)
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

        ~QuestionManager()
        {
            Dispose(false);
        }



        public async Task<int> InsertQuestion(List<InsertQuestionEntity> InputParam, int user_id)
        {
            using (var db = new QuestionHelper(_config))
            {
                return await db.InsertQuestion(InputParam, user_id);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuestionList(GetQuestionListEntity InputParam)
        {
            using (var db = new QuestionHelper(_config))
            {
                return await db.GetQuestionList(InputParam);
            }
        }
       
    }
}
