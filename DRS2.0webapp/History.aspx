<%@ Page Title="History Browser" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="History.aspx.vb" Inherits="DRS2._0webapp.History  " %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
       <h3> History!</h3></p>
    <p>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Id" DataSourceID="AccessDataSource1" EnableModelValidation="True" ForeColor="#333333" GridLines="None" PageSize="30">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
                <asp:BoundField DataField="RecordingTime" HeaderText="RecordingTime" SortExpression="RecordingTime" />
                <asp:BoundField DataField="MP3OutFileName" HeaderText="MP3OutFileName" SortExpression="MP3OutFileName" />
                <asp:CheckBoxField DataField="StatusDone" HeaderText="StatusDone" SortExpression="StatusDone" />
            </Columns>
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
        <asp:AccessDataSource ID="AccessDataSource1" runat="server" DataFile="~/App_Data/DRS2.0.mdb" DeleteCommand="DELETE FROM [DRS] WHERE [Id] = ?" InsertCommand="INSERT INTO [DRS] ([Id], [RecordingTime], [MP3OutFileName], [StatusDone]) VALUES (?, ?, ?, ?)" SelectCommand="SELECT [Id], [RecordingTime], [MP3OutFileName], [StatusDone] FROM [DRS]  where [RecordingTime] < now() order by [RecordingTime] DESC" UpdateCommand="UPDATE [DRS] SET [RecordingTime] = ?, [MP3OutFileName] = ?, [StatusDone] = ? WHERE [Id] = ?">
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="Id" Type="Int32" />
                <asp:Parameter Name="RecordingTime" Type="DateTime" />
                <asp:Parameter Name="MP3OutFileName" Type="String" />
                <asp:Parameter Name="StatusDone" Type="Boolean" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="RecordingTime" Type="DateTime" />
                <asp:Parameter Name="MP3OutFileName" Type="String" />
                <asp:Parameter Name="StatusDone" Type="Boolean" />
                <asp:Parameter Name="Id" Type="Int32" />
            </UpdateParameters>
        </asp:AccessDataSource>
    </p>
    <p>
        &nbsp;</p>
</asp:Content>
