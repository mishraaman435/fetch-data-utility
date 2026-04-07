using BAL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using Newtonsoft.Json;
using Utility;

namespace Fetchdata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private IConfig _config;
        string _log = "";
        String _key = "";

        public MasterController(IConfig config)
        {
            _config = config;
            _log = config.Log;
            _key = config.key;
        }


        [HttpPost("GetDesignation")]

        public async Task<IActionResult> GetDesignation(EncryptedDataEntity data)
        {
            IActionResult response = BadRequest();
            var _InputParam = JsonConvert.DeserializeObject<DesignationEntity>(DecryptDataService.DecryptString(data.Data, _key));
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
                    
                    using (MasterManager db = new MasterManager(_config))
                    {
                        var result = await db.GetDesignation(_InputParam);
                        return Ok(result);
                    }


                }
                catch (Exception ex)
                {

                    LogService.WriteErrorLog("GetDesignation", ex.ToString(), _log);
                }


            }
            else
            {

            }
            return response;
        }


        [HttpPost("sendsmstocandidate")]
        public async Task<IActionResult> sendsmstocandidate(SMSEntity _InputParam)
        {
            IActionResult response = BadRequest();
                try
                {
                    using (MasterManager db = new MasterManager(_config))
                    {
                        var result = await db.sendsmstocandidate(_InputParam);
                        return Ok(result);
                    }
                }
                catch (Exception ex)
                {
                    LogService.WriteErrorLog("sendsmstocandidate", ex.ToString(), _log);
                }
           
            return response;
        }

        [HttpPost("GetListOfCandidate")]

        public async Task<IActionResult> GetListOfCandidate(CandidateEntity _InputParam)
        {
            IActionResult response = BadRequest();
            
                try
                {

                    using (MasterManager db = new MasterManager(_config))
                    {
                        var result = await db.GetListOfCandidate(_InputParam);
                        return Ok(result);
                    }


                }
                catch (Exception ex)
                {

                    LogService.WriteErrorLog("GetDesignation", ex.ToString(), _log);
                }


           
            return response;
        }


    }


}
