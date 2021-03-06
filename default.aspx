﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="DefaultHomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Monadic Explorations - Listing of Available Visualizations</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="src/style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <h1>Monadic Explorations - Listing of Available Visualizations</h1>
        <p>Select from the available visualizations to view, or log in to create a new one:</p>
        <asp:Repeater ID="availableMonads" runat="server" OnItemDataBound="availableMonads_ItemDataBound">
            <HeaderTemplate>
                <ul>
                    <li><a href="/admin">[Create New]</a></li> 
            </HeaderTemplate>
            <ItemTemplate>
                    <li><a runat="server" id="anchor"></a></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
