using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DMLResponse
    {

        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string DisplayMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string ResponseCode { get; set; }
    }


    public class DQLResponse<T>
    {

        public T Data { get; set; }
        public int StatusCode { get; set; }
        public Boolean IsSuccess { get; set; }
        public string DisplayMessage { get; set; }
        public string ErrorMessage { get; set; }
        public int DataLength { get; set; }

    }
    public class LoginResponse<T>
    {

        public T Data { get; set; }
        public string DisplayMessage { get; set; }
        public int StatusCode { get; set; }
        public Boolean IsSuccess { get; set; }
        public string jwt_token { get; set; }
        public string ErrorMessage { get; set; }
        public int DataLength { get; set; }

    }
}
