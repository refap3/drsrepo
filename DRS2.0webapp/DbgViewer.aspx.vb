Imports System.IO

Partial Public Class DbgViewer
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' load on entry
        Button2_Click(sender, e)
    End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'TextBox1.Text = My.Computer.FileSystem.ReadAllText(getDBGVIEWERFILENAME())

        TextBox1.Text = LoadDebugFile(0)
    End Sub
    Function LoadDebugFile(Optional maxRows As Integer = 200) As String
        Using fs As FileStream = New FileStream(getDBGVIEWERFILENAME(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Using sr As StreamReader = New StreamReader(fs, Encoding.Default)
                Dim allTheLines = sr.ReadToEnd()
                Dim resultLines = ""
                If (maxRows > 0) Then
                    Dim someLines = allTheLines.Split(Environment.NewLine)
                    Dim iFrom = 0
                    iFrom = Math.Max(someLines.Length - maxRows, 0)
                    For i As Integer = iFrom To someLines.Length - 1
                        resultLines &= someLines(i)
                    Next
                Else
                    resultLines = allTheLines
                End If
                Return resultLines
            End Using
        End Using
    End Function

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.Text = LoadDebugFile(300)
    End Sub
End Class