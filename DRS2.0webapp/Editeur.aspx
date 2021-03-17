<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="DRS2._0webapp.Editeur" title="Editeur" Codebehind="Editeur.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h3>
    Editeur moi!</h3>
    <br />

    <div>
        <h4>
            NOTE: cannot edit EndTime (calced!)
        Always rewrite schedule file after updates! do it: <a href="DisplayOE1.aspx">here</a></h4>
        <a href="DisplayOE1.aspx"></a>
        <br />
        <br />
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            DataSourceID="AccessDataSource2" AllowPaging="True" AllowSorting="True" CellPadding="4" ForeColor="#333333" GridLines="None" Width="80%" EnableModelValidation="True" CaptionAlign="Left">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True"
                    SortExpression="Id" />
                <asp:BoundField DataField="RecordingLegth" HeaderText="Length(s)" SortExpression="RecordingLegth" ReadOnly="true" />
                <asp:BoundField DataField="RecordingTime" HeaderText="RecordingTime" SortExpression="RecordingTime" ReadOnly="false"  />
                <asp:BoundField DataField="StatusEncodeEnd" HeaderText="End" SortExpression="StatusEncodeEnd" DataFormatString="{0:HH\:mm}"  ReadOnly="false"   />
                <asp:BoundField DataField="MP3OutFileName" HeaderText="MP3OutFileName" SortExpression="MP3OutFileName" />
            </Columns>
            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:AccessDataSource ID="AccessDataSource2" runat="server"
            DataFile="~/App_Data/DRS2.0.mdb" DeleteCommand="DELETE FROM [DRS] WHERE [Id] = ?"
            InsertCommand="INSERT INTO [DRS] ([Id], [RecordingTime], [RecordingLegth], [StatusEncodeEnd], [MP3OutFileName]) VALUES (?, ?, ?, ?, ?)"
            OldValuesParameterFormatString="original_{0}" 
            SelectCommand="SELECT [Id], [RecordingTime], [RecordingLegth], [StatusEncodeEnd], [MP3OutFileName] FROM [DRS]
 where [RecordingTime] &gt; now() order by [RecordingTime]"
            UpdateCommand="UPDATE [DRS] SET [RecordingTime] = ?, [RecordingLegth] = ?, [StatusEncodeEnd] = ?, [MP3OutFileName] = ? WHERE [Id] = ?">
            <DeleteParameters>
                <asp:Parameter Name="original_Id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="Id" Type="Int32" />
                <asp:Parameter Name="RecordingTime" Type="DateTime" />
                <asp:Parameter Name="RecordingLegth" Type="Int32" />
                <asp:Parameter Name="StatusEncodeEnd" Type="DateTime" />
                <asp:Parameter Name="MP3OutFileName" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="RecordingTime" Type="DateTime" />
                <asp:Parameter Name="RecordingLegth" Type="Int32" />
                <asp:Parameter Name="StatusEncodeEnd" Type="DateTime" />
                <asp:Parameter Name="MP3OutFileName" Type="String" />
                <asp:Parameter Name="original_Id" Type="Int32" />
            </UpdateParameters>
        </asp:AccessDataSource>
    
    </div>
  

</asp:Content>

