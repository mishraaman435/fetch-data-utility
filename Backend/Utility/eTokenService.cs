
using System.Data;


namespace Utility
{
    public class eTokenService
    {
        public  string AccessTokenGenerate()
        {
            var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var resultToken = new string(
               Enumerable.Repeat(allChar, 8)
               .Select(token => token[random.Next(token.Length)]).ToArray());
           // string authToken = resultToken.ToString();
           // return authToken;

            Guid guid = Guid.NewGuid();    
            string authToken = guid.ToString();
            return authToken;
        }

        //public bool VerifyAccessToken(string access_token)
        //{
        //    bool check;
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt = dataObj.ExecuteDataTable("select * from forest_plantation.user_detail where access_token='" + access_token + "'", ConnStrAdmin);
        //        if (dt.Rows.Count > 0)
        //        {
        //            check = true;
        //        }
        //        else
        //        {
        //            //check for other user
        //            dt = dataObj.ExecuteDataTable("select * from forest_plantation.login_detail where access_token='" + access_token + "'", ConnStrAdmin);
        //            if (dt.Rows.Count > 0)
        //            {
        //                check = true;
        //            }
        //            else
        //            {
        //                check = false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return check;
        //}
    }
}
