<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="EditMonadPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Monadic Explorations - Editing a Monad</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/src/style.css" rel="stylesheet" />
    <link href="/app/style.css" rel="stylesheet" />
    <style>
        body{overflow:scroll;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1><asp:Literal ID="headingText" runat="server">Monadic Explorations - Editing: </asp:Literal></h1>
        <asp:Literal ID="errorText" runat="server" Mode="PassThrough"></asp:Literal>
        <asp:Panel ID="landingScreen" runat="server" Visible="true" CssClass="editscrn" >
            <p>What would you like to do?</p>
            <ul>
                <li><asp:LinkButton ID="editBasicLink" OnClick="editBasicLink_Click" runat="server">Edit Basic Monad Information</asp:LinkButton></li>
                <li><asp:LinkButton ID="editNodeTypesLink" OnClick="editNodeTypesLink_Click" runat="server">Edit the Node Types</asp:LinkButton></li>
                <li><asp:LinkButton ID="editNodesLink" runat="server">Edit the Individual Nodes</asp:LinkButton></li>
            </ul>
        </asp:Panel>
        <asp:Panel ID="editBasicScreen" runat="server" Visible="false" CssClass="editscrn">
            <p>Please edit the following basic information about this monad:</p>
            <p>
                <strong><asp:Label ID="monadTitleLabel" AssociatedControlID="monadTitle" runat="server" Text="Monad Title:"></asp:Label></strong>
                <asp:TextBox ID="monadTitle" runat="server"></asp:TextBox> 
                <asp:RequiredFieldValidator Display="Dynamic" ID="titleValidator" runat="server" ControlToValidate="monadTitle" SetFocusOnError="true"><span class="required">* Required</span></asp:RequiredFieldValidator>
            </p>
            <p>
                <strong><asp:Label ID="monadLinkToLabel" AssociatedControlID="monadLinkTo" runat="server" Text="Link Title To:"></asp:Label></strong>
                <asp:TextBox ID="monadLinkTo" runat="server"></asp:TextBox> 
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
                <br /><em>Please note: you will be required to log in again if you change this.</em>
             </p>
            <p>
                <asp:CheckBox ID="showNodeTypesFlag" runat="server" Text="Show Node Types:" TextAlign="Left" Font-Bold="true" Checked="true" />
            </p>
            <p>
                <asp:Button ID="saveBasicInfoButton" CausesValidation="true" OnClick="saveBasicInfoButton_Click" runat="server" Text="Save the Updated Information" />
                or
                <asp:Button ID="cancelBasicInfoButton" CausesValidation="false" runat="server" OnClick="cancelButton_Click" Text="Cancel Editing" />
            </p>
        </asp:Panel>
        <asp:Panel ID="listNodeTypesScreen" runat="server" Visible="false" CssClass="editscrn">
            <p>Select a node type to edit, or use the arrows to move up or down in relative display order:</p>
            <asp:Repeater ID="nodeTypeList" runat="server" OnItemDataBound="nodeTypeList_ItemDataBound">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                        <li>
                            <span class="order"><asp:LinkButton ID="nodeTypeUp" runat="server" OnClick="nodeTypeUp_Click" ToolTip="Move this type up the list">&#8679;</asp:LinkButton>
                            <asp:LinkButton ID="nodeTypeDown" runat="server" OnClick="nodeTypeDown_Click" ToolTip="Move this type down the list">&#8681;</asp:LinkButton></span>
                            <asp:LinkButton ID="nodeTypeEdit" runat="server" OnClick="nodeTypeEdit_Click">Name [and color] of Node Type</asp:LinkButton> 
                        </li>
                </ItemTemplate>
                <FooterTemplate>
                        <li><asp:LinkButton ID="nodeTypeAdd" runat="server" OnClick="nodeTypeEdit_Click">[Add a New Node Type]</asp:LinkButton></li>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <p>
                <asp:Button ID="cancelNodeTypeListButton" CausesValidation="false" runat="server" OnClick="cancelButton_Click" Text="Return to the Main Editing Options" />
            </p>
        </asp:Panel>
        <asp:Panel ID="editNodeTypeScreen" runat="server" Visible="false" CssClass="editscrn">
            <p>Please use the following elements to define this node type:</p>
            <p>
                <strong><asp:Label ID="nodeTypeNameLabel" AssociatedControlID="nodeTypeName" runat="server" Text="Node Type Name:"></asp:Label></strong>
                <asp:TextBox ID="nodeTypeName" runat="server"></asp:TextBox> 
                <asp:RequiredFieldValidator Display="Dynamic" ID="nodeTypeNameValidator" runat="server" ControlToValidate="nodeTypeName" SetFocusOnError="true"><span class="required">* Required</span></asp:RequiredFieldValidator>
            </p>
            <p>
                <strong><asp:Label ID="nodeTypePluralNameLabel" AssociatedControlID="nodeTypePluralName" runat="server" Text="Plural Name:"></asp:Label></strong>
                <asp:TextBox ID="nodeTypePluralName" runat="server"></asp:TextBox> 
                <asp:RequiredFieldValidator Display="Dynamic" ID="nodeTypePluralNameValidator" runat="server" ControlToValidate="nodeTypePluralName" SetFocusOnError="true"><span class="required">* Required</span></asp:RequiredFieldValidator>
            </p><p>
                <strong><asp:Label ID="slugNameLabel" AssociatedControlID="slugName" runat="server" Text="Slug (Icon) Name:"></asp:Label></strong>
                <asp:TextBox ID="slugName" runat="server" ReadOnly="true"></asp:TextBox>  
                <asp:Button CausesValidation="false" ID="slugSelectionButton" runat="server" OnClick="slugSelectionButton_Click" Text="Select Image..." />
                <asp:RequiredFieldValidator Display="Dynamic" ID="slugNameValidator" runat="server" ControlToValidate="slugName" SetFocusOnError="true"><span class="required">* Required</span></asp:RequiredFieldValidator><br />
                <span id="slugSample" runat="server"></span>
            </p>
            <p>
                <strong><asp:Label ID="nodeTypeColorLabel" AssociatedControlID="nodeTypeColor" runat="server" Text="Color:"></asp:Label></strong>
                <input type="color" name="nodeTypeColor" id="nodeTypeColor" runat="server" value="#000000" /><br />
                <em>Please Note: The HTML5 color selection input sadly does not work in Safari or IE. <br />
                    Please use <a href="http://www.color-hex.com/">hexcode</a> for your color indications if it isn't available.</em>
            </p>
            <p>
                <asp:Button ID="saveNodeTypeButton" CausesValidation="true" OnClick="saveNodeTypeButton_Click" runat="server" Text="Save the Updated Information" />
                or
                <asp:Button ID="cancelNodeTypeButton" CausesValidation="false" runat="server" OnClick="cancelButton_Click" Text="Cancel Editing" />
            </p>
        </asp:Panel>
        <asp:Panel ID="slugSelectionScreen" runat="server" Visible="false" CssClass="editscrn">
            <p>Please select the image you would like to use for the node type slug:</p>
            <asp:Repeater ID="slugList" runat="server" OnItemDataBound="slugList_ItemDataBound">
                <HeaderTemplate><p></HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="slugSelectButton" runat="server" OnClick="slugSelectButton_Click"></asp:LinkButton>
                </ItemTemplate>
                <FooterTemplate></p><div style="clear:left;"></div></FooterTemplate>
            </asp:Repeater>
            <p>
                <asp:Button ID="cancelSlugSelectButton" CausesValidation="false" runat="server" OnClick="cancelButton_Click" Text="Cancel Slug Selection" />
            </p>
        </asp:Panel>
        <asp:Panel ID="listNodesScreen" runat="server" Visible="false" CssClass="editscrn">
        </asp:Panel>
        <asp:Panel ID="editNodeScreen" runat="server" Visible="false" CssClass="editscrn">
        </asp:Panel>
        <asp:Panel ID="fileUploadScreen" runat="server" Visible="false" CssClass="editscrn">
        </asp:Panel>
        <asp:Panel ID="fileSelectScreen" runat="server" Visible="false" CssClass="editscrn">
        </asp:Panel>
        <asp:Panel ID="linkSelectScreen" runat="server" Visible="false" CssClass="editscrn">
        </asp:Panel>
        <p>or <asp:Button ID="cancelButton" CausesValidation="false" runat="server" OnClick="cancelAllButton_Click" Text="Cancel" /> all editing and return to the visualization.</p>
    </form>
</body>
</html>
