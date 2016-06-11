using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LogoutPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Clear the session password key
        Session[ConfigurationManager.AppSettings["SessionPasswordKey"]] = null;
        // Go to the homepage
        Response.Redirect("/default.aspx"); 
    }
}