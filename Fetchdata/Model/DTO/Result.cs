using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class Response<T>
    {

        [DataMember(Order = 0)]
        public T Data { get; set; }


        [DataMember(Order = 1)]
        public String Message { get; set; }

        [DataMember(Order = 2)]

        public Boolean IsSuccess { get; set; }


        [DataMember(Order = 3)]

        public int DataLength { get; set; }

        [DataMember(Order = 4)]
        public int ResponseCode { get; set; }

    }

}
