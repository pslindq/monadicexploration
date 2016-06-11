using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ConfigJSPage : System.Web.UI.Page 
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
                        // Generate the config.js based on the original template
                        Response.Write(
                            "var app = { };" +
                            "app.node_types = [");
                        // Loop through the node types and generate the records for the javascript array
                        int x = 0;
                        foreach (NodeType nodetype in monad.NodeTypes.OrderBy(s => s.Sequence))
                        {
                            // Commit this to the response stream
                            Response.Write("{slug:'" + nodetype.SlugName + "',name:'" + nodetype.Name + "',names:'" + nodetype.PluralName + "',color:'#" + nodetype.Color + "'}");
                            if(x != monad.NodeTypes.Count - 1) Response.Write(",");
                            x++;
                        }
                        // And wrap it up...
                        Response.Write(
                            "];" +
                            "app.dataFile = '/app/data.json.aspx?" + ConfigurationManager.AppSettings["MonadQueryStringParam"] + "=" + monad.URLSegment + "'; " +
                            "app.showType = " + (monad.ShowNodeTypes?"true":"false") + "; " +
                            "app.title = '" + monad.Title + "'; " +
                            "app.url = '" + monad.URL + "'; "
                        );
                        Response.End();
                    }
                }
            }
        }
    }
}