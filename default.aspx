﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="DefaultHomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Monadic Exploration</title>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">

    <script src="app/config.js" type="text/javascript"></script>
    <link href="app/style.css" type="text/css" rel="stylesheet">

    <script src="src/jquery-2.0.3.min.js"></script>
    <script src="src/prefixfree.min.js"></script>
    <script src="src/jquery.parse.min.js" type="text/javascript"></script>

    <script src="src/mn.js" type="text/javascript"></script>
    <link href="src/style.css" type="text/css" rel="stylesheet">

    <script type="text/javascript">
    var mn;
    function init() {
	    mn = new MonadicNomad();	
	    mn.load();
    }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <nav id="info">
	    <h1></h1>
	    <ul></ul>
    </nav>

    <div class="overlay" id="hint">
	    <input type="search" results name="search" value="" id="search" placeholder='search'>		
    </div>	

    <div id="canvas"></div>
    </form>
</body>
</html>
