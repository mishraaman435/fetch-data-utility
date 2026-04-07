using Model.Entities;
using Npgsql;
using System.Data;
using System.Reflection;

namespace DAL
{
    public class BaseDBHelper : IDisposable
    {
        protected NpgsqlConnection _con = null;
        public readonly IConfig _config;
        public BaseDBHelper(IConfig config)
        {
            _config = config;
        }

        public void Dispose()
        {
            if (_con != null)
            {
                if (_con.State == System.Data.ConnectionState.Open)
                {
                    _con.Close();
                }
                _con.Dispose();
            }            
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
