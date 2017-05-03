
Partial Class Filter
    Inherits System.Web.UI.Page

    Protected Sub Load_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            TextBox1.Text = My.Computer.FileSystem.ReadAllText(Server.MapPath("app_data/" & EXCLUDEFILENAME))
        Catch ex As Exception
            TextBox1.Text = ex.ToString
        End Try
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        My.Computer.FileSystem.WriteAllText(Server.MapPath("app_data/" & EXCLUDEFILENAME), TextBox1.Text, False, New System.Text.ASCIIEncoding)
    End Sub


End Class
