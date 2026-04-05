using Dapper;
using Model;
using Model.Entities;
using Newtonsoft.Json;
using System.Data;
using Utility;


namespace DAL
{
    public class AuthDBHelper : BaseDBHelper
    {
        private readonly IConfig _config;

        private readonly string JwtKey;
        private readonly string JwtIssuer;
        private readonly string JwtAudience;
        private readonly string MailKey;

        public AuthDBHelper(IConfig config) : base(config)
        {
            _config = config;

            JwtKey = config.JwtKey;
            JwtIssuer = config.JwtIssuer;
            JwtAudience = config.JwtAudience;
            MailKey = config.MailKey;
        }

        public async Task<LoginResponse<IEnumerable<dynamic>>> Login(LoginEntity inputParam)
        {
            var objResponse = new LoginResponse<IEnumerable<dynamic>>();

            try
            {
                await _con.OpenAsync();
                using var dbTransaction = _con.BeginTransaction();

                var parameters = new DynamicParameters();
                parameters.Add("@querytype", "GetLogin");
                parameters.Add("@param_array", new[] { inputParam.user_name, inputParam.password });

                var cursorName = await _con.ExecuteScalarAsync<string>("SELECT public.fn_get_general_multipurpose(@querytype, @param_array)", parameters, dbTransaction);
                if (cursorName != null)
                {
                    objResponse.Data = await _con.QueryAsync<dynamic>(
                    $"FETCH ALL FROM \"{cursorName}\";",
                    transaction: dbTransaction
                );

                    await _con.ExecuteAsync($"CLOSE \"{cursorName}\";", transaction: dbTransaction);

                }

                dbTransaction.Commit();

                if (objResponse.Data?.Any() == true)
                {

                    objResponse.StatusCode = 200;
                    objResponse.IsSuccess = true;
                    objResponse.DisplayMessage = "Get Login Details";
                    objResponse.DataLength = objResponse.Data.Count();
                    var user = objResponse.Data.FirstOrDefault();
                    if (user != null)
                    {
                        var updatedToken = await getUpdatedToken(user.user_id.ToString());

                        var jWTService = new JWTService();
                        objResponse.jwt_token = jWTService.GenerateJwtToken(user.user_id.ToString(), updatedToken, user.coaching_id, 102, JwtIssuer, JwtAudience, JwtKey);
                    }
                }
                else
                {
                    objResponse.StatusCode = 401;
                    objResponse.IsSuccess = false;
                    objResponse.DisplayMessage = "Login List Not Found";
                    objResponse.DataLength = 0;
                }
            }

            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                    "Login",
                    inputParam?.ToString() ?? string.Empty,
                    "fn_get_general_multipurpose(GetLogin)",
                    ex.ToString()
                );

                objResponse.StatusCode = 400;
                objResponse.IsSuccess = false;
                objResponse.ErrorMessage = ex.Message;
                objResponse.DisplayMessage = "Something went wrong. Please try again later.";
            }
            finally
            {
                if (_con.State == ConnectionState.Open)
                {
                    _con.Close();
                }
            }

            return objResponse;
        }
        public async Task<string> getUpdatedToken(string user_id)
        {

            try
            {
                using var dbTransaction = _con.BeginTransaction();
                var parameters = new DynamicParameters();
                parameters.Add("@userid", user_id.ToString());
                var _result = await _con.QueryFirstOrDefaultAsync<string>("SELECT public.update_token(@userid)", param: parameters, dbTransaction);
                dbTransaction.Commit();
                return _result;
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                  "getUpdatedToken",
                  user_id,
                  "public.update_toke",
                  ex.ToString()
              );
                return "";
            }



        }

        public async Task<string> DemoRegister(EnquiryRequest InputParam)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "InsertForDemo";
                DynamicParameters parameters = new DynamicParameters();

                string JsonData = "[" + JsonConvert.SerializeObject(InputParam) + "]";

                string[] param = { JsonData };

                parameters.Add("@querytype", querytype);
                parameters.Add("@param_array", param);

                var _result = await _con.QueryFirstOrDefaultAsync<string>("select public.fn_set_insert_multipurpose(@querytype, @param_array)", parameters, dbTransaction);
                dbTransaction.Commit();
                _con.Close();
                if (_result == "Success")
                {
                    var mailService = new MailService();
                    _ = await mailService.SendDemoMail(InputParam, MailKey);

                }
                return _result;
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                   "DemoRegister",
                   InputParam?.ToString() ?? string.Empty,
                   "fn_set_insert_multipurpose(InsertForDemo)",
                   ex.ToString());
                return ex.ToString();
            }
            finally
            {
                if (_con.State == ConnectionState.Open)
                {
                    _con.Close();
                }
            }
            return "something went wrong !!";
        }
        public async Task<string> RegisterCoaching(RegisterCoachingEntity InputParam)
        {
            try
            {
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "InsertRegisterRequest";
                DynamicParameters parameters = new DynamicParameters();

                string JsonData = "[" + JsonConvert.SerializeObject(InputParam) + "]";

                string[] param = { JsonData };

                parameters.Add("@querytype", querytype);
                parameters.Add("@param_array", param);

                var _result = await _con.QueryFirstOrDefaultAsync<string>("select public.fn_set_insert_multipurpose(@querytype, @param_array)", parameters, dbTransaction);
                dbTransaction.Commit();
                _con.Close();
                if (_result == "Success")
                {
                    var mailService = new MailService();
                    _ = await mailService.registarMail(InputParam, MailKey);

                }
                return _result;
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                   "RegisterCoaching",
                   InputParam?.ToString() ?? string.Empty,
                   "fn_set_insert_multipurpose(InsertRegisterRequest)",
                   ex.ToString());
                return ex.ToString();
            }
            finally
            {
                if (_con.State == ConnectionState.Open)
                {
                    _con.Close();
                }
            }
            return "something went wrong !!";
        }

        public async Task<string> VerifyMail(VerifyMailEntity inputParam)
        {
            try
            {
                string otp_val = OTPService.GenerateAlphanumericOTP(6);
                _con.Open();
                IDbTransaction dbTransaction = _con.BeginTransaction();
                string querytype = "InsertOtp";
                DynamicParameters parameters = new DynamicParameters();

                string[] param = { "email", inputParam.email, inputParam.purpose, otp_val };

                parameters.Add("@querytype", querytype);
                parameters.Add("@param_array", param);

                var _result = await _con.QueryFirstOrDefaultAsync<string>("select public.fn_set_insert_multipurpose(@querytype, @param_array)", parameters, dbTransaction);
                dbTransaction.Commit();
                _con.Close();
                if (_result == "Success")
                {
                    var mailservice = new MailService();
                    _ = mailservice.verifyMail(inputParam, otp_val, MailKey);

                }
                return _result;
            }
            catch (Exception ex)
            {
                _ = GCloudLogService.LogErrorAsync(
                   "VerifyMail",
                   inputParam?.ToString() ?? string.Empty,
                   "fn_set_insert_multipurpose(InsertOtp)",
                   ex.ToString());
                return ex.ToString();
            }
            finally
            {
                if (_con.State == ConnectionState.Open)
                {
                    _con.Close();
                }
            }
            return "something went wrong !!";
        }
    }
}
