Partial Public Class RecordLog
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' load on start 
        Button1_Click(sender, e)
    End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        TextBox1.Text = My.Computer.FileSystem.ReadAllText(getPathtoAppData() & LOGFILENAME)
    End Sub

    Protected Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        My.Computer.FileSystem.WriteAllText(getPathtoAppData() & LOGFILENAME, "", False)
        Button1_Click(sender, e)
    End Sub
End Class