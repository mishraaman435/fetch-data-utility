using DAL;
using Model;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class VerifyAccessTokenManager : IDisposable
    {
        private IConfig _config;
        public VerifyAccessTokenManager(IConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public async Task<bool> VerifyAccessToken(string user_id, string etoken)
        {
            using (VerifyAccessTokenDBHelper db = new VerifyAccessTokenDBHelper(_config))
            {
                return await db.VerifyAccessToken(user_id, etoken);
            }
        }
    }
}
