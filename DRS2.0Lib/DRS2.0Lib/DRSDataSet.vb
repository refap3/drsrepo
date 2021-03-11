Partial Class DRSDataSet
    Partial Public Class DRS20DataTable
        Private Sub DRS20DataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.AirTimeColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class
End Class
