<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="DbgViewer.aspx.vb" Inherits="DRS2._0webapp.DbgViewer" 
    title="DbgViewer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="Button1" runat="server" Text="Load" AccessKey="L" ToolTip="L(oad)" />&nbsp;
    <br />
        <asp:TextBox ID="TextBox1" runat="server" Height="378px" TextMode="MultiLine"
            Width="80%" Wrap="False"></asp:TextBox>
</asp:Content>
