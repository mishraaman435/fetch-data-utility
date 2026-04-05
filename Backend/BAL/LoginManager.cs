using DAL;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{

    public class LoginManager : IDisposable
    {
        private IConfig _config;
        public LoginManager(IConfig config)
        {
            _config = config;
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public async Task<dynamic> Login(LoginEntity webLogin)
        {

            using (LoginHelper db = new LoginHelper(_config))
            {
                return await db.Login(webLogin);
            }

        }
        public async Task<dynamic> RegisterUser(RegisterEntity webLogin)
        {

            using (LoginHelper db = new LoginHelper(_config))
            {
                
                return await db.RegisterUser(webLogin);
            }
            return 1;

        }

    }
}
