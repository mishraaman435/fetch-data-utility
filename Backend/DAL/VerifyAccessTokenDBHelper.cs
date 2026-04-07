using Dapper;
using Model.DTO;
using Model.Entities;
using System.Data;

namespace DAL
{
    public class VerifyAccessTokenDBHelper : BaseDBHelper
    {
        private IConfig _config;
        string _log = "";
        string _filePath = "";
        string _ReturnException = "";
        public VerifyAccessTokenDBHelper(IConfig conifg) : base(conifg)
        {
            _config = conifg;
            _log = conifg.Log;
            _ReturnException = conifg.ReturnException;
        }


        public async Task<Response<dynamic>> VerifyAccessToken(access_tokenDto access_TokenDto)
        {
            Response<dynamic> objResponse = new Response<dynamic>();
            String Database = "adminboundary";
            string connectionString = $"{_config.ConnectionString}{Database}";
            _con = new Npgsql.NpgsqlConnection(connectionString);
            try
            {
                _con.Open();
                string querytype = "GetVarifirdToken";
                string[] param3 = { access_TokenDto.tokens };
                DynamicParameters parameters3 = new DynamicParameters();
                parameters3.Add("@querytype", querytype);
                parameters3.Add("@param_array", param3);
                IDbTransaction dbTransactiondbTransaction1 = _con.BeginTransaction();

                objResponse.Data = await _con.QueryFirstOrDefaultAsync<dynamic>("gis_db_access_users.fn_get_general_multipurpose", param: parameters3, dbTransactiondbTransaction1, commandType: CommandType.StoredProcedure);

                dbTransactiondbTransaction1.Commit();

                if (objResponse.Data != null)
                {
                    objResponse.IsSuccess = true;
                    objResponse.Message = "Access Token  Verify Successfully";
                    objResponse.ResponseCode = 200;
                }
                else
                {
                    objResponse.IsSuccess = false;
                    objResponse.Message = "Session Expired";
                    objResponse.ResponseCode = 440;
                }

            }
            catch (Exception ex)
            {
                Utility.LogService.WriteErrorLog("VerifyUser", ex.Message, _log);
                objResponse.IsSuccess = false;
                if (_ReturnException == "0")
                    objResponse.Message = "Data Not Available";
                else
                    objResponse.Message = ex.Message;
                objResponse.ResponseCode = 440;
            }
            return objResponse;
        }

    }
}
