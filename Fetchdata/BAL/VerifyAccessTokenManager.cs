using DAL;
using Model.DTO;
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
            _config = config;
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public async Task<Response<dynamic>> VerifyAccessToken(access_tokenDto access_TokenDto)
        {
            using (VerifyAccessTokenDBHelper db = new VerifyAccessTokenDBHelper(_config))
            {
                return await db.VerifyAccessToken(access_TokenDto);
            }
        }
    }
}
