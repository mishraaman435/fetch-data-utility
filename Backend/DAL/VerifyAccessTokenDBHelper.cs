using Dapper;
using Model;
using Model.Entities;
using System.Data;
using System.Data.Common;
using Utility;

namespace DAL
{
    public class VerifyAccessTokenDBHelper : BaseDBHelper
    {
        private readonly IConfig _config;
      

        public VerifyAccessTokenDBHelper(IConfig conifg) : base(conifg)
        {
            _config = conifg;
           
        }


        public async Task<bool> VerifyAccessToken(string user_id, string eTokens)
        {
            try
            {
                _con.Open();
                string querytype = "GetVarifirdToken";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@userid", user_id);
                parameters.Add("@sessiontoken", eTokens);
                IDbTransaction dbTransaction = _con.BeginTransaction();
                var _res = await _con.QueryFirstOrDefaultAsync<bool>("SELECT public.verify_session(@userid,@sessiontoken)", parameters, dbTransaction );

                return _res;

            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                    "VerifyAccessToken",
                    "user_id: " + user_id + ", eTokens: " + eTokens,
                    "verify_session()",
                    ex.ToString());

            }
            return false;
        }
    }
}
