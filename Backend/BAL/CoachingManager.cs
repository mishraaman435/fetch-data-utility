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
    public class CoachingManager : IDisposable
    {
        private readonly IConfig _config;
        private bool _disposed;

        public CoachingManager(IConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }



        public async Task<string> OnboardStudent(List<OnboardStudentEntity> InputParam, int user_id, int coaching_id)
        {
            using (var db = new CoachingHelper(_config))
            {
                return await db.OnboardStudent(InputParam, user_id, coaching_id);
            }
        }


        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentList(GetStudentListEntity InputParam)
        {
            using (var db = new CoachingHelper(_config))
            {
                return await db.GetStudentList(InputParam);
            }
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

        ~CoachingManager()
        {
            Dispose(false);
        }

    }
}
