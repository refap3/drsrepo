Imports System.Net
Imports System.Text



Public Class Form1


    Dim myOe1s As OE1Sendung()

    Private Sub NavigateTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim nav2This As String = TextBox1.Text
        WebBrowser1.Navigate(nav2This)
    End Sub

    Private Sub ParseThisURL_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TabControl1.SelectedIndex = 1

        Dim dateTogoFor As Date = DateTimePicker1.Value
        Dim objWebClient As New WebClient()
        Dim strURL As String = TextBox1.Text
        Dim objUTFenc As New UTF8Encoding
        'Loop until we have connectivity 

        myOe1s = ParseOe1ProgTextByRegex(objUTFenc.GetString(objWebClient.DownloadData(strURL)), dateTogoFor)
        FilterCheckBox_CheckedChanged(sender, e)
    End Sub

    Private Sub generateTESTdata_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        ' no display the new OE1 test data 
        myOe1s = ReturnTestData(20)
        FilterCheckBox_CheckedChanged(sender, e)
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged
        TextBox1.Text = ORFBASEURL & "/" & "tag/" & DateTimePicker1.Value.ToString("yyyyMMdd")
        NavigateTo_Click(sender, e)
    End Sub

    Private Sub TabPage2_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage2.Enter
        Try
            ParseThisURL_click(sender, e)
        Catch ex As WebException
            ComplainWebConnect(ex)
        End Try
    End Sub

    Private Sub PreviousDay_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        DateTimePicker1.Value = DateTimePicker1.Value.AddDays(-1)
        ParseThisURL_click(sender, e)
    End Sub

    Private Sub NextDay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        DateTimePicker1.Value = DateTimePicker1.Value.AddDays(1)
        ParseThisURL_click(sender, e)
    End Sub


    Private Sub Record_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        'SaveRecordingInfofromCheckedListBox(CheckedListBox1, cbViaWebService.Checked)
        Dim outOE1s() As OE1Sendung
        outOE1s = Array.CreateInstance(GetType(OE1Sendung), 0)

        For Each thisOe1Prog As OE1Sendung In CheckedListBox1.CheckedItems
            Array.Resize(outOE1s, outOE1s.Length + 1)
            outOE1s(outOE1s.Length - 1) = thisOe1Prog
        Next
        Dim errMsg As String = SaveRecordingInfoTODRS20DATABASE(outOE1s)
        If errMsg <> "" Then ToolStripStatusLabel1.Text = " Probs with DUPLICATES!"
    End Sub

    Private Sub ToggleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim sumRecordingTime As Integer = 0
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, Not CheckedListBox1.GetItemChecked(i))
            If CheckedListBox1.GetItemCheckState(i) Then sumRecordingTime += CType(CheckedListBox1.Items(i), OE1Sendung).Duration
        Next
        ToolStripStatusLabel1.Text = "Duration: " & sumRecordingTime & " MB: " & sumRecordingTime / 60.0 * 30.0

    End Sub

    Private Sub INITRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        TextBox2.Text = writeRecordingInfoFromDRS20Database(cbViaWebService.Checked)
    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckedListBox1.Click
        If CheckedListBox1.SelectedIndex <> -1 Then
            CheckedListBox1.SetItemChecked(CheckedListBox1.SelectedIndex, Not (CheckedListBox1.GetItemCheckState(CheckedListBox1.SelectedIndex)))
        End If
    End Sub

    Private Sub FilterCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Dim testFilter() = LoadFilterList(EXCLUDEFILENAME)
            ToolStripStatusLabel1.Text = FillCheckBoxWithprogInfo(CheckedListBox1, myOe1s, testFilter)
        Else
            ToolStripStatusLabel1.Text = FillCheckBoxWithprogInfo(CheckedListBox1, myOe1s)
        End If
    End Sub

    Private Sub SaveFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        SaveFilterfromCheckedListBox(CheckedListBox1)
    End Sub


    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TextBox1.Text = ORFBASEURL
        'init the WS redirecrion PARAMS 
        InitWSredirection(My.Settings.FILESERVINGHOST, My.Settings.RECORDERHOST)
        If My.Settings.AutoExec Then
            Beep()

            ' set up timer for action !
            tmrAUTOEXEC.Interval = 15000
            tmrAUTOEXEC.Enabled = True

            TabControl1.SelectedIndex = 1 ' goto work page !
            ToolStripStatusLabel1.Text = ("Warning -- AutoEXEC is ON! " & "Kill within next seconds !")
        End If
    End Sub

    Private Sub TabPage3_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage3.Enter
        ' load schedule file in text box 
        TextBox2.Text = ReadFromScheduleFile(cbViaWebService.Checked)
    End Sub



    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click

        SaveRecordingInfoFromCheckedListBoxTODRS10DATABASE(CheckedListBox1)

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        My.Forms.AboutBox1.ShowDialog()
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
        My.Forms.Settings.ShowDialog()
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        MsgBox(DisplayWSredirection())

    End Sub

    Private Sub AUTOEXEC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        If Not My.Settings.AutoExec Then
            MsgBox("AUTOEXEC not enabled!")
        Else
            ' check iff running autoexec ...
            If tmrAUTOEXEC.Enabled Then
                tmrAUTOEXEC.Enabled = False
                MsgBox("AUTOEXEC aborted ...", MsgBoxStyle.Exclamation, "Go and disable under Options!")
                Exit Sub
            End If

            ToolStripStatusLabel1.Text = "AUTOEXEC runnn .."
            ' reparse THIS day 
            ' and wait for web connectivity !
            Dim webConnect As Boolean = False
            Do
                Try
                    ParseThisURL_click(sender, e)
                    webConnect = True
                    ToolStripStatusLabel1.Text = Now.ToLongTimeString & " WEB connect .."
                    ToolStripStatusLabel2.Text = ToolStripStatusLabel1.Text
                    My.Application.DoEvents()
                Catch ex As WebException
                    ComplainWebConnect(ex)
                    My.Application.DoEvents()
                    Threading.Thread.Sleep(5000)
                End Try
            Loop Until webConnect
            ' filter - go to day toggle append 
            CheckBox1.Checked = True ' filter 
            DateTimePicker1.Value = Now ' today
            ToggleButton_Click(sender, e) ' toggle all 
            Record_Click(sender, e) ' record to DB 
            My.Application.DoEvents()
            ' now process next days ...
            For i As Integer = 1 To My.Settings.DaysAheadInAutoExec - 1

                NextDay_Click(sender, e)  ' next day 
                ToggleButton_Click(sender, e) ' toggle all 
                Record_Click(sender, e) ' record to DB 
                My.Application.DoEvents()

            Next

            ' finally write schedule 

            INITRecord_Click(sender, e) ' write schedule 
            My.Application.DoEvents()

        End If
    End Sub

    Private Sub DELETEallWMrecordings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        ToolStripStatusLabel1.Text = DeleteALLrecordingsINDRS20DATABASE(Me.cbViaWebService.Checked)
    End Sub

    Private Sub tmrAUTOEXEC_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrAUTOEXEC.Tick
        'ACTION WITH Autoexec !
        tmrAUTOEXEC.Enabled = False
        AUTOEXEC_Click(sender, e)
        Threading.Thread.Sleep(2000)
        ToolStripStatusLabel1.Text = "Sleep before go !"
        Threading.Thread.Sleep(2000)
        End
    End Sub

    Private Sub ComplainWebConnect(ByVal ex As Exception)
        ToolStripStatusLabel2.Text = Now.ToLongTimeString & " NO web! " & ex.Message
    End Sub
End Class


