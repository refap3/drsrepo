
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' set up WS redirection config 
        Dim appSettings As NameValueCollection = System.Web.Configuration.WebConfigurationManager.AppSettings
        InitWSredirection(appSettings("FILESERVINGHOST"), appSettings("RECORDERHOST"))
    End Sub
End Class

