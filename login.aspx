<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Monadic Explorations - Master Logon</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/src/style.css" rel="stylesheet" />
    <link href="/app/style.css" rel="stylesheet" /> 
</head>
<body>
    <form id="form1" runat="server">
        <h1><asp:Literal ID="headingText" runat="server">Monadic Explorations - Master Logon</asp:Literal></h1>
        <p><asp:Literal ID="directionsText" runat="server">
            Please enter the master access password to create a new monad.
        </asp:Literal></p>
        <asp:Literal ID="errorText" runat="server" Mode="PassThrough"></asp:Literal>
        <p>
            <strong><asp:Label ID="passwordLabel" AssociatedControlID="password" runat="server" Text="Password:"></asp:Label></strong>
            <asp:TextBox TextMode="Password" ID="password" runat="server"></asp:TextBox>
        </p>
        <p>
            <asp:Button ID="submitButton" runat="server" OnClick="submitButton_Click" Text="Submit" />
        </p>
    </form>
</body>
</html>
