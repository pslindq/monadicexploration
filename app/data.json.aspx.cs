using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DataJsonPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            // Is there a request in the query string?
            if (Request[System.Configuration.ConfigurationManager.AppSettings["MonadQueryStringParam"]] != null && Request[ConfigurationManager.AppSettings["MonadQueryStringParam"]].Length > 0)
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
                        // Declare our working response string
                        string jsonResponse = string.Empty;
                        // Loop through each node type
                        int x = 0;
                        foreach (NodeType nodetype in monad.NodeTypes.OrderBy(s => s.Sequence))
                        {
                            // Loop through each node
                            foreach (Node node in nodetype.Nodes.OrderBy(s => s.Sequence))
                            {
                                // While I could do .tolower, I'm jsut going to use a private
                                // class defined in this page class to have everything formatted
                                // the way I want
                                jsonNode n = new jsonNode();
                                // Populate with the information from the record
                                n.id = node.NodeID;
                                n.type = x;
                                n.title = node.Title;
                                n.text = node.Text;
                                n.url = node.URL;
                                // Initialize the links member array to all the possible links
                                // Note - I'm assuming here the links are duplex, i.e. if 1 links to 2 
                                // then 2 is considered linked to 1, etc.
                                n.links = new int[node.NodeLinks1.Count + node.NodeLinks2.Count];
                                // Loop thro the node links and added them for rendering
                                int y = 0;
                                foreach (NodeLink link in node.NodeLinks1)
                                {
                                    n.links[y] = link.NodeID2;
                                    y++;
                                }
                                foreach (NodeLink link in node.NodeLinks2)
                                {
                                    n.links[y] = link.NodeID1;
                                    y++;
                                }
                                // Convert the object using our json serializer
                                jsonResponse += JsonConvert.SerializeObject(n);
                                // See https://github.com/JamesNK/Newtonsoft.Json for more information on this
                                // utility
                            }
                            x++;
                        }
                        // Send the completed response
                        Response.Write("[" + jsonResponse.Replace("}{","},{") + "]");
                        Response.End();
                    }
                }
            }
        }
    }

    private class jsonNode
    {
        public int id;
        public int type;
        public string title;
        public string text;
        public string url;
        public int[] links;
    }
}