
Partial Class scheduled
    Inherits System.Web.UI.Page

    Protected Sub Load_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        TextBox1.Text = ReadFromScheduleFile(True)
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim ws As New WSfileacc.WSFileAccess
        writeToSchedFile(TextBox1.Text, True)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Load_Click(sender, e)
    End Sub
End Class
