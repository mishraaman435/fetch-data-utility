using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mapview
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void subm_Click(object sender, EventArgs e)
        {
            if (float.TryParse(lati.Text, out float lat) && float.TryParse(longi.Text, out float lng))
            {
                string script = $"showbutn('{lat}', '{lng}');";
                ClientScript.RegisterStartupScript(this.GetType(), "showbutn", script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "some thing went wrong", true);
            }
        }
    }
}