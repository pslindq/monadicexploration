using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class ShowMonadPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            // Is there a request in the query string?
            if (Request[ConfigurationManager.AppSettings["MonadQueryStringParam"]] != null && Request[ConfigurationManager.AppSettings["MonadQueryStringParam"]].Length > 0)
            {
                // Pull it and see what we find in the database
                string monadURL = Request[ConfigurationManager.AppSettings["MonadQueryStringParam"]];
                using (MonadModel context = new MonadModel())
                {
                    // Pull and sort by title for ease of finding things first
                    Monad monad = context.Monads.FirstOrDefault(s => s.URLSegment == monadURL);
                    if (monad == null)
                    {
                        // Nope. Naw. Ain't gonna happen. BONG...
                        Response.StatusCode = 404;
                        Response.End();
                    }
                    else
                    {
                        // Populate the visualization config scripts
                        WebControl configJSScript = (WebControl) this.Header.FindControl("configJSFile");
                        configScriptLink.Text = configScriptLink.Text.Replace("@@QUERYSTRING@@", ConfigurationManager.AppSettings["MonadQueryStringParam"] + "=" + monad.URLSegment);
                        // Initialize the client site routine for the visualization
                        ClientScript.RegisterStartupScript(this.GetType(), "init", "init();", true);
                        this.Title = monad.Title;
                    }
                }
            }
        }
    }
}