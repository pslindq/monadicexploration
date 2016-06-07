<%@ Page Language="C#" AutoEventWireup="true" CodeFile="create.aspx.cs" Inherits="CreateMonadPage" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Monadic Explorations - Create Monad</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/src/style.css" rel="stylesheet" />
    <link href="/app/style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <h1><asp:Literal ID="headingText" runat="server">Monadic Explorations - Create Monad</asp:Literal></h1>
        <p>Please provide the following to set up a shell monadic visualization.</p>
        <asp:Literal ID="errorText" runat="server" Mode="PassThrough"></asp:Literal>
        <p>
            <strong><asp:Label ID="titleLabel" AssociatedControlID="title" runat="server" Text="Monad Title:"></asp:Label></strong>
            <asp:TextBox ID="title" runat="server"></asp:TextBox> 
            <asp:RequiredFieldValidator Display="Dynamic" ID="titleValidator" runat="server" ControlToValidate="title" SetFocusOnError="true"><span class="required">* Required</span></asp:RequiredFieldValidator>
        </p>
        <p>
            <strong><asp:Label ID="linkToLabel" AssociatedControlID="linkTo" runat="server" Text="Link Title To:"></asp:Label></strong>
            <asp:TextBox ID="linkTo" runat="server"></asp:TextBox> 
        </p>
        <p>
            <strong><asp:Label ID="urlSegmentLabel" AssociatedControlID="urlSegment" runat="server" Text="URL Segment:"></asp:Label></strong>
            <asp:TextBox ID="urlSegment" runat="server"></asp:TextBox>  
            <asp:RequiredFieldValidator Display="Dynamic" ID="urlSegmentValidator" runat="server" ControlToValidate="urlSegment" SetFocusOnError="true"><span class="required">* Required</span></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator Display="Dynamic" ID="urlSegmentRegExValidator" runat="server" ControlToValidate="urlSegment" SetFocusOnError="true" ValidationExpression="^[a-zA-Z0-9_-]+$"><span class="required">* Alphanumeric, dash, and underscore characters only</span></asp:RegularExpressionValidator>
        </p>
        <p>
            <strong><asp:Label ID="adminPWDLabel" AssociatedControlID="adminPWD" runat="server" Text="Editing Password:"></asp:Label></strong>
            <asp:TextBox ID="adminPWD" runat="server"></asp:TextBox> 
            <asp:RequiredFieldValidator Display="Dynamic" ID="adminPWDValidator" runat="server" ControlToValidate="adminPWD" SetFocusOnError="true"><span class="required">* Required</span></asp:RequiredFieldValidator>
         </p>
        <p>
            <asp:CheckBox ID="showNodeTypesFlag" runat="server" Text="Show Node Types:" TextAlign="Left" Font-Bold="true" Checked="true" />
        </p>
        <p>
            <asp:Button ID="submitButton" CausesValidation="true" OnClick="submitButton_Click" runat="server" Text="Create" />
        </p>
    </form>
</body>
</html>
