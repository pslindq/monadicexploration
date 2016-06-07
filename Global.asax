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
                // TODO: send to login to create new monad (top level)
            }
            else if (originalURL.EndsWith("/admin"))
            {
                // TODO: send to login to edit existing monad (top level)
            }
            else
            {
                // This is a standard visualization - rewrite to the display handler
                Context.RewritePath("/showmonad.aspx?" + ConfigurationManager.AppSettings["MonadQueryStringParam"] + "=" + originalURL.Replace("/",""));
            }
        }
    }

</script>
