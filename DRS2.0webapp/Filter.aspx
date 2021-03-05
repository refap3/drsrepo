<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="DRS2._0webapp.Filter" Title="Filter" Codebehind="Filter.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="Button1" runat="server" Text="Load" AccessKey="L" ToolTip="L(oad)" />&nbsp;
    <asp:Button ID="Button2" runat="server" Text="Save" />
    ONLY WORKS LOCALLY !! NOT VIA WEBSERVICE<br />
    <br />
    <asp:TextBox ID="TextBox1" runat="server" Height="378px" TextMode="MultiLine" Width="80%" Wrap="False"></asp:TextBox>
</asp:Content>

