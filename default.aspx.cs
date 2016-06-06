using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class DefaultHomePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // On initial page load pull the list of monads from the database
        // and bind them to the repeater
        using (MonadModel context = new MonadModel())
        {
            // Pull and sort by title for ease of finding things first
            var monads = from s in context.Monads
                         select s;
            monads.OrderBy(s => s.Title);
            availableMonads.DataSource = monads.ToArray();
            availableMonads.DataBind();
        }
    }

    protected void availableMonads_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        // Set the working item's anchor tag to point to the correct monad info
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Monad currentMonad = (Monad)e.Item.DataItem;
            HtmlAnchor anchor = (HtmlAnchor)e.Item.FindControl("anchor");
            anchor.HRef = "~/" + currentMonad.URLSegment;
            anchor.InnerText = currentMonad.Title;
        }
    }
}
