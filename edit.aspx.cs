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
        else if (listNodesScreen.Visible)
        {
            errorText.Text = null;
            landingScreen.Visible = true;
            listNodesScreen.Visible = false;
        }
        else if (editNodeScreen.Visible)
        {
            errorText.Text = "<p class=\"confirm\">Node Adding/Editing Cancelled</p>";
            listNodesScreen.Visible = true;
            editNodeScreen.Visible = false;
        }
        else if (fileSelectScreen.Visible)
        {
            errorText.Text = "<p class=\"confirm\">Local File Selection Cancelled</p>";
            editNodeScreen.Visible = true;
            fileSelectScreen.Visible = false;
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
        // Set up the flag for deleting
        deleteMonad.CommandArgument = "false";
    }

    // For when the user wants to delete the monad
    protected void deleteMonad_Click(object sender, EventArgs e)
    {
        // Clear previous errors
        errorText.Text = null;
        if (deleteMonad.CommandArgument == "false")
        {
            // Update the error text for a confirmation and set the delete flag
            // to process the next time the user clicks
            deleteMonad.CommandArgument = "true";
            errorText.Text = "<p class=\"error\">Are you sure you want to delete this monad?<br/>Confirm by selecting the 'Delete' button once more.<br/>This will delete all the underlying data and return you to the homepage!!</p>";
        }
        else if (deleteMonad.CommandArgument == "true")
        {
            // This is a confirmed delete...
            // Use a context to loop all referenced records and delete
            using (MonadModel context = new MonadModel())
            {
                try
                {
                    Monad monad = context.Monads.First(m => m.MonadID == MyMonad.MonadID);
                    // Loop the node types
                    foreach (NodeType nt in monad.NodeTypes.ToArray())
                    {
                        // Loop the nodes
                        foreach (Node n in nt.Nodes.ToArray())
                        {
                            // Remove the links
                            foreach (NodeLink nl in n.NodeLinks1.ToArray()) context.NodeLinks.Remove(nl);
                            foreach (NodeLink nl in n.NodeLinks2.ToArray()) context.NodeLinks.Remove(nl);
                            // Remove the node
                            context.Nodes.Remove(n);
                        }
                        // Remove the node type
                        context.NodeTypes.Remove(nt);
                    }
                    // Remove the monad itself
                    context.Monads.Remove(monad);
                    // And save the changes
                    context.SaveChanges();
                    // Return to the homepage
                    Response.Redirect("/default.aspx?monad_deleted=true");
                }
                catch (Exception ex)
                {
                    // Get to the last inner exception and display that
                    while (ex.InnerException != null) ex = ex.InnerException;
                    errorText.Text = "<p class=\"error\">An error occured: " + ex.Message + "</p>";
                }
            }
        }
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
                deleteNodeTypeButton.Enabled = false;
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
                deleteNodeTypeButton.Enabled = true;
                deleteNodeTypeButton.CommandArgument = "false";
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

    /// <summary>
    /// For when the user wants to delete a node type
    /// </summary>
    protected void deleteNodeTypeButton_Click(object sender, EventArgs e)
    {
        // Clear previous errors
        errorText.Text = null;
        if (deleteNodeTypeButton.CommandArgument == "false")
        {
            // Update the error text for a confirmation and set the delete flag
            // to process the next time the user clicks
            deleteNodeTypeButton.CommandArgument = "true";
            errorText.Text = "<p class=\"error\">Are you sure you want to delete this node type?<br/>Confirm by selecting the 'Delete' button once more.<br/>This will delete all nodes underneath it!!</p>";
        }
        else if (deleteNodeTypeButton.CommandArgument == "true")
        {
            // This is a confirmed delete...
            // Use a context to loop all referenced records and delete
            int id = (int)ViewState["EditingNodeType"];
            using (MonadModel context = new MonadModel())
            {
                try
                {
                    foreach (Node n in context.NodeTypes.First(nt => nt.NodeTypeID == id).Nodes.ToArray())
                    {
                        // Remove the links
                        foreach (NodeLink nl in n.NodeLinks1.ToArray()) context.NodeLinks.Remove(nl);
                        foreach (NodeLink nl in n.NodeLinks2.ToArray()) context.NodeLinks.Remove(nl);
                        // Remove the node
                        context.Nodes.Remove(n);
                    }
                    // Remove the node type
                    context.NodeTypes.Remove(context.NodeTypes.First(nt => nt.NodeTypeID == id));
                    // And save the changes
                    context.SaveChanges();
                    // Go back to the main node listing
                    errorText.Text = "<p class=\"confirm\">Node Type Deleted</p>";
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
                }
            }
        }
    }

    #endregion

    #region Editing Individual Nodes

    /// <summary>
    /// For when the user wants to list out the nodes for editing
    /// </summary>
    protected void editNodesLink_Click(object sender, EventArgs e)
    {
        // Clear all previous error text
        errorText.Text = null;
        // Switch screens
        landingScreen.Visible = false;
        listNodesScreen.Visible = true;
        // Initialize the top level repeater with the current list of node types
        using (MonadModel context = new MonadModel())
        {
            nodeTypeTopRepeater.DataSource = context.Monads.First(m => m.MonadID == MyMonad.MonadID).
                                      NodeTypes.OrderBy(nt => nt.Sequence).
                                      ToArray();
            nodeTypeTopRepeater.DataBind();
        }
    }

    /// <summary>
    /// For when a top level repeater for node types binds a data item
    /// </summary>
    protected void nodeTypeTopRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        // Check to ensure this is the correct item type
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            NodeType nodeType = (NodeType)e.Item.DataItem;
            ((Literal)e.Item.FindControl("nodeTypeName")).Text = "<span style=\"color:#" + nodeType.Color + ";\">" + nodeType.PluralName + "</span>";
            // Find the subrepeater for the nodes and bind it
            ((Repeater)e.Item.FindControl("nodeList")).DataSource = nodeType.Nodes.OrderBy(n => n.Sequence).ToArray();
            ((Repeater)e.Item.FindControl("nodeList")).DataBind();
        }
    }

    // Have a global member tracking the current node type being bound
    private int workingNodeType = 0;

    /// <summary>
    /// For when our sublist databinds
    /// </summary>
    protected void nodeList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            // Cast the incoming dataitem
            Node node = (Node)e.Item.DataItem;
            // Populate the main link button information for this node
            LinkButton lb = (LinkButton)e.Item.FindControl("nodeEdit");
            lb.ForeColor = System.Drawing.ColorTranslator.FromHtml("#" + node.NodeType.Color);
            lb.Text = node.Title;
            lb.CommandArgument = node.NodeID.ToString();
            // Populate the up and down arrows with the correct information (node ID and type ID)
            ((LinkButton)e.Item.FindControl("nodeUp")).CommandArgument = node.NodeID.ToString() + ":" + node.NodeTypeID.ToString();
            ((LinkButton)e.Item.FindControl("nodeDown")).CommandArgument = node.NodeID.ToString() + ":" + node.NodeTypeID.ToString();
            // Store the node type
            workingNodeType = node.NodeTypeID;
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            // Make reference to the nodetype so we know where to place the new node
            ((LinkButton)e.Item.FindControl("nodeAdd")).CommandArgument = workingNodeType.ToString();
        }
    }

    /// <summary>
    ///  For when a node needs to move up...
    /// </summary>
    protected void nodeUp_Click(object sender, EventArgs e)
    {
        using (MonadModel context = new MonadModel())
        {
            // Pull the full list in decending order
            int nodeID = int.Parse(((LinkButton)sender).CommandArgument.Split(':').First());
            int nodeTypeID = int.Parse(((LinkButton)sender).CommandArgument.Split(':').Last());
            Node[] nodes = context.Nodes.Where(n => n.NodeTypeID == nodeTypeID).OrderBy(n => n.Sequence).ToArray();
            // Loop through and process accordingly and skipping the first record (which can't move up)
            for (int x = (nodes.Length - 1); x > 0; x--)
            {
                if (nodes[x].NodeID == nodeID)
                {
                    nodes[x].Sequence -= 1;
                    nodes[x - 1].Sequence += 1;
                    x = 1;
                }
            }
            // Save the changes
            context.SaveChanges();
        }
        // Now rebind the list
        editNodesLink_Click(sender, e);
    }

    /// <summary>
    /// And when a node needs to move down...
    /// </summary>
    protected void nodeDown_Click(object sender, EventArgs e)
    {
        using (MonadModel context = new MonadModel())
        {
            // Pull the full list in decending order
            int nodeID = int.Parse(((LinkButton)sender).CommandArgument.Split(':').First());
            int nodeTypeID = int.Parse(((LinkButton)sender).CommandArgument.Split(':').Last());
            Node[] nodes = context.Nodes.Where(n => n.NodeTypeID == nodeTypeID).OrderBy(n => n.Sequence).ToArray();
            // Loop through and process accordingly and skipping the first record (which can't move up)
            for (int x = 0; x < (nodes.Length - 1); x++)
            {
                if (nodes[x].NodeID == nodeID)
                {
                    nodes[x].Sequence += 1;
                    nodes[x + 1].Sequence -= 1;
                    x = nodes.Length;
                }
            }
            // Save the changes
            context.SaveChanges();
        }
        // Now rebind the list
        editNodesLink_Click(sender, e);
    }

    /// <summary>
    /// For when we are editing or adding a node
    /// </summary>
    protected void nodeEdit_Click(object sender, EventArgs e)
    {
        // Clear any previous error
        errorText.Text = null;
        // Switch screens
        listNodesScreen.Visible = false;
        editNodeScreen.Visible = true;
        // Populate with the information from what's in the database
        using (MonadModel context = new MonadModel())
        {
            // Is this a new node addition?
            if (((LinkButton)sender).ID == "nodeAdd")
            {
                deleteNodeButton.Enabled = false;
                // Populate for a new record
                nodeTitle.Text = null;
                nodeText.Text = null;
                nodeURL.Text = null;
                // And store the blank ID of this node in the viewstate
                ViewState["EditingNode"] = 0;
                // And store the node type reference for later in our hidden thingydohicky
                newNodeTypeReference.Value = ((LinkButton)sender).CommandArgument;
            }
            else
            {
                deleteNodeButton.Enabled = true;
                deleteNodeButton.CommandArgument = "false";
                // This is editing an existing node type
                int id = int.Parse(((LinkButton)sender).CommandArgument);
                Node n = context.Nodes.First(x => x.NodeID == id);
                nodeTitle.Text = n.Title;
                nodeText.Text = n.Text;
                nodeURL.Text = n.URL;
                // And store the ID of this nodetype in the viewstate
                ViewState["EditingNode"] = n.NodeID;
            }
        }
    }

    /// <summary>
    /// For when we want to delete a node
    /// </summary>
    protected void deleteNodeButton_Click(object sender, EventArgs e)
    {
        // Clear previous errors
        errorText.Text = null;
        if (deleteNodeButton.CommandArgument == "false")
        {
            // Update the error text for a confirmation and set the delete flag
            // to process the next time the user clicks
            deleteNodeButton.CommandArgument = "true";
            errorText.Text = "<p class=\"error\">Are you sure you want to delete this node?<br/>Confirm by selecting the 'Delete' button once more.</p>";
        }
        else if (deleteNodeButton.CommandArgument == "true")
        {
            // This is a confirmed delete...
            // Use a context to loop all referenced records and delete
            int id = (int)ViewState["EditingNode"];
            using (MonadModel context = new MonadModel())
            {
                try
                {
                    // Remove the links
                    Node n = context.Nodes.First(x => x.NodeID == id);
                    foreach (NodeLink nl in n.NodeLinks1.ToArray()) context.NodeLinks.Remove(nl);
                    foreach (NodeLink nl in n.NodeLinks2.ToArray()) context.NodeLinks.Remove(nl);
                    // Remove the node
                    context.Nodes.Remove(n);
                    // And save the changes
                    context.SaveChanges();
                    // Go back to the main node listing
                    errorText.Text = "<p class=\"confirm\">Node Deleted</p>";
                    listNodesScreen.Visible = true;
                    editNodeScreen.Visible = false;
                    // Rebind the list
                    nodeTypeTopRepeater.DataSource = context.Monads.First(m => m.MonadID == MyMonad.MonadID).
                                      NodeTypes.OrderBy(nt => nt.Sequence).
                                      ToArray();
                    nodeTypeTopRepeater.DataBind();
                }
                catch (Exception ex)
                {
                    // Get to the last inner exception and display that
                    while (ex.InnerException != null) ex = ex.InnerException;
                    errorText.Text = "<p class=\"error\">An error occured: " + ex.Message + "</p>";
                }
            }
        }
    }

    /// <summary>
    /// For When the user wants to select our upload a local file
    /// </summary>
    protected void nodeURLSelect_Click(object sender, EventArgs e)
    {
        // Clear any previous error
        errorText.Text = null;
        // Switch screens
        fileSelectScreen.Visible = true;
        editNodeScreen.Visible = false;
        // And bind the current list from the working monad directory
        if (System.IO.Directory.Exists(Server.MapPath("~/library/" + MyMonad.MonadID)))
        {
            fileList.DataSource = System.IO.Directory.EnumerateFiles(Server.MapPath("~/library/" + MyMonad.MonadID));
            fileList.DataBind();
        }
        else
        {
            // Trigger the no records message
            fileList.DataSource = new Array[] { };
            fileList.DataBind();
        }

    }

    /// <summary>
    /// A user want to upload a file
    /// </summary>
    protected void fileUploadSubmit_Click(object sender, EventArgs e)
    {
        // Clear any previous error
        errorText.Text = string.Empty;
        // Is there are file?
        if (!fileUploader.HasFiles)
        {
            errorText.Text = "<p class=\"error\">No file(s) were selected for upload.</p>";
        }
        else
        {
            // Specify the path to save the uploaded file(s) to.
            string savePath = Server.MapPath("~/library/" + MyMonad.MonadID + "/");
            // Create this directory if necessary
            if (!System.IO.Directory.Exists(savePath)) System.IO.Directory.CreateDirectory(savePath);
            // The follow code was adapted from 
            // https://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.fileupload.hasfile(v=vs.110).aspx
            foreach (HttpPostedFile file in fileUploader.PostedFiles)
            {
                // Get the name of the file to upload.
                string fileName = file.FileName;
                // Create the path and file name to check for duplicates.
                string pathToCheck = savePath + "\\" + fileName;
                // Create a temporary file name to use for checking duplicates.
                string tempfileName = "";
                // Check to see if a file already exists with the
                // same name as the file to upload.        
                if (System.IO.File.Exists(pathToCheck))
                {
                    int counter = 2;
                    while (System.IO.File.Exists(pathToCheck))
                    {
                        // if a file with this name already exists,
                        // prefix the filename with a number.
                        tempfileName = "(" + counter.ToString() + ")" + fileName;
                        // Update the path to check
                        pathToCheck = savePath + tempfileName;
                        // Increment the counter
                        counter++;
                    }
                    // Update the working filename
                    fileName = tempfileName;
                    // Notify the user that the file name was changed.
                    errorText.Text = "<p class=\"confirm\">Files Uploaded.<br/>Please be aware some were numerically prefixed due to pre-existing files with the same name.</p>";
                }
                else
                {
                    // Notify the user that the file was saved successfully.
                    errorText.Text = "<p class=\"confirm\">Files Uploaded Successfully.</p>";
                }
                // Call the SaveAs method to save the uploaded
                // file to the specified directory.
                file.SaveAs(savePath + fileName);
            }
            // Rebind the grid!
            if (System.IO.Directory.Exists(Server.MapPath("~/library/" + MyMonad.MonadID)))
            {
                fileList.DataSource = System.IO.Directory.EnumerateFiles(Server.MapPath("~/library/" + MyMonad.MonadID));
                fileList.DataBind();
            }
            else
            {
                // Trigger the no records message
                fileList.DataSource = new Array[] { };
                fileList.DataBind();
            }
        }
    }

    /// <summary>
    /// For when the file listing populates on local monad files
    /// </summary>
    protected void fileList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Grab the incoming file name
            string file = (string)e.Row.DataItem;
            // Set the preview
            ((HyperLink)e.Row.FindControl("filePreview")).NavigateUrl = "/library/" + MyMonad.MonadID + "/" + file.Split('\\').Last();
            ((Literal)e.Row.FindControl("filePartialName")).Text = file.Split('\\').Last();
        }
    }

    /// <summary>
    /// For when the right file is found and selected
    /// </summary>
    protected void selectFile_Click(object sender, EventArgs e)
    {
        // Clear and errors
        errorText.Text = null;
        // Snag the URL from the preview link
        nodeURL.Text = ((HyperLink)((Button)sender).NamingContainer.FindControl("filePreview")).NavigateUrl;
        // Change screens
        editNodeScreen.Visible = true;
        fileSelectScreen.Visible = false;
        // Notify the user that the file was selected.
        errorText.Text = "<p class=\"confirm\">Local File Selected for Node Link.</p>";
    }

    /// <summary>
    /// For when a file is marked to be deleted from the directory
    /// </summary>
    protected void deleteFile_Click(object sender, EventArgs e)
    {
        // Clear previous errors
        errorText.Text = null;
        if (((Button)sender).CommandArgument == "false")
        {
            // Update the error text for a confirmation and set the delete flag
            // to process the next time the user clicks it
            ((Button)sender).CommandArgument = "true";
            errorText.Text = "<p class=\"error\">Are you sure you want to delete this file?<br/>Confirm by selecting the 'Delete' button for this file once more.</p>";
        }
        else if (((Button)sender).CommandArgument == "true")
        {
            // This is a confirmed delete...
            // Use the preview and mappath to delete it
            string fileToDelete = Server.MapPath(((HyperLink)((Button)sender).NamingContainer.FindControl("filePreview")).NavigateUrl);
            if (System.IO.File.Exists(fileToDelete)) System.IO.File.Delete(fileToDelete);
            // Notify the user that the file was delete successfully.
            errorText.Text = "<p class=\"confirm\">" + fileToDelete.Split('\\').Last() + " Deleted Successfully.</p>";
            // Rebind the grid!
            if (System.IO.Directory.Exists(Server.MapPath("~/library/" + MyMonad.MonadID)))
            {
                fileList.DataSource = System.IO.Directory.EnumerateFiles(Server.MapPath("~/library/" + MyMonad.MonadID));
                fileList.DataBind();
            }
            else
            {
                // Trigger the no records message
                fileList.DataSource = new Array[] { };
                fileList.DataBind();
            }
        }
    }

    #endregion

    protected void nodeRelationsEdit_Click(object sender, EventArgs e)
    {

    }
}
