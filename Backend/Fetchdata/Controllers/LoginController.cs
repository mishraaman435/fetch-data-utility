using BAL;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using Newtonsoft.Json;
using Utility;

namespace Fetchdata.Controllers
{
    [AllowWithoutAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfig _config;
        string _log = "";
        String _key = "";

        public LoginController(IConfig config)
        {
            _config = config;
            _log = config.Log;
            _key = config.key;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(EncryptedDataEntity data)
        {
            IActionResult response = BadRequest();
            var webLogin = JsonConvert.DeserializeObject<LoginEntity>(DecryptDataService.DecryptString(data.Data, _key));
            try
            {
                using (LoginManager mgr = new LoginManager(_config))
                {
                    var result = await mgr.Login(webLogin);
                    if (result.IsSuccess)
                    {
                        var token = Guid.NewGuid().ToString(); // ya secure random string
                        Response.Cookies.Append("auth_token", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTime.UtcNow.AddHours(4)
                        });
                    }
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                Utility.LogService.WriteErrorLog("Login", ex.ToString(), _log);
            }
            return response;
        }

        [HttpPost("InsertUser")]

        public async Task<IActionResult> RegisterUser(EncryptedDataEntity data)
        {
            IActionResult response = BadRequest();
            var _InputParam = JsonConvert.DeserializeObject<RegisterEntity>(DecryptDataService.DecryptString(data.Data, _key));
            bool check = false;
            access_tokenDto access_TokenDto = new access_tokenDto();
            access_TokenDto.tokens = _InputParam.Token;
            using (VerifyAccessTokenManager mgr = new VerifyAccessTokenManager(_config))
            {
                var result1 = await mgr.VerifyAccessToken(access_TokenDto);
                if (result1.IsSuccess)
                {
                    check = result1.IsSuccess;
                }
                else
                {
                    return Ok(result1);
                }
            }

            if (check == true)
            {
                try
                {
                    using (LoginManager mgr = new LoginManager(_config))
                    {
                        var result = await mgr.RegisterUser(_InputParam);
                        return Ok(result);
                    }


                }
                catch (Exception ex)
                {

                    LogService.WriteErrorLog("GetRegister", ex.ToString(), _log);
                }


            }
            else
            {

            }
            return response;
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (Request.Cookies.TryGetValue("auth_token", out var token))
            {
                //TokenService.Remove(token);
                Response.Cookies.Delete("auth_token");
            }

            return Ok(new { message = "Logged out" });
        }

    }
}
