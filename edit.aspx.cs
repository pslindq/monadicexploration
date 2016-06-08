using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditMonadPage : System.Web.UI.Page
{
    /// <summary>
    /// Get or set the working reference to the Monad this editing is for
    /// </summary>
    private Monad MyMonad {
        get
        {
            if (ViewState["MonadQueryStringParam"] == null)
            {
                // Pull the monad information
                string monadURL = Request[ConfigurationManager.AppSettings["MonadQueryStringParam"]];
                using (MonadModel context = new MonadModel())
                {
                    Monad monad = context.Monads.FirstOrDefault(s => s.URLSegment == monadURL);
                    if (monad == null)
                    {
                        // Nope. Naw. Ain't gonna happen. BONG...
                        Response.StatusCode = 404;
                        Response.End();
                    }
                    else
                    {
                        // Store this in the viewstate for reference
                        ViewState["MonadQueryStringParam"] = monad;
                    }
                }
            }
            return (Monad)ViewState["MonadQueryStringParam"];
        }
        set
        {
            ViewState["MonadQueryStringParam"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Test for the password first - make sure an admin is logged in
        if (Session[ConfigurationManager.AppSettings["SessionPasswordKey"]] == null || 
            ((string)Session[ConfigurationManager.AppSettings["SessionPasswordKey"]] != ConfigurationManager.AppSettings["MasterPassword"] &&
            (string)Session[ConfigurationManager.AppSettings["SessionPasswordKey"]] != MyMonad.AdminPWD))
        {
            // Redirect to the login as this isn't a valid authentication to be here
            Response.Redirect("/" + MyMonad.URLSegment + "/admin", true);
        }

        // Reset our form action to something cleaner
        form1.Action = "/edit.aspx";
        // set the heading on the page
        headingText.Text = "Monadic Explorations - Editing: " + MyMonad.Title;
    }

    /// <summary>
    /// For when the user wants to cancel all editing no matter where they are 
    /// </summary>
    protected void cancelAllButton_Click(object sender, EventArgs e)
    {
        // Return to the visualization
        Response.Redirect("/" + MyMonad.URLSegment);
    }
    
    /// <summary>
    /// For when the user wants to cancel a particular editing action 
    /// </summary>
    protected void cancelButton_Click(object sender, EventArgs e)
    {
        // Based on the current screen, decide what to do
        if (editBasicScreen.Visible)
        {
            errorText.Text = "<p class=\"confirm\">Basic Information Editing Cancelled</p>";
            landingScreen.Visible = true;
            editBasicScreen.Visible = false;
        }
        else if (listNodeTypesScreen.Visible)
        {
            errorText.Text = "<p class=\"confirm\">Node Type Editing Cancelled</p>";
            landingScreen.Visible = true;
            listNodeTypesScreen.Visible = false;
        }
    }

    #region Edit Basic Information  

    /// <summary>
    /// For when the user wants to edit the basic information
    /// </summary>
    protected void editBasicLink_Click(object sender, EventArgs e)
    {
        // Clear all previous error text
        errorText.Text = null;
        // Switch screens
        landingScreen.Visible = false;
        editBasicScreen.Visible = true;
        // Initialize the basic editing form with the known values
        monadTitle.Text = MyMonad.Title;
        monadLinkTo.Text = MyMonad.URL;
        urlSegment.Text = MyMonad.URLSegment;
        adminPWD.Text = MyMonad.AdminPWD;
        showNodeTypesFlag.Checked = MyMonad.ShowNodeTypes;
    }

    /// <summary>
    /// For when the user wants to save basic info updates
    /// </summary>
    protected void saveBasicInfoButton_Click(object sender, EventArgs e)
    {
        // Clear all previous error text
        errorText.Text = null;
        // Pull the context and try to save the updates to it
        using (MonadModel context = new MonadModel())
        {
            try
            {
                Monad workingMonad = context.Monads.First(m => m.MonadID == MyMonad.MonadID);
                workingMonad.Title = monadTitle.Text;
                workingMonad.URL = monadLinkTo.Text;
                workingMonad.ShowNodeTypes = showNodeTypesFlag.Checked;
                workingMonad.URLSegment = urlSegment.Text;
                workingMonad.AdminPWD = adminPWD.Text;
                context.SaveChanges();
                MyMonad = workingMonad;
                // Go back to the main screen
                errorText.Text = "<p class=\"confirm\">Basic Information Updated</p>";
                landingScreen.Visible = true;
                editBasicScreen.Visible = false;
                // Update the page heading
                headingText.Text = "Monadic Explorations - Editing: " + MyMonad.Title;
            }
            catch (Exception ex)
            {
                // Get to the last inner exception and display that
                while (ex.InnerException != null) ex = ex.InnerException;
                errorText.Text = "<p class=\"error\">An error occured: " + ex.Message + "</p>";
                // May not be all to friendly, but we aren't worried about UX in this
            }
        }
    }

    #endregion

    #region Edit Node Types

    /// <summary>
    /// For when the user wants to view and potentially edit the node types
    /// </summary>
    protected void editNodeTypesLink_Click(object sender, EventArgs e)
    {
        // Clear all previous error text
        errorText.Text = null;
        // Switch screens
        landingScreen.Visible = false;
        listNodeTypesScreen.Visible = true;
        // Initialize the repeater with the current list
        using (MonadModel context = new MonadModel())
        {
            nodeTypeList.DataSource = context.Monads.First(m => m.MonadID == MyMonad.MonadID).
                                      NodeTypes.OrderBy(nt => nt.Sequence).
                                      ToArray();
            nodeTypeList.DataBind();
        }       
    }

    /// <summary>
    /// For when a node type from the DB is bound to our viewable repeater list
    /// </summary>
    protected void nodeTypeList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            // Cast the incoming dataitem
            NodeType nodeType = (NodeType)e.Item.DataItem;
            // Populate the main link button information for this type
            LinkButton lb = (LinkButton)e.Item.FindControl("nodeTypeEdit");
            lb.ForeColor = System.Drawing.ColorTranslator.FromHtml("#"+nodeType.Color);
            lb.Text = nodeType.Name;
            lb.CommandArgument = nodeType.NodeTypeID.ToString();
        } 
    }

    /// <summary>
    /// When the user wants to edit a specific node type
    /// </summary>
    protected void nodeTypeEdit_Click(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// When the user wants a node type moved up
    /// </summary>
    protected void nodeTypeUp_Click(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// When the user wants a node type moved down
    /// </summary>
    protected void nodeTypeDown_Click(object sender, EventArgs e)
    {

    }

    #endregion

}
