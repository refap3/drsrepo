Imports System.Net
Partial Class DisplayOE1
    Inherits System.Web.UI.Page

    Dim myOe1s As OE1Sendung()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            Calendar1.SelectedDate = Now
            Calendar1_SelectionChanged(sender, e)

        Else
            myOe1s = Session("myoe1")
        End If
    End Sub
    Private Sub FilterCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Dim testFilter() As String = LoadFilterList(HttpContext.Current.Server.MapPath("App_data\" & EXCLUDEFILENAME))
            Label1.Text = FillCheckBoxWithprogInfo(CheckBoxList1, myOe1s, testFilter)
        Else
            Label1.Text = FillCheckBoxWithprogInfo(CheckBoxList1, myOe1s)
        End If
    End Sub

    Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        Dim strUrl As String = ORFBASEURL & "/" & "tag/" & Calendar1.SelectedDate.ToString("yyyyMMdd")
        Nav2This(strUrl, Calendar1.SelectedDate)
    End Sub


    Private Sub Nav2This(ByVal url As String, ByVal dateSelected As Date)
        Dim objWebClient As New WebClient()
        Dim objUTF7 As New UTF8Encoding()
        myOe1s = ParseOe1ProgTextByRegex(objUTF7.GetString(objWebClient.DownloadData(url)), dateSelected)
        Session("myoe1") = myOe1s
        FilterCheckBox_CheckedChanged(Nothing, Nothing)
    End Sub

    Protected Sub SaveFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim testFilter() As String = Array.CreateInstance(GetType(String), 0)
        For i As Integer = 0 To CheckBoxList1.Items.Count - 1
            If CheckBoxList1.Items(i).Selected Then
                Array.Resize(testFilter, testFilter.Length + 1)
                For Each oe1 As OE1Sendung In myOe1s
                    If oe1.ToString = CheckBoxList1.Items(i).ToString Then
                        testFilter(testFilter.Length - 1) = oe1.Program
                    End If
                Next
            End If
        Next
        SaveFilterList(Server.MapPath("App_data\" & EXCLUDEFILENAME), testFilter)
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Dim strUrl As String = ORFBASEURL & "/" & "tag/" & Calendar1.SelectedDate.ToString("yyyyMMdd")
        Response.Redirect(strUrl)
    End Sub

    Protected Sub RecordINIT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        DeleteScheduleFile(cbViaWebService.Checked)
        Label1.Text = writeRecordingInfoFromDRS20Database(cbViaWebService.Checked)
    End Sub

    Protected Sub RecordAPPEND_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click, Button10.Click
        'SaveRecordingInfofromCheckedListBox(CheckBoxList1, myOe1s, cbViaWebService.Checked)
        Dim outOE1s() As OE1Sendung
        outOE1s = Array.CreateInstance(GetType(OE1Sendung), 0)
        For Each li As ListItem In CheckBoxList1.Items
            If li.Selected Then
                For i As Integer = 0 To myOe1s.Length - 1
                    If li.Value = myOe1s(i).ToString Then
                        Array.Resize(outOE1s, outOE1s.Length + 1)
                        outOE1s(outOE1s.Length - 1) = myOe1s(i)
                        Exit For
                    End If
                Next
            End If
        Next
        Dim errMsg As String = SaveRecordingInfoTODRS20DATABASE(outOE1s)
        If errMsg <> "" Then Label1.Text = "DUPLICATES were removed!" & vbCrLf & errMsg
        ' go for tomorrow too 
        If sender Is Button10 Then Tomorrow_Click(sender, e)
    End Sub

    Protected Sub Today_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        Calendar1.SelectedDate = Now
        Calendar1_SelectionChanged(sender, e)
    End Sub

    Protected Sub Yesterday_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button5.Click
        Calendar1.SelectedDate = Calendar1.SelectedDate.AddDays(-1)
        Calendar1_SelectionChanged(sender, e)
    End Sub

    Protected Sub Tomorrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button6.Click
        Calendar1.SelectedDate = Calendar1.SelectedDate.AddDays(1)
        Calendar1_SelectionChanged(sender, e)
    End Sub

    Protected Sub ToggleSel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button7.Click
        For Each li As ListItem In CheckBoxList1.Items
            li.Selected = Not li.Selected
        Next
    End Sub

    Protected Sub TestDataGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button8.Click
        ' generate test data 
        myOe1s = ReturnTestData(20, tbLength.Text)
        Session("myoe1") = myOe1s
        FilterCheckBox_CheckedChanged(Nothing, Nothing)
    End Sub


    Protected Sub Button9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button9.Click
        Label1.Text = DisplayWSredirection()
    End Sub

    Protected Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        DeleteALLrecordingsINDRS20DATABASE(True)
    End Sub
End Class
