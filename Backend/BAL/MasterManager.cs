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
    public class MasterManager : IDisposable
    {
        private readonly IConfig _config;
        private bool _disposed;

        public MasterManager(IConfig config)
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

        ~MasterManager()
        {
            Dispose(false);
        }



        public async Task<DQLResponse<IEnumerable<dynamic>>> GetCoachingList(GetCoachingEntity InputParam)
        {
            using (var db = new MasterHelper(_config))
            {
                return await db.GetCoachingList(InputParam);
            }
        }

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetRoleList(RoleEntity InputParam)
        {
            using (var db = new MasterHelper(_config))
            {
                return await db.GetRoleList(InputParam);
            }
        }


        public async Task<DQLResponse<IEnumerable<dynamic>>> GetSubscriptionList(GetSubscriptionEntity InputParam)
        {
            using (var db = new MasterHelper(_config))
            {
                return await db.GetSubscriptionList(InputParam);
            }
        }

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetDemoRequest(GetDemoRequestEntity InputParam)
        {
            using (var db = new MasterHelper(_config))
            {
                return await db.GetDemoRequest(InputParam);
            }
        }


        public async Task<DQLResponse<IEnumerable<dynamic>>> GetCourse(GetCourseEntity InputParam)
        {
            using (var db = new MasterHelper(_config))
            {
                return await db.GetCourse(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetSubjects(GetSubjectsEntity InputParam)
        {
            using (var db = new MasterHelper(_config))
            {
                return await db.GetSubjects(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetTopics(GetTopicEntity InputParam)
        {
            using (var db = new MasterHelper(_config))
            {
                return await db.GetTopics(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuestionType()
        {
            using (var db = new MasterHelper(_config))
            {
                return await db.GetQuestionType();
            }
        }



    }
}
