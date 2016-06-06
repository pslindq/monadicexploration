<%@ Application Language="C#" %>

<script runat="server">

    void Application_BeginRequest(Object sender, EventArgs e)
    {
        // Pull the original URL request
        string originalURL = HttpContext.Current.Request.Path.ToLower();
        // Rewrite URL only if not referencing a physical file
        if (!originalURL.StartsWith("/app/")&&!originalURL.StartsWith("/src/"))
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
                // This is a standard visualization - rewrite to the display logic
                Context.RewritePath("/showmonad.aspx?" + ConfigurationManager.AppSettings["MonadQueryStringParam"] + "=" + originalURL.Replace("/",""));
            }
        }
    }

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }

</script>
