

using Microsoft.Extensions.Configuration;

namespace Model
{
    public interface IConfig
    {
        string ConnectionString { get; }   
        string FilePath { get; }
        public string JwtKey { get; }
        public string JwtIssuer  { get;}
        public string JwtAudience { get;}
        public string MailKey { get; }


    }
    public class Config : IConfig

    {
        readonly IConfiguration _config;
        public Config(IConfiguration conifg)
        {
            _config = conifg;
        }
        public string ConnectionString => _config["DB:ConnectionString"];
        

        public string FilePath => _config["FilePath:path"];

        //public string LogEnabled => _config["LogEnabled:LogEnable"];

        public string MailKey=> _config["MailInfo:APIKey"];

        public string JwtKey => _config["Jwt:Key"];
        public string JwtIssuer => _config["Jwt:Issuer"];
        public string JwtAudience => _config["Jwt:Audience"];


        


     
  

    }
}
