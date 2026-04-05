using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class SchemaEntity
    {
        public string Token { get; set; }
        public string Database { get; set; }
    }
    public class DatabseEntity
    {
        public string Token { get; set; }

    }
    public class TransactionEntity
    {
        public string Token { get; set; }
        public string user_id { get; set; }

    }
    public class TableEntity
    {
        public string Token { get; set; }

        public string Schema { get; set; }
        public string Database { get; set; }

    }
    public class FunctionEntity
    {
        public string Token { get; set; }
        public string Schema { get; set; }
        public string Database { get; set; }
        public int qid {  get; set; } 
    }
    public class ScriptEntity
    {
        public string Token { get; set; }
        public string Schema { get; set; }
        public string name { get; set; }
        public string Database { get; set; }
        public int qid { get; set; }
    }
    public class QueryEntity
    {
        public string Token { get; set; }

        public string Database { get; set; }
        public string Query { get; set; }

        [JsonIgnore]
        public int user_id { get; set; }
    }
    public class InsertQueryEntity
    {
        public int user_id { get; set; }

        public string query_val { get; set; }
    }
}
