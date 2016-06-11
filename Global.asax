<%@ Application Language="C#" %>

<script runat="server">

    void Application_BeginRequest(Object sender, EventArgs e)
    {
        // Pull the original URL request - we need to perform some rewriting
        string originalURL = HttpContext.Current.Request.Path.ToLower();
        // Rewrite URL only if not referencing a physical file or the original forked directories (or the root)
        if (!originalURL.StartsWith("/app/")&&!originalURL.StartsWith("/src/")&&!originalURL.Contains(".")&&originalURL!="/")
        {
            // So what are we looking for?
            if (originalURL == "/admin")
            {
                 // Go to the login page and request master password
                 Context.RewritePath("/login.aspx?" + ConfigurationManager.AppSettings["ForceMasterQueryStringParam"] + "=true");
            }
            else if (originalURL.EndsWith("/admin"))
            {
                // Go to the login page and request monad password (or master if they have it)
                 Context.RewritePath("/login.aspx?" + ConfigurationManager.AppSettings["MonadQueryStringParam"] + "=" + originalURL.Replace("/admin","").Replace("/",""));
            }
            else if (originalURL == "/logout") 
            {
                // Go to the logout page
                Context.RewritePath("/logout.aspx?");
            }
            else
            {
                // This is a standard visualization - rewrite to the display handler
                Context.RewritePath("/showmonad.aspx?" + ConfigurationManager.AppSettings["MonadQueryStringParam"] + "=" + originalURL.Replace("/",""));
            }
        }
    }

</script>
