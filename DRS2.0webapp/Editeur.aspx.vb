
Partial Class Editeur
    Inherits System.Web.UI.Page

    Private Sub GridView2_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridView2.RowUpdating
        Dim newEnd As Date = e.NewValues.Item("StatusEncodeEnd")
        Dim newStart As Date = e.NewValues.Item("RecordingTime")
        Dim newLength As Integer = (newEnd - newStart).TotalSeconds
        e.NewValues.Item("RecordingLegth") = newLength
    End Sub
End Class
