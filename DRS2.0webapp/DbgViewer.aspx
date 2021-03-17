<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" ValidateRequest="false" CodeBehind="DbgViewer.aspx.vb" Inherits="DRS2._0webapp.DbgViewer" 
    title="DbgViewer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="Button1" runat="server" Text="Load ALL" AccessKey="L" ToolTip="L(oad)" />
    <asp:Button ID="Button2" runat="server" Text="Load some" />
    &nbsp;
    <br />
        <asp:TextBox ID="TextBox1" runat="server" Height="378px" TextMode="MultiLine"
            Width="80%" Wrap="False"></asp:TextBox>
</asp:Content>
