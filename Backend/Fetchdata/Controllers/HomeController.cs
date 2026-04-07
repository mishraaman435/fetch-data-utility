using BAL;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using Newtonsoft.Json;
using Utility;

namespace Fetchdata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IConfig _config;
        string _log = "";
        string _key = "";

        public HomeController(IConfig config)
        {
            _config = config;
            _log = config.Log;
            _key = config.key;
        }

        [HttpPost("GetDataBase")]

        public async Task<IActionResult> GetDataBase(EncryptedDataEntity data)
        {
            IActionResult response = BadRequest();
            var _InputParam = JsonConvert.DeserializeObject<DatabseEntity>(DecryptDataService.DecryptString(data.Data, _key));
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
                    using (HomeManager mgr = new HomeManager(_config))
                    {

                        var result = await mgr.GetDataBase();
                        return Ok(result);

                    }

                }
                catch (Exception ex)
                {

                    LogService.WriteErrorLog("GetDataBase", ex.ToString(), _log);
                }


            }
            else
            {

            }
            return response;
        }


        [HttpPost("GetSchemas")]
        public async Task<IActionResult> GetSchemas(EncryptedDataEntity data)
        {
            IActionResult response = BadRequest();
            var _InputParam = JsonConvert.DeserializeObject<SchemaEntity>(DecryptDataService.DecryptString(data.Data, _key));

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

                    using (HomeManager mgr = new HomeManager(_config))
                    {
                        var result = await mgr.GetSchemas(_InputParam);
                        return Ok(result);

                    }

                }
                catch (Exception ex)
                {

                    LogService.WriteErrorLog("GetSchemas", ex.ToString(), _log);
                }


            }
            else
            {


            }

            return response;
        }

        [HttpPost("GetTable")]
        public async Task<IActionResult> GetTable(EncryptedDataEntity data)
        {
            IActionResult response = BadRequest();
            var _InputParam = JsonConvert.DeserializeObject<TableEntity>(DecryptDataService.DecryptString(data.Data, _key));

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

                    using (HomeManager mgr = new HomeManager(_config))
                    {
                        var result = await mgr.GetTable(_InputParam);
                        return Ok(result);

                    }

                }
                catch (Exception ex)
                {

                    LogService.WriteErrorLog("GetTable", ex.ToString(), _log);
                }

            }
            else
            {


            }
            return response;
        }

        [HttpPost("GetTransaction")]
        public async Task<IActionResult> GetTransaction(EncryptedDataEntity data)
        {
            IActionResult response = BadRequest();
            var _InputParam = JsonConvert.DeserializeObject<TransactionEntity>(DecryptDataService.DecryptString(data.Data, _key));

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

                    using (HomeManager mgr = new HomeManager(_config))
                    {
                        var result = await mgr.GetTransaction(_InputParam);
                        return Ok(result);

                    }

                }
                catch (Exception ex)
                {

                    LogService.WriteErrorLog("GetTransaction", ex.ToString(), _log);
                }

            }
            else
            {


            }
            return response;
        }

        [HttpPost("GetQuery")]
        public async Task<IActionResult> GetQuery(EncryptedDataEntity data)
        {
            IActionResult response = BadRequest();
            var _InputParam = JsonConvert.DeserializeObject<QueryEntity>(DecryptDataService.DecryptString(data.Data, _key));
            int user_id = 0;

            bool check = false;

            access_tokenDto access_TokenDto = new access_tokenDto();
            access_TokenDto.tokens = _InputParam.Token;
            using (VerifyAccessTokenManager mgr = new VerifyAccessTokenManager(_config))
            {
                var result1 = await mgr.VerifyAccessToken(access_TokenDto);
                if (result1.IsSuccess)
                {

                    check = result1.IsSuccess;
                    user_id = result1.Data.user_id;



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
                    _InputParam.user_id = user_id;
                    using (HomeManager mgr = new HomeManager(_config))
                    {
                        var result = await mgr.GetQuery(_InputParam);
                        return Ok(result);

                    }

                }
                catch (Exception ex)
                {

                    LogService.WriteErrorLog("GetQuery", ex.ToString(), _log);
                }

            }
            else
            {


            }
            return response;
        }

        [HttpPost("GetFunctionsOrViewsList")]
        public async Task<IActionResult> GetFunctions(EncryptedDataEntity data)
        {
            IActionResult response = BadRequest();
            var _InputParam = JsonConvert.DeserializeObject<FunctionEntity>(DecryptDataService.DecryptString(data.Data, _key));

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

                    using (HomeManager mgr = new HomeManager(_config))
                    {
                        var result = await mgr.GetFunctions(_InputParam);
                        return Ok(result);

                    }

                }
                catch (Exception ex)
                {

                    LogService.WriteErrorLog("GetFunctions", ex.ToString(), _log);
                }

            }
            else
            {


            }
            return response;
        }

        [HttpPost("GetScript")]
        public async Task<IActionResult> GetScript(EncryptedDataEntity data)
        {
            IActionResult response = BadRequest();
            var _InputParam = JsonConvert.DeserializeObject<ScriptEntity>(DecryptDataService.DecryptString(data.Data, _key));

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

                    using (HomeManager mgr = new HomeManager(_config))
                    {
                        var result = await mgr.GetScript(_InputParam);
                        return Ok(result);

                    }

                }
                catch (Exception ex)
                {

                    LogService.WriteErrorLog("GetScript", ex.ToString(), _log);
                }

            }
            else
            {


            }
            return response;
        }

    
}
}
