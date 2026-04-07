using DAL;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class MasterManager : IDisposable
    {
        private IConfig _config;
        public MasterManager(IConfig config)
        {
            _config = config;
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public async Task<dynamic> GetDesignation(DesignationEntity webLogin)
        {
            using (MasterHelper db = new MasterHelper(_config))
            {
                return await db.GetDesignation(webLogin);
            }
        }
        public async Task<dynamic> GetListOfCandidate(CandidateEntity webLogin)
        {
            using (MasterHelper db = new MasterHelper(_config))
            {
                return await db.GetListOfCandidate(webLogin);
            }
        }
        public async Task<dynamic> sendsmstocandidate(SMSEntity _InputParam)
        {
            using (MasterHelper db = new MasterHelper(_config))
            {
                return await db.sendsmstocandidate(_InputParam);
            }

        }

        
    }
}
