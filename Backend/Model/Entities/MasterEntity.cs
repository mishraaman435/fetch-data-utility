using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class DesignationEntity
    {
        public string Token { get; set; }
    }

    public class SMSEntity
    {
        public int from { get; set; }
        public int to { get; set; }
        public int other { get; set; }

    }

    public class CandidateEntity
    {
        public int from { get; set; }
        public int to { get; set; }

        public int smsflg { get; set; }


    }


}
