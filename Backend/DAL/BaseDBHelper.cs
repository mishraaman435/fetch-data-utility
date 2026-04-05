
using Model;
using Npgsql;
using System.Data;
using System.Reflection;
namespace DAL
{
    public class BaseDBHelper : IDisposable
    {
        public NpgsqlConnection _con = null;
        public readonly IConfig _config;
        public BaseDBHelper(IConfig conifg)
        {
            _config = conifg;

            _con = new NpgsqlConnection(_config.ConnectionString);
        }
        public void Dispose()
        {
            if (_con != null && _con.State == System.Data.ConnectionState.Open)
            {
                _con.Close();
            }
            _con.Dispose();
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
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
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
