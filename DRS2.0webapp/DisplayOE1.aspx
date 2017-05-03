<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="DRS2._0webapp.DisplayOE1" Codebehind="DisplayOE1.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#999999" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
        ForeColor="Black" Height="180px" Width="200px" CellPadding="4">
        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
        <SelectorStyle BackColor="#CCCCCC" />
        <OtherMonthDayStyle ForeColor="#808080" />
        <NextPrevStyle VerticalAlign="Bottom" />
        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
        <TitleStyle BackColor="#999999" Font-Bold="True" BorderColor="Black" />
        <WeekendDayStyle BackColor="#FFFFCC" />
    </asp:Calendar>
    <strong>navigating</strong>:
    <asp:Button ID="Button4" runat="server" Text="Today" />&nbsp;
    <asp:Button ID="Button5" runat="server" Text="<<" />&nbsp;
    <asp:Button ID="Button6" runat="server" Text=">>" />
    &nbsp;
    &nbsp; &nbsp;
    <asp:CheckBox ID="CheckBox1" runat="server" Text="Filter on" AutoPostBack="True" Checked="True" />&nbsp;&nbsp;
    &nbsp;<br />
    <strong>recording</strong>: &nbsp;
    <asp:Button ID="Button3" runat="server" Text="Append" />&nbsp;
    <asp:Button ID="Button2" runat="server" Text="writ SCHED" />&nbsp;
    <asp:CheckBox
        ID="cbViaWebService" runat="server" Checked="True" Text="Via WebService" />&nbsp;<br />
    -----------------------------------------------------------------------------------<br />
    <asp:CheckBoxList ID="CheckBoxList1" runat="server">
    </asp:CheckBoxList><br />
    ------------------------------------------------------------------------------------<br />
    <br />
    <asp:Button ID="Button10" runat="server" Text="App and  >>" /><br />
    <strong>Other: &nbsp; &nbsp;<asp:Button
        ID="Button8" runat="server" Text="TESTdata" />
        <asp:Button ID="Button9" runat="server"
            Text="Display WS redir" />
        <asp:LinkButton ID="LinkButton1" runat="server">2 Oe1 Web</asp:LinkButton><br />
        filtering:
    <asp:Button ID="Button1" runat="server" Text="SaveFilter" />
        <asp:Button
        ID="Button7" runat="server" Text="TogglSEL" />
    <asp:Label ID="Label1" runat="server" Text="Status to come here ..." Width="776px" Height="31px"></asp:Label><br />
    </strong>
</asp:Content>

