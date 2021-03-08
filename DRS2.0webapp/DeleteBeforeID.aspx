<%@ Page Title="Delete all before ID ..." Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="DeleteBeforeID.aspx.vb" Inherits="DRS2._0webapp.DeleteBeforeID" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        Delete all before and including ID:&nbsp;&nbsp;
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;<asp:Button ID="Button1" runat="server" BackColor="Red" Font-Bold="True" Text="DO IT NOW" ToolTip="EXTREME CAUTION !" />
    </p>
    <p>
        &nbsp;</p>
    <p>
        RESULT:
        <asp:Label ID="Label1" runat="server" Text="... "></asp:Label>
    </p>
</asp:Content>
