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
                <li><asp:LinkButton ID="editNodesLink" OnClick="editNodesLink_Click" runat="server">Edit the Individual Nodes</asp:LinkButton></li>
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
                <asp:Button ID="deleteMonad" CausesValidation="false" OnClick="deleteMonad_Click" runat="server" Text="Delete this Monad" />
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
                            <asp:LinkButton ID="nodeTypeEdit" Font-Bold="true" runat="server" OnClick="nodeTypeEdit_Click">Name [and color] of Node Type</asp:LinkButton> 
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
                <asp:Button ID="deleteNodeTypeButton" CausesValidation="false" OnClick="deleteNodeTypeButton_Click" runat="server" Text="Delete this Node Type" />
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
            <p>Select a node to edit, use the arrows to move up or down in relative display order, or use the [Associations] 
                link at the end of each node title to edit the node's relationships with other nodes in this monad:</p>
            <asp:Repeater ID="nodeTypeTopRepeater" runat="server" OnItemDataBound="nodeTypeTopRepeater_ItemDataBound">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                        <li>
                            <p><strong><asp:Literal runat="server" ID="nodeTypeName" Mode="PassThrough"></asp:Literal></strong></p>
                            <asp:Repeater ID="nodeList" runat="server" OnItemDataBound="nodeList_ItemDataBound">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate> 
                                        <li>
                                            <span class="order"><asp:LinkButton ID="nodeUp" runat="server" OnClick="nodeUp_Click" ToolTip="Move this node up the list">&#8679;</asp:LinkButton>
                                            <asp:LinkButton ID="nodeDown" runat="server" OnClick="nodeDown_Click" ToolTip="Move this node down the list">&#8681;</asp:LinkButton></span>
                                            <asp:LinkButton ID="nodeEdit" Font-Bold="true" runat="server" OnClick="nodeEdit_Click">Name [and color] of Node</asp:LinkButton> 
                                            <asp:LinkButton ID="nodeRelationsEdit" runat="server" OnClick="nodeRelationsEdit_Click">[# Association(s)]</asp:LinkButton>
                                        </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <li><asp:LinkButton ID="nodeAdd" runat="server" OnClick="nodeEdit_Click">[Add a New Node of this Type]</asp:LinkButton></li>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <p>
                <asp:Button ID="cancelNodeListButton" CausesValidation="false" runat="server" OnClick="cancelButton_Click" Text="Return to the Main Editing Options" />
            </p>
        </asp:Panel>
        <asp:Panel ID="editNodeScreen" runat="server" Visible="false" CssClass="editscrn">
            <p>Please use the following elements to define this node:</p>
            <p>
                <strong><asp:Label ID="nodeTitleLabel" AssociatedControlID="nodeTitle" runat="server" Text="Node Title:"></asp:Label></strong>
                <asp:TextBox ID="nodeTitle" runat="server" Width="300px"></asp:TextBox> 
                <asp:RequiredFieldValidator Display="Dynamic" ID="nodeTitleValidator" runat="server" ControlToValidate="nodeTitle" SetFocusOnError="true"><span class="required">* Required</span></asp:RequiredFieldValidator>
            </p>
            <p>
                <strong><asp:Label ID="nodeTextLabel" AssociatedControlID="nodeText" runat="server" Text="Node Text/Content:"></asp:Label></strong>
                <asp:RequiredFieldValidator Display="Dynamic" ID="nodeTextValidation" runat="server" ControlToValidate="nodeText" SetFocusOnError="true"><span class="required">* Required</span></asp:RequiredFieldValidator><br />
                <asp:TextBox ID="nodeText" runat="server" TextMode="MultiLine" Width="400px" Height="300px"></asp:TextBox> <br />
                <em>Please Note: Too much text will be truncated in the final display. <br />
                Remember to sample view your visualization content to avoid confusing text cutoffs. </em>
            </p>
            <p>
                <strong><asp:Label ID="nodeURLLabel" AssociatedControlID="nodeURL" runat="server" Text="Node Hyperlink:"></asp:Label></strong>
                <asp:TextBox ID="nodeURL" runat="server"></asp:TextBox>  
                <asp:Button CausesValidation="false" OnClick="nodeURLSelect_Click" ID="nodeURLSelect" runat="server" Text="Select/Upload Local File..." /><br />
                <em>This field is optional. Leave it blank, select a local file, or paste/type<br /> in the Internet link you which the node title to point to.</em>
             </p>
            <p>
                <asp:Button ID="saveNodeButton" CausesValidation="true" runat="server" OnClick="saveNodeButton_Click" Text="Save the Updated Information" />
                <asp:HiddenField ID="newNodeTypeReference" runat="server" />
                or
                <asp:Button ID="deleteNodeButton" OnClick="deleteNodeButton_Click" CausesValidation="false" runat="server" Text="Delete this Node" />
                or
                <asp:Button ID="cancelNodeButton" OnClick="cancelButton_Click" CausesValidation="false" runat="server" Text="Cancel Editing" />
            </p>
        </asp:Panel>
        <asp:Panel ID="fileSelectScreen" runat="server" Visible="false" CssClass="editscrn">
            <p>The following is a complete list of the files currently uploaded to be associated with this monad visualization.<br />
                Use the buttons to preview the file in a new tab/window or to select it to be associated with the working node.
            </p>
            <div class="editscrn">
            <asp:GridView ID="fileList" runat="server" CssClass="filelist" AutoGenerateColumns="false" CellPadding="2" OnRowDataBound="fileList_RowDataBound">
                <Columns>
                    <asp:TemplateField ShowHeader="true" HeaderText="File Name" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <asp:HyperLink ID="filePreview" runat="server" Target="_blank">[Preview]</asp:HyperLink>
                            <asp:Literal ID="filePartialName" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="true" HeaderText="Your Options" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <asp:Button ID="selectFile" OnClick="selectFile_Click" runat="server" Text="Select this File" />
                            <asp:Button ID="deleteFile" OnClick="deleteFile_Click" runat="server" CommandArgument="false" Text="Delete this File" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <p style="text-align:center;"><strong>No files currently are in this monad's local file library.</strong></p>
                </EmptyDataTemplate>
            </asp:GridView>
            </div>
            <p>Alternatively, you can upload a new file(s).<br />
                After uploading, this screen will refresh with the newly posted files for your selection.<br />
                <em>Total size of all files together must be under 100MB.</em>
            </p>
            <p>
                <strong>Step 1)</strong> <asp:FileUpload ID="fileUploader" runat="server" Font-Italic="true" AllowMultiple="true" /> <br /><br />
                <strong>Step 2)</strong> <asp:Button ID="fileUploadSubmit" OnClick="fileUploadSubmit_Click" runat="server" Text="Upload Selected File" />
            </p>
            <p>- or - </p>
            <p>
                <asp:Button ID="cancelFileSelectButton" OnClick="cancelButton_Click" CausesValidation="false" runat="server" Text="Cancel Local File Selection" />
            </p>
        </asp:Panel>
        <asp:Panel ID="linkSelectScreen" runat="server" Visible="false" CssClass="editscrn">
            <p>Select the nodes to you wish to relate the node entitled 
                <strong><asp:Literal ID="associateToNodeName" runat="server"></asp:Literal></strong>
                to using the checkboxes below.  Items already checked 
                indicate a pre-existing association.
            </p>
            <asp:Repeater ID="nodeTypeLinkingRepeater" runat="server" OnItemDataBound="nodeTypeLinkingRepeater_ItemDataBound">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                        <li>
                            <p><strong><asp:Literal runat="server" ID="nodeTypeName" Mode="PassThrough"></asp:Literal></strong></p>
                            <asp:Repeater ID="nodeListLinking" runat="server" OnItemDataBound="nodeListLinking_ItemDataBound">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate> 
                                        <li>
                                            <asp:CheckBox ID="linkNodeFlag" TextAlign="Right" Text="Node Name" ForeColor="Window" runat="server" />
                                            <asp:HiddenField ID="linkID" runat="server" />
                                            <asp:HiddenField ID="nodeID" runat="server" />
                                        </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <p>
                <asp:Button ID="saveNodeLinkingButton" CausesValidation="true" OnClick="saveNodeLinkingButton_Click" runat="server" Text="Save the Linking Information" />
                or
                <asp:Button ID="cancelNodeLinkingButton" OnClick="cancelButton_Click" CausesValidation="false" runat="server" Text="Cancel Linking" />
            </p>
        </asp:Panel>
        <p>or <asp:Button ID="cancelButton" CausesValidation="false" runat="server" OnClick="cancelAllButton_Click" Text="Cancel" /> all editing and return to the visualization.</p>
    </form>
</body>
</html>
