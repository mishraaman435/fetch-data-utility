using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace grid
{
    public partial class _Default : Page
    {
        private DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dt = new DataTable();
                dt.Columns.Add("Name");
                dt.Columns.Add("Gender");
                dt.Columns.Add("Comments");
                ViewState["am"] = dt;
            }

        }

        protected void submit_Click(object sender, EventArgs e)
        {
            dt= ViewState["am"] as DataTable;
            string name = dropdown1.Text;
            string gender = RadioButton1.Checked ? "Male" : RadioButton2.Checked ? "Female" : "";
            string com = textwa.Text;
            DataRow dr = dt.NewRow();
            dr["Name"] = name;
            dr["Gender"] = gender;
            dr["Comments"] = com;
            dt.Rows.Add(dr);
            ViewState["am"]=dt;
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();

            dropdown1.SelectedIndex = 0;
            RadioButton1.Checked = false;
            RadioButton2.Checked = false;
            textwa.Text = string.Empty;


        }
    }
}