using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utility
{
    public class VerifyQuery
    {
        public (bool isUnSafe, string msg) IsQueryUnSafe(string query)
        {
            string[] restrictedKeywords = new[]
            {
            "INSERT", "UPDATE" , "DELETE", "TRUNCATE", "CREATE", "ALTER", "DROP", "REINDEX", "COMMIT",
            "CLUSTER", "VACUUM", "COMMENT", "GRANT", "REVOKE", "ANALYZE", "COPY", "LOCK", "EXECUTE", "gis_db_access_users"
        };

            string pattern = string.Join("|", restrictedKeywords.Select(Regex.Escape));
            Regex regex = new Regex($@"\b({pattern})\b", RegexOptions.IgnoreCase);

            Match match = regex.Match(query);

            if (match.Success)
            {
                if (match.Value.ToLower() == "gis_db_access_users") return (true, "Schema is not allowed, i.e. 'gis_db_access_users'.");

                else return (true, "Query contains restricted keyword, i.e. '" + match.Value.ToUpper() + "'");
            }



            return (false, null);
        }



    }
}
