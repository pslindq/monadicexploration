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
            if (ViewState["EditingMonad"] == null)
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
                        ViewState["EditingMonad"] = monad;
                    }
                }
            }
            return (Monad)ViewState["EditingMonad"];
        }
        set
        {
            ViewState["EditingMonad"] = value;
        }
    }

    /// <summary>
    /// When the page loads...
    /// </summary>
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
            errorText.Text = null;
            landingScreen.Visible = true;
            listNodeTypesScreen.Visible = false;
        }
        else if (editNodeTypeScreen.Visible)
        {
            errorText.Text = "<p class=\"confirm\">Node Type Adding/Editing Cancelled</p>";
            listNodeTypesScreen.Visible = true;
            editNodeTypeScreen.Visible = false;
        }
        else if (slugSelectionScreen.Visible)
        {
            errorText.Text = "<p class=\"confirm\">Slug Image Selection Cancelled</p>";
            editNodeTypeScreen.Visible = true;
            slugSelectionScreen.Visible = false;
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
        // Initialize the repeater with the current list of node types
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
            // Populate the up and down arrows with the correct information
            ((LinkButton)e.Item.FindControl("nodeTypeUp")).CommandArgument = nodeType.NodeTypeID.ToString();
            ((LinkButton)e.Item.FindControl("nodeTypeDown")).CommandArgument = nodeType.NodeTypeID.ToString();
            // So at this point we have the node type ID stored in the command argument so later query
        }
    }

    /// <summary>
    /// When the user wants a node type moved up
    /// </summary>
    protected void nodeTypeUp_Click(object sender, EventArgs e)
    {
        using (MonadModel context = new MonadModel())
        {
            // Pull the full list in decending order
            NodeType[] nt = context.NodeTypes.Where(t => t.MonadID == MyMonad.MonadID).OrderBy(t => t.Sequence).ToArray();
            // Loop through and process accordingly and skipping the first record (which can't move up)
            for (int x = (nt.Length - 1); x > 0; x--)
            {
                if (nt[x].NodeTypeID == int.Parse(((LinkButton)sender).CommandArgument))
                {
                    nt[x].Sequence -= 1;
                    nt[x - 1].Sequence += 1;
                    x = 1;
                }
            }
            // Save the changes
            context.SaveChanges();
        }
        // Now rebind the list
        editNodeTypesLink_Click(sender, e);
    }

    /// <summary>
    /// When the user wants a node type moved down
    /// </summary>
    protected void nodeTypeDown_Click(object sender, EventArgs e)
    {
        using (MonadModel context = new MonadModel())
        {
            // Pull the full list in decending order
            NodeType[] nt = context.NodeTypes.Where(t => t.MonadID == MyMonad.MonadID).OrderBy(t => t.Sequence).ToArray();
            // Loop through and process accordingly skipping the first record
            for (int x = 0; x < (nt.Length - 1); x++)
            {
                if (nt[x].NodeTypeID == int.Parse(((LinkButton)sender).CommandArgument))
                {
                    nt[x].Sequence += 1;
                    nt[x + 1].Sequence -= 1;
                    x = nt.Length;
                }
            }
            // Save the changes
            context.SaveChanges();
        }
        // Now rebind the list
        editNodeTypesLink_Click(sender, e);
    }

    /// <summary>
    /// When the user wants to edit/add a specific node type
    /// </summary>
    protected void nodeTypeEdit_Click(object sender, EventArgs e)
    {
        // Clear any previous error
        errorText.Text = null;
        // Switch screens
        listNodeTypesScreen.Visible = false;
        editNodeTypeScreen.Visible = true;
        // Populate with the information from what's in the database
        using (MonadModel context = new MonadModel())
        {
            // Is this a new node type addition?
            if (((LinkButton)sender).ID == "nodeTypeAdd")
            {
                // Populate for a new record
                nodeTypeName.Text = null;
                nodeTypePluralName.Text = null;
                nodeTypeColor.Value = "#000000";
                slugName.Text = null;
                slugSample.Attributes["style"] = "display:block;background-color:black;height:50px;width:50px;background-size:100%;background-image:url('/app/img/na.png');border-radius:50%;";
                //And store the ID of this nodetype in the viewstate
                ViewState["EditingNodeType"] = 0;
                // We'll need this later for saving
            }
            else
            {
                // This is editing an existing node type
                int id = int.Parse(((LinkButton)sender).CommandArgument);
                NodeType nt = context.NodeTypes.First(t => t.NodeTypeID == id);
                nodeTypeName.Text = nt.Name;
                nodeTypePluralName.Text = nt.PluralName;
                nodeTypeColor.Value = "#" + nt.Color;
                slugName.Text = nt.SlugName;
                slugSample.Attributes["style"] = "display:block;background-color:black;height:50px;width:50px;background-size:100%;background-image:url('/app/img/" + slugName.Text + ".png');border-radius:50%;";
                // And store the ID of this nodetype in the viewstate
                ViewState["EditingNodeType"] = nt.NodeTypeID;
                // We'll need this later for saving
            }
        }
    }

    /// <summary>
    /// For when the user wants to select a new slug
    /// </summary>
    protected void slugSelectionButton_Click(object sender, EventArgs e)
    {
        // Clear any previous error
        errorText.Text = null;
        // Change screens
        editNodeTypeScreen.Visible = false;
        slugSelectionScreen.Visible = true;
        // Pull the list of slugs (trans PNG files only) from the file.io and list them out if we haven't already
        if (slugList.DataSource == null)
        {
            slugList.DataSource = System.IO.Directory.EnumerateFiles(Server.MapPath("~/app/img")).ToArray().Where(x => x.EndsWith(".png"));
            slugList.DataBind();
        }
    }

    /// <summary>
    /// When the slug list is populating
    /// </summary>
    protected void slugList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            // Link in the button images where we can reference them back
            ((LinkButton)e.Item.FindControl("slugSelectButton")).CommandArgument = e.Item.DataItem.ToString().Split(("\\").ToArray()).Last().Split('.').First();
            ((WebControl)e.Item.FindControl("slugSelectButton")).Attributes["style"] = "display:block;float:left; margin:0 3px 3px 0;background-color:black;height:50px;width:50px;background-size:100%;background-image:url('/app/img/" + e.Item.DataItem.ToString().Split(("\\").ToArray()).Last() + "'); border-radius:50%;";
        }
    }

    /// <summary>
    /// When a new slug is slected
    /// </summary>
    protected void slugSelectButton_Click(object sender, EventArgs e)
    {
        // Confirm the selection
        errorText.Text = "<p class=\"confirm\">New Slug Image Selected</p>";
        // Change screens
        editNodeTypeScreen.Visible = true;
        slugSelectionScreen.Visible = false;
        // And update the form fields and sample slug for the node type
        slugName.Text = ((LinkButton)sender).CommandArgument;
        slugSample.Attributes["style"] = "display:block;background-color:black;height:50px;width:50px;background-size:100%;background-image:url('/app/img/" + slugName.Text + ".png');border-radius:50%;";
    }

    /// <summary>
    /// For when the user wants to save node type information
    /// </summary>
    protected void saveNodeTypeButton_Click(object sender, EventArgs e)
    {
        // Clear any previous error messages
        errorText.Text = null;
        // Two possibilities, new record or existing node type
        int id = (int)ViewState["EditingNodeType"];
        NodeType nt;
        using (MonadModel context = new MonadModel())
        {
            if (id == 0)
            {
                // A new record!
                nt = new NodeType();
                context.NodeTypes.Add(nt);
                nt.MonadID = MyMonad.MonadID;
                // Set the sequence to be last
                nt.Sequence = context.Monads.First(m => m.MonadID == MyMonad.MonadID).NodeTypes.Count + 1;
            }   
            else
            {
                // An existing record - load fresh from the database
                nt = context.NodeTypes.First(t => t.NodeTypeID == id);
            }
            // Set the rest of the fields
            nt.Name = nodeTypeName.Text;
            nt.PluralName = nodeTypePluralName.Text;
            nt.SlugName = slugName.Text;
            nt.Color = nodeTypeColor.Value.Replace("#", "");
            try
            {
                // Try to save the changes
                context.SaveChanges();
                // Go back to the main node listing
                errorText.Text = "<p class=\"confirm\">Node Type Adding/Editing Completed</p>";
                listNodeTypesScreen.Visible = true;
                editNodeTypeScreen.Visible = false;
                // Rebind the list
                nodeTypeList.DataSource = context.Monads.First(m => m.MonadID == MyMonad.MonadID).
                                      NodeTypes.OrderBy(t => t.Sequence).
                                      ToArray();
                nodeTypeList.DataBind();
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
}
