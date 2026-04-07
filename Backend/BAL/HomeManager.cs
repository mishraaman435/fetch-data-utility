
using DAL;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class HomeManager : IDisposable
    {
        private IConfig _config;
        public HomeManager(IConfig config)
        {
            _config = config;
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public async Task<dynamic> GetDataBase()
        {

            using (HomeHelper db = new HomeHelper(_config))
            {
                return await db.GetDataBase();
            }

        }
        
        public async Task<dynamic> GetSchemas(SchemaEntity _InputParam)
        {

            using (HomeHelper db = new HomeHelper(_config))
            {
                return await db.GetSchemas(_InputParam);
            }

        }
        public async Task<dynamic> GetTransaction(TransactionEntity _InputParam)
        {

            using (HomeHelper db = new HomeHelper(_config))
            {
                return await db.GetTransaction(_InputParam);
            }

        }

        public async Task<dynamic> GetTable(TableEntity _InputParam)
        {

            using (HomeHelper db = new HomeHelper(_config))
            {
                return await db.GetTable(_InputParam);
            }

        }
        public async Task<dynamic> GetFunctions(FunctionEntity _InputParam)
        {

            using (HomeHelper db = new HomeHelper(_config))
            {
                return await db.GetFunctions(_InputParam);
            }

        }
        public async Task<dynamic> GetScript(ScriptEntity _InputParam)
        {

            using (HomeHelper db = new HomeHelper(_config))
            {
                return await db.GetScript(_InputParam);
            }

        }
        public async Task<dynamic> GetQuery(QueryEntity _InputParam)
        {

            using (HomeHelper db = new HomeHelper(_config))
            {
                return await db.GetQuery(_InputParam);
            }

        }

    }
}
