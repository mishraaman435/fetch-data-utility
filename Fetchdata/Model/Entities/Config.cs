using Microsoft.Extensions.Configuration;

namespace Model.Entities
{
    public interface IConfig
    {
        string ConnectionString { get; }
        string Log { get; }
        string FilePath { get; }
        string GSTIN_Validation { get; }

        string OTP_Validation { get; }
        string LogEnabled { get; }

        string ReturnException { get; }
        string FilePathDownload { get; }
        string key { get; }

    }
    public class Config : IConfig

    {
        readonly IConfiguration _config;
        public Config(IConfiguration conifg)
        {
            _config = conifg;
        }
        public string ConnectionString => _config["DB:ConnectionString"];
        public string Log => _config["Log:path"];
        public string FilePath => _config["FilePath:path"];
        public string GSTIN_Validation => _config["GSTIN_Validation:Valuelength"];

        public string OTP_Validation => _config["OTP_Validation:OTP"];

        public string LogEnabled => _config["LogEnabled:LogEnable"];

        public string ReturnException => _config["LogEnabled:ReturnException"];

        public string FilePathDownload => _config["FilePathDownload:path"];
        public string key => _config["AppSettings:ApiKey"];


    }
}
