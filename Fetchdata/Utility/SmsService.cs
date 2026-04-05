using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utility
{
    public class SmsService
    {

        static String username = "User_name";
        static String password = "_paswword";
        static String senderid = "sender_id";
        static String secureKey = "security_key";
        public string sendSingleSMS(string mobileNo, string message, string templateid)

        {
            try
            {
                //objLogger.Write_InfoLog(message + "-" + templateid);

                //Latest Generated Secure Key
                Stream dataStream;

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //forcing .Net framework to use TLSv1.2

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT");

                request.ProtocolVersion = HttpVersion.Version10;
                request.KeepAlive = false;
                request.ServicePoint.ConnectionLimit = 1;

                //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
                ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";

                request.Method = "POST";



                String encryptedPassword = encryptedPasswod(password);
                String NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
                String smsservicetype = "singlemsg"; //For single message.

                String query = "username=" + HttpUtility.UrlEncode(username.Trim()) +
                    "&password=" + HttpUtility.UrlEncode(encryptedPassword) +

                    "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +

                    "&content=" + HttpUtility.UrlEncode(message.Trim()) +

                    "&mobileno=" + HttpUtility.UrlEncode(mobileNo) +

                    "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
                  "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +
                  "&templateid=" + HttpUtility.UrlEncode(templateid.Trim());



                byte[] byteArray = Encoding.ASCII.GetBytes(query);

                request.ContentType = "application/x-www-form-urlencoded";

                request.ContentLength = byteArray.Length;



                dataStream = request.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();

                WebResponse response = request.GetResponse();

                String Status = ((HttpWebResponse)response).StatusDescription;

                dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                String responseFromServer = reader.ReadToEnd();

                reader.Close();

                dataStream.Close();

                response.Close();

                //objLogger.Write_InfoLog("sendSingleSMS-Info-" + responseFromServer);

                return responseFromServer;
            }
            catch (Exception ex)
            {
                // objLogger.Write_ErrorLog("sendSingleSMS-Error-" + ex.Message);
                return ex.Message;
            }

        }
        protected static String encryptedPasswod(String password)
        {
            byte[] encPwd = Encoding.UTF8.GetBytes(password);
            //static byte[] pwd = new byte[encPwd.Length];
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA1");
            byte[] pp = sha1.ComputeHash(encPwd);
            // static string result = System.Text.Encoding.UTF8.GetString(pp);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in pp)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        protected static String hashGenerator(String Username, String sender_id, String message, String secure_key)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
            //static byte[] pwd = new byte[encPwd.Length];
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
            byte[] sec_key = sha1.ComputeHash(genkey);
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sec_key.Length; i++)
            {
                sb1.Append(sec_key[i].ToString("x2"));
            }
            return sb1.ToString();
        }

    }
}
