using DAL;
using Model;
using Model.Entities;
using Model.Entity;
using System;
using System.Threading.Tasks;

namespace BAL
{
    public class AuthManager : IDisposable
    {
        private readonly IConfig _config;
        private bool _disposed;

        public AuthManager(IConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<LoginResponse<IEnumerable<dynamic>>> Login(LoginEntity InputParam)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(AuthManager));

            using (var db = new AuthDBHelper(_config))
            {
                return await db.Login(InputParam);
            }
        }


        public async Task<string> DemoRegister(EnquiryRequest InputParam)
        {
            using (var db = new AuthDBHelper(_config))
            {
                return await db.DemoRegister(InputParam);
            }
        }


        public async Task<string> RegisterCoaching(RegisterCoachingEntity inputParam)
        {
            using (var db = new AuthDBHelper(_config))
            {
                return await db.RegisterCoaching(inputParam);
            }
        }

        public async Task<string> VerifyMail(VerifyMailEntity inputParam)
        {
            using (var db = new AuthDBHelper(_config))
            {
                return await db.VerifyMail(inputParam);
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

        ~AuthManager()
        {
            Dispose(false);
        }
    }
}
