Imports System.IO

Partial Public Class DbgViewer
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' load on entry
        Button1_Click(sender, e)
    End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'TextBox1.Text = My.Computer.FileSystem.ReadAllText(getDBGVIEWERFILENAME())
        Dim fs As FileStream = New FileStream(getDBGVIEWERFILENAME(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Dim sr As StreamReader = New StreamReader(fs, Encoding.Default)
        TextBox1.Text = sr.ReadToEnd()
    End Sub

End Class