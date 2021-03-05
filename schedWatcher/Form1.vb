Imports System.IO


Public Class Form1

    Const WAITTIMEBETWEENSCHEFILEUPDATES = 60 ' in secs 

    Dim t As Threading.Thread
    Delegate Sub SetTextCallback(ByVal [text] As String)
    Dim mediaFILEEXTENSION = My.Settings.renameFILEEXTENSION.ToLower





    'needed to update controls from other thread !

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FileSystemWatcher1.Path = My.Computer.FileSystem.GetParentPath(getSCHEDULETEXTFILEname())
        InitWSredirection(My.Settings.FILESERVINGHOST, My.Settings.RECORDERHOST)
        addListBoxInfo("inited file system wotcher @ " & getSCHEDULETEXTFILEname())
    End Sub

    Private Sub FileSystemWatcher1_Changed(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs) Handles FileSystemWatcher1.Changed
        addListBoxInfo("change detected to: " & e.FullPath)
        If Trim(e.FullPath).ToLower = Trim(getSCHEDULETEXTFILEname()).ToLower Then
            addListBoxInfo("SCHED file changed -- DO NOT restart wmrecorder!")
            If Not IsNothing(t) Then
                t.Abort()
                addListBoxInfo("ABORT old hound waiting thread!!")
            End If
            t = New Threading.Thread(AddressOf AcTION)
            'Do NOT kill !
            't.Start()
        End If
    End Sub

    Private Sub addListBoxInfo(ByVal s As String)

        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.ListBox1.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf addListBoxInfo)
            Me.Invoke(d, New Object() {s})
        Else
            ListBox1.Items.Add(Now.ToString("HH:mm:ss") & " " & s)
            ListBox1.SelectedIndex = ListBox1.Items.Count - 1
        End If
    End Sub

    Private Sub addLocalTime(ByVal s As String)
        ' same invoke shit as above ...
        If Me.StatusStrip1.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf addLocalTime)
            Me.Invoke(d, New Object() {s})
        Else
            tslbLocalTime.Text = s
        End If
    End Sub

    Private Sub addInternetTime(ByVal s As String)
        ' same invoke shit as above ...
        If Me.StatusStrip1.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf addInternetTime)
            Me.Invoke(d, New Object() {s})
        Else
            tslbInternetTime.Text = s
        End If
    End Sub

    Sub AcTION()
        addListBoxInfo("thread START")

        Dim noASFhadlesOpen As Boolean = False

        Do
            Threading.Thread.Sleep(WAITTIMEBETWEENSCHEFILEUPDATES * 1000)
            addListBoxInfo("thread wake (again) check open handles")

            Dim handls As String = recStatus()
            noASFhadlesOpen = (Trim(handls) = "")

            If noASFhadlesOpen Then
                addListBoxInfo("no HANDLES found -- assume recorder dead ")
            Else
                addListBoxInfo("open handles found assumme recorder running ...")
                addListBoxInfo(handls)
            End If
        Loop Until noASFhadlesOpen

        Dim restartStaus As String = recRestart()
        If restartStaus <> "" Then addListBoxInfo(restartStaus)

        addListBoxInfo("file system wotcher DONE")

        Dim t As Threading.Thread = Threading.Thread.CurrentThread
        t.Abort()
        t = Nothing
    End Sub

    Private Function recRestart() As String
        Dim reslt As String = ""
        Dim killa As String = Split(PATHTOWMRECORDA, "\")(Split(PATHTOWMRECORDA, "\").Length - 1)
        Try
            Process.Start("pskill", killa)
        Catch ex As Exception
            reslt &= vbCrLf & ("KILLA failed: " & ex.Message)
        End Try

        Threading.Thread.Sleep(2 * 1000)
        Try
            Process.Start(PATHTOWMRECORDA)
        Catch ex As Exception
            reslt &= vbCrLf & ("Restart failed: " & ex.Message)
        End Try
        Return reslt
    End Function

    Private Function recStatus() As String
        ' return possible open handles to .asf files 
        ' could be parded batter by regex !!
        Dim noASFhadlesOpen As Boolean = False
        Dim p As Process
        Dim si As New ProcessStartInfo("cmd")
        si.WindowStyle = ProcessWindowStyle.Hidden

        si.Arguments = "/C handle.exe -a -p wmrurl  " & mediaFILEEXTENSION & " | findstr.exe  -i  " & mediaFILEEXTENSION & " >c:\asf.tmp"
        p = Process.Start(si)
        p.WaitForExit()

        Dim handls As String = My.Computer.FileSystem.ReadAllText("c:\asf.tmp", New System.Text.ASCIIEncoding())
        Return Trim(handls)
    End Function



    Private Sub tmrArchive_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrArchive.Tick
        ' come here at timer tick and try to archive some files 
        Dim di As New DirectoryInfo(My.Settings.asfWATCHdirectory)
        For Each fil As FileInfo In di.GetFiles
            If (fil.Extension).ToLower = mediaFILEEXTENSION Then
                addListBoxInfo("try MOV media fil: " & fil.Name)
                Dim yr As String, mo As String, dy As String, hh As String, mm As String
                yr = fil.CreationTime.Year.ToString
                mo = fil.CreationTime.Month.ToString("00")
                dy = fil.CreationTime.Day.ToString("00")
                hh = fil.CreationTime.Hour.ToString("00")
                mm = fil.CreationTime.Minute.ToString("00")

                If My.Settings.IncludeHHMMinFilename Then
                    dy += "-" & hh & mm
                End If

                Dim trgDir As String = My.Settings.asfARCHIVEdirectory
                trgDir &= "\" & yr
                If Not IO.Directory.Exists(trgDir) Then IO.Directory.CreateDirectory(trgDir)
                trgDir &= "\" & mo
                If Not IO.Directory.Exists(trgDir) Then IO.Directory.CreateDirectory(trgDir)

                Try
                    Dim dstPath As String = trgDir & "\" & dy & "-" & fil.Name

                    'fil.MoveTo(Path.ChangeExtension(dstPath, ".mp3"))    ' 9.1.18 it IS already MP3 -- !!
                    ' ALWAYS produces bloody error  but looks OK -- error is from AppendToRecordLog !!!

                    'addListBoxInfo("Lofile path: " & getPathtoAppData())

                    fil.MoveTo(dstPath)    ' 9.1.18 it IS already MP3 -- !!
                    addListBoxInfo(" MOVEed ..." & dstPath)
                    AppendToRecordLog("OK: " & dstPath)
                Catch ex As Exception
                    addListBoxInfo("MOV faild- maybe still OK: " & ex.Message)
                End Try
            End If
        Next

    End Sub

    Private Sub tmrClockSync_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrClockSync.Tick
        ''I did it again with seperate threads -- this bloody things hangs often at first sync attempt!
        'CLOCKSYNC() ' try it directly this time ! change this with thread abort bolow !!
        'Exit Sub
        Dim t As New Threading.Thread(AddressOf CLOCKSYNC)
        t.Start()
    End Sub


    Sub CLOCKSYNC()
        ' keep the local around clock 30 secs behind the internet clock 
        ' this will go bongo when there is no internet connection but what shalls ...
        Daytime.SetWindowsClock(Daytime.GetTime().AddSeconds(-My.Settings.WEBRADIOTIMELAGINSECS))
        Try
            addInternetTime("I: " & Daytime.GetTime())
        Catch ex As Exception
            addListBoxInfo("EXC in timSync: " & ex.Message)
        End Try
        addLocalTime("L: " & Now)

        Threading.Thread.CurrentThread.Abort()
        t = Nothing


    End Sub


    Private Sub Form1_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Application.ExitThread()
        End
    End Sub


    Private Sub ArchiveNOWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArchiveNOWToolStripMenuItem.Click
        Me.Text = "sW manual archv @: " & Now.ToLongTimeString
        tmrArchive_Tick(sender, e)
    End Sub

    Private Sub ClockSyncNOWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClockSyncNOWToolStripMenuItem.Click
        ' do a clock sync now !
        Me.Text = "sW manual clk sync @: " & Now.ToLongTimeString
        tmrClockSync_Tick(sender, e)
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.Click
        My.Forms.Settings.ShowDialog()
    End Sub
End Class
