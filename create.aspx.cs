using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CreateMonadPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Test for the password first - make sure an admin is logged in
        if (Session[ConfigurationManager.AppSettings["SessionPasswordKey"]] == null || (string)Session[ConfigurationManager.AppSettings["SessionPasswordKey"]] != ConfigurationManager.AppSettings["MasterPassword"])
        {
            // Redirect to the login as this isn't a valid authentication to be here
            Response.Redirect("/admin", true);
        }
        // Reset our form action to something cleaner
        form1.Action = "/create.aspx";
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        // Reset any error 
        errorText.Text = null;
        // Create the new record
        Monad newMonad = new Monad();
        newMonad.Title = title.Text;
        newMonad.URL = linkTo.Text;
        newMonad.ShowNodeTypes = showNodeTypesFlag.Checked;
        newMonad.URLSegment = urlSegment.Text;
        newMonad.AdminPWD = adminPWD.Text;
        // Pull the context and try to save it
        using (MonadModel context = new MonadModel())
        {
            try
            {
                context.Monads.Add(newMonad);
                context.SaveChanges();
                // Move on to the editing page
                Response.Redirect("/" + newMonad.URLSegment + "/admin");
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

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("/default.aspx");
    }
}