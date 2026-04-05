using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MailRequest
    {
        public string apiKey { get; set; }
        public string senderName { get; set; }
        public string senderEmail { get; set; }
        public string toName { get; set; }
        public string toEmail { get; set; }
        public string subject { get; set; }
        public string htmlBody { get; set; }
        public List<RecipientDto>? bccList { get; set; }

    }
    public class RecipientDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
