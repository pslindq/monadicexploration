using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoginPage : System.Web.UI.Page
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Set the page title to keep postbacks consistent
        this.Title = headingText.Text;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // If we aren't in postback...
        if (!this.IsPostBack)
        {
            // This page is preset to be a top-level creation of a new monad
            // so if that's the case we can exit out
            if (Request[ConfigurationManager.AppSettings["ForceMasterQueryStringParam"]]!=null&&Request[ConfigurationManager.AppSettings["ForceMasterQueryStringParam"]].ToLower() == bool.TrueString.ToLower())
            {
                // This is a top level create request. Do we already have the password?
                if ((string)Session[ConfigurationManager.AppSettings["SessionPasswordKey"]] == ConfigurationManager.AppSettings["MasterPassword"])
                {
                    // Yep!  transfer this request...
                    Server.Transfer("/create.aspx", false);
                }
                else
                {
                    // Update the action to point to the correct version
                    this.form1.Action = "/admin";
                }
            }
            else if (Request[ConfigurationManager.AppSettings["MonadQueryStringParam"]] != null)
            {
                // This is a specific monad edit request. Do we have the master password?
                if ((string)Session[ConfigurationManager.AppSettings["SessionPasswordKey"]] == ConfigurationManager.AppSettings["MasterPassword"])
                {
                    // Yep! Top-level master so transfer this request...
                    Server.Transfer("/edit.aspx", false);
                }
                else
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
                        else if (Session[ConfigurationManager.AppSettings["SessionPasswordKey"]]!=null&&(string)Session[ConfigurationManager.AppSettings["SessionPasswordKey"]]==monad.AdminPWD)
                        {
                            // Yep! Monad level admin pwd so transfer this request...
                            Server.Transfer("/edit.aspx", false);
                        }
                        else
                        {
                            // Update the action to point to the correct version
                            this.form1.Action = "/" + Request[ConfigurationManager.AppSettings["MonadQueryStringParam"]] + "/admin";
                            // And update the text for the user
                            directionsText.Text = "Please enter the monad or master password to edit the underlying data.";
                            headingText.Text = "Monadic Explorations - " + monad.Title + " Editing Logon";
                        }
                    }
                }
            }
            else
            {
                // there is no directive being passed.  Redirect to main homepage
                Response.Redirect("default.aspx");
            }
        }
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        // We need to go back through and vet the password.
        // Reset any error text
        errorText.Text = null;
        if (Request[ConfigurationManager.AppSettings["ForceMasterQueryStringParam"]] != null && Request[ConfigurationManager.AppSettings["ForceMasterQueryStringParam"]].ToLower() == bool.TrueString.ToLower())
        {
            // This is a top level create request. Was the master password provided?
            if (password.Text == ConfigurationManager.AppSettings["MasterPassword"])
            {
                // Yep!  Save this password to the session and transfer this request...
                Session[ConfigurationManager.AppSettings["SessionPasswordKey"]] = password.Text;
                Server.Transfer("/create.aspx", false);
            }
            else
            {
                // Display the error
                errorText.Text = "<p class=\"error\">The password is incorrect.</p>";
            }
        }
        else
        {
            // This has to be a specific monad edit request. Did they give the master password?
            if (password.Text == ConfigurationManager.AppSettings["MasterPassword"])
            {
                // Yep!  Save this password to the session and transfer this request...
                Session[ConfigurationManager.AppSettings["SessionPasswordKey"]] = password.Text;
                Server.Transfer("/edit.aspx", false);
            }
            else
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
                    else if (password.Text == monad.AdminPWD)
                    {
                        // Yep! Monad level admin pwd so transfer this request...
                        Session[ConfigurationManager.AppSettings["SessionPasswordKey"]] = password.Text;
                        Server.Transfer("/edit.aspx", false);
                    }
                    else
                    {
                        // Display the error
                        errorText.Text = "<p class=\"error\">The password is incorrect.</p>";
                    }
                }
            }
        }
    }
}