Imports System.Windows.Forms
Imports System.Web.UI.WebControls
Imports System.Web.HttpRequest
Imports System.Text.RegularExpressions
Imports System.Net
Imports System.Text

Public Module Functions

    Public Const ORFBASEURL = "http://oe1.orf.at/programm"
    Public Const ORFPINGURL = "oe1.orf.at"
    Public Const EXCLUDEFILENAME = "Exclude.txt"
    Public Const LOGFILENAME = "RecordLog.txt"
    Public Const PATHTOWMRECORDA = "C:\Program Files\WMR11\WMR11.exe"
    Public Const RELATIVEWMRTARGETHOSTWEBSERVICEURL = "/drs2.0/WSFileAccess.asmx"

    Private WMRHOST As String = "-"  ' set this to deploy and retrieve schedule file via WS, i.e. wmrecorda
    Private FILEHOST As String = "-" ' set his iff DB files are to located via a WS call, i.e. localhost

    Public Sub InitWSredirection(ByVal hostForFiles As String, ByVal hostForRecording As String)
        WMRHOST = hostForRecording
        FILEHOST = hostForFiles
    End Sub

    Public Function DisplayWSredirection() As String
        Return "FILEHOST " & FILEHOST & " " & "WMRHOST " & WMRHOST & " " & getDRS20DATABASEPATH()
    End Function




    ' this is the deleted fucked up version !
    ' recovered by .net reflector !!

    Public Function ParseOe1ProgTextByRegex(ByVal prgText As String, ByVal day As DateTime) As OE1Sendung()

        Dim lastOe1Prog As OE1Sendung = Nothing
        Dim thisOe1Prog As OE1Sendung = Nothing
        Dim timFound As String = ""
        Dim typeFound As String = ""
        Dim progFound As String = ""
        Dim moreInfFound As String = ""
        Dim lastTimefound As String = "00:00"
        Dim mehrLink As String = ""
        Dim outOE1s As OE1Sendung() = Array.CreateInstance(GetType(OE1Sendung), 0)

        '' Tried out a new way: 
        'instead of regexping over multiple lines i now do this 
        '1. find sthe start pattern 
        '2. grab the next 9 lines or so 
        '3. figure out which line is which 
        '4. remove HTML residues AFTER regexping 
        ' ALL dead nittl in 2017 
        ' lines look like this now 


        'Dim mtch As MatchCollection = New Regex(((((("<h3>(<a class=""blk"".*){0,1}<span>(?<dumm1>.*)</span>(?<dumm2>.*?)(</a>){0,1}</h3>") & "(.*\n){2}" & "(?<dumm3>.*)\n") & "(?<dumm4>.*)\n" & "(?<dumm5>.*)\n") & "(?<dumm6>.*)\n" & "(?<dumm7>.*)\n") & "(?<dumm8>.*)\n" & "(?<dumm9>.*)\n"), RegexOptions.IgnoreCase).Matches(prgText)
        'Dim mtch As MatchCollection = New Regex(((((("<h3>(<a class=""blk"".*){0,1}<span>(?<dumm1>.*)</span>(?<dumm2>.*?)(</a>){0,1}</h3>") & "(.*\n){2}" & "(?<dumm3>.*)\n") & "(?<dumm4>.*).*\n" & "(?<dumm5>.*)\n") & "(?<dumm6>.*)\n" & "(?<dumm7>.*)\n") & "(?<dumm8>.*)\n" & "(?<dumm9>.*)\n"), RegexOptions.IgnoreCase).Matches(prgText)

        '             <h3 data-detail-url="/broadcast/473914"><span class="programTime"> 5:00</span><a href="/programm/20170504/473914">Das Wichtigste zu Tagesbeginn</a></h3>
        Dim mtch As MatchCollection = New Regex((".*data-detail-url.*<span class=""programTime"">(?<time>.*)</span><a href=""(?<link>.*)"">(?<title>.*)</a></h3>.*\n"), RegexOptions.IgnoreCase).Matches(prgText)
        Dim ntim As String = ""
        Dim ntit As String = ""
        Dim nlink As String = ""


        Dim ms As Match
        For Each ms In mtch
            ntim = Right(("0" & Trim(ms.Groups("time").Value)), 5) ' must add leading zero for later comparison 
            ntit = Trim(ms.Groups("title").Value)
            nlink = Trim(ms.Groups("link").Value)
            Debug.WriteLine(ntim & "##" & ntit & "##" & nlink)

            timFound = ntim
            progFound = removeControlcharsAndTagsAndSpaces(ntit)
            typeFound = "" ' gibts nimma !
            moreInfFound = "TBD"


            'MEHR Link logic ...
            mehrLink = nlink
            mehrLink = ("http://oe1.orf.at" & mehrLink)


            Dim objWebClient As New WebClient()
            Dim objUTFenc As New UTF8Encoding
            Dim fullMoreInfo As String

            ' TODO: do NOT process moreInfo here this takes too long -- will do it at storage time! 
            'fullMoreInfo = objUTFenc.GetString(objWebClient.DownloadData(mehrLink))
            ' we look for this
            '<p class="teaserText">"Hören und Zuhören". Persönliche Affinitäten zu Stimmen, zu Klang und Musik, auch über die Stille, über Hören und Gehört-Werden, über Zuhören als erste Kontaktaufnahme mit dem Du von Daniel Landau, Lehrer und Dirigent. - Gestaltung: Alexandra Mantler</p>
            'Dim teasermtch As MatchCollection = New Regex((".*<p class=""teaserText"">(?<teaser>.*)</p>.*\n"), RegexOptions.IgnoreCase).Matches(fullMoreInfo)
            'moreInfFound = ""
            'For Each ms2 As Match In teasermtch
            '    moreInfFound = Trim(ms2.Groups("teaser").Value)
            'Next

            moreInfFound = Trim(removeControlcharsAndTagsAndSpaces(moreInfFound))
            If timFound < lastTimefound Then day = day.AddDays(1)
            lastTimefound = timFound

            thisOe1Prog = New OE1Sendung(timFound, Strings.Format(day, "yyyy-MM-dd"), "", progFound, moreInfFound, mehrLink, typeFound)
            If Not IsNothing(lastOe1Prog) Then
                lastOe1Prog.SetEndTime(Strings.Format(day, "yyyy-MM-dd"), timFound)
            End If

            Array.Resize(outOE1s, outOE1s.Length + 1)
            outOE1s((outOE1s.Length - 1)) = thisOe1Prog
            lastOe1Prog = thisOe1Prog
        Next

        If Not IsNothing(lastOe1Prog) Then
            timFound = "05:59" ' hard coded end time of last program 
            If timFound < lastTimefound Then day = day.AddDays(1)
            lastOe1Prog.SetEndTime(Strings.Format(day, "yyyy-MM-dd"), timFound)
        End If
        Return outOE1s
    End Function












    Public Function ParseOe1ProgTextByRegexTest(ByVal prgText As String, ByVal day As Date) As OE1Sendung()
        ' this is only a test sub called by RegExTest Program ... 

        Dim outOE1s() As OE1Sendung
        outOE1s = Array.CreateInstance(GetType(OE1Sendung), 0)


        'Dim regX As New Regex("<b class=""head2"">(?<prog>.*)(<a href='(?<mlink>.*)'.*mehr.*</a><br />){0,1}", RegexOptions.IgnoreCase)
        'Dim regX As New Regex("<b class=""head2"">(?<prog>.*)|(<a href='(?<mlink>.*)'.*mehr.*</a><br />)", RegexOptions.IgnoreCase)
        'Dim regX As New Regex("<div id=""mid-section"" class=""light|dark", RegexOptions.IgnoreCase + RegexOptions.Multiline)
        Dim regX As New Regex("<div id=""mid-section"" class=""[lightdark]*(?<more>.*)\s*</div>\s*\s*</div>\s*\s*</div>\s*", RegexOptions.Singleline + RegexOptions.IgnoreCase)


        Dim mtch As MatchCollection = regX.Matches(prgText)


        Dim lastOe1Prog As OE1Sendung = Nothing
        Dim thisOe1Prog As OE1Sendung = Nothing
        Dim timFound As String = ""
        Dim typeFound As String = ""
        Dim progFound As String = ""
        Dim moreInfFound As String = ""
        Dim lastTimefound As String = "00:00"
        Dim mehrLink As String = ""

        Dim safEx As Integer = 0

        Console.WriteLine("BEGIN MATHC")
        For Each ms As Match In mtch
            safEx += 1
            timFound = ms.Result("${time}")
            typeFound = ms.Result("${type}")
            progFound = ms.Result("${prog}")
            moreInfFound = removeControlcharsAndTagsAndSpaces(ms.Result("${more}")).Replace("<br />", "")
            mehrLink = ORFBASEURL & ms.Result("${mlink}")


            Console.WriteLine("TI: " & timFound)
            'Console.WriteLine("Ty: " & typeFound)
            'Console.WriteLine("PR: " & progFound)
            Console.WriteLine("MO: " & moreInfFound)
            'Console.WriteLine("LI: " & mehrLink)

            'Console.WriteLine()


            'If safEx > 5 Then Exit For
        Next
        Console.WriteLine("MATHED: " & safEx)
        Return outOE1s
    End Function
    Public Function removeControlcharsAndTagsAndSpaces(ByVal str As String) As String
        ' remove &quot; AND &nbsp; as well ! and atags and double spaces 
        Dim s As String = str

        Do While s.IndexOf("<") >= 0 AndAlso s.IndexOf(">") >= 0
            Dim iFrom = s.IndexOf("<")
            Dim iTo = s.IndexOf(">")
            s = Left(s, iFrom) & Right(s, s.Length - iTo - 1)
        Loop

        s = s.Replace("  ", " ")
        s = s.Replace(Chr(9), "")
        s = s.Replace(Chr(10), "")
        s = s.Replace("&quot;", "")
        s = s.Replace("&nbsp;", " ")
        s = s.Replace("""", "")
        Return Trim(s)
    End Function

    Public Function LoadFilterList(ByVal filename As String) As String()
        Dim inText As String = My.Computer.FileSystem.ReadAllText(filename)
        Return inText.Split(vbCrLf)
    End Function


    Public Sub SaveFilterList(ByVal filename As String, ByVal filterd() As String)
        SaveFilterList(filename, filterd, True)
    End Sub
    Public Sub SaveFilterList(ByVal filename As String, ByVal filterd() As String, ByVal doAPPEND As Boolean)
        Dim outText As String = vbCrLf & String.Join(vbCrLf, filterd)
        My.Computer.FileSystem.WriteAllText(filename, Trim(outText), doAPPEND, New System.Text.ASCIIEncoding)
    End Sub

    Public Sub AppendToRecordLog(ByVal txt As String)
        My.Computer.FileSystem.WriteAllText(getPathtoAppData() & LOGFILENAME, txt & vbCrLf, True)
    End Sub

    Public Function FillCheckBoxWithprogInfo(ByVal clb As CheckedListBox, ByVal myOe1s As OE1Sendung(), ByVal filtered() As String)
        Dim sumRecordingTime As Integer = 0
        clb.Items.Clear()
        For Each oneOE1s As OE1Sendung In myOe1s
            Dim skipp As Boolean = False
            For Each filt As String In filtered
                filt = removeControlcharsAndTagsAndSpaces(filt).ToLower
                If Trim(filt) <> "" Then skipp = (oneOE1s.Program.ToLower.IndexOf(filt) <> -1)
                If skipp Then Exit For
            Next
            If Not skipp Then
                If Now < oneOE1s.EndTime Then
                    clb.Items.Add(oneOE1s, False)
                    sumRecordingTime += oneOE1s.Duration
                End If

            End If
        Next
        Return "Duration: " & sumRecordingTime & " MB: " & sumRecordingTime / 60.0 * 30.0
    End Function
    Public Function FillCheckBoxWithprogInfo(ByVal clb As CheckBoxList, ByVal myOe1s As OE1Sendung(), ByVal filtered() As String)
        Dim sumRecordingTime As Integer = 0
        clb.Items.Clear()
        For Each oneOE1s As OE1Sendung In myOe1s
            Dim skipp As Boolean = False
            For Each filt As String In filtered
                filt = removeControlcharsAndTagsAndSpaces(filt).ToLower
                If Trim(filt) <> "" Then skipp = (oneOE1s.Program.ToLower.IndexOf(filt) <> -1)
                If skipp Then Exit For
            Next
            If Not skipp Then
                If Now < oneOE1s.EndTime Then
                    clb.Items.Add(oneOE1s.ToString)
                    sumRecordingTime += oneOE1s.Duration
                End If

            End If
        Next
        Return "Duration: " & sumRecordingTime & " MB: " & sumRecordingTime / 60.0 * 30.0
    End Function
    Public Function FillCheckBoxWithprogInfo(ByVal clb As CheckedListBox, ByVal myOe1s As OE1Sendung())
        Dim dummyf() As String = Array.CreateInstance(GetType(String), 0)
        Return FillCheckBoxWithprogInfo(clb, myOe1s, dummyf)
    End Function
    Public Function FillCheckBoxWithprogInfo(ByVal clb As CheckBoxList, ByVal myOe1s As OE1Sendung())
        Dim dummyf() As String = Array.CreateInstance(GetType(String), 0)
        Return FillCheckBoxWithprogInfo(clb, myOe1s, dummyf)
    End Function

    Public Sub SaveFilterfromCheckedListBox(ByVal clb As CheckedListBox)
        Dim testFilter() As String = Array.CreateInstance(GetType(String), 0)
        For Each oe1Prog As OE1Sendung In clb.CheckedItems
            Array.Resize(testFilter, testFilter.Length + 1)
            testFilter(testFilter.Length - 1) = oe1Prog.Program
        Next
        SaveFilterList(EXCLUDEFILENAME, testFilter)
    End Sub

    Public Sub SaveRecordingInfoFromCheckedListBoxTODRS10DATABASE(ByVal clb As CheckedListBox)
        ' enter checked info to old DRS database 

        Dim ds As New DRSDataSet
        Dim ta As New DRSDataSetTableAdapters.DRSTableAdapter
        ta.Fill(ds.DRS)
        For Each oe1Prog As OE1Sendung In clb.CheckedItems
            Dim rw As DRSDataSet.DRSRow = ds.DRS.NewDRSRow

            rw.MP3OutFileName = "y:\mp3\" & DRSCorrectFileNameChars(oe1Prog.Program)
            rw.RecordingLegth = oe1Prog.Duration * 60
            rw.DoEncode = True
            rw.RecordingTime = oe1Prog.StartTime
            rw.EncoderBitrate = 6
            rw.SampleRate = 2
            rw.BurnerMode = 1
            rw.EncoderQuality = 2
            rw.BurnerSpeed = 0
            ds.DRS.Rows.Add(rw)
        Next
        ta.Update(ds.DRS)
    End Sub

    Public Function SaveRecordingInfoTODRS20DATABASE(ByVal mySelectedOes1s() As OE1Sendung) As String
        ' save to DRS 2.0 DB 
        Dim ds As New DRSDataSet
        Dim ta As DRSDataSetTableAdapters.DRS20TableAdapter = getDRS20TableAdapter()

        For Each oe1Prog As OE1Sendung In mySelectedOes1s
            Dim rw As DRSDataSet.DRS20Row = ds.DRS20.NewDRS20Row
            rw.MP3OutFileName = trimToThisLength(oe1Prog.Program, ds.DRS20.MP3OutFileNameColumn.MaxLength)
            rw.RecordingLegth = oe1Prog.Duration * 60
            rw.RecordingTime = oe1Prog.StartTime
            rw.Beschreibung = trimToThisLength(oe1Prog.MoreInfo, ds.DRS20.BeschreibungColumn.MaxLength)
            rw.WMrecorderEntry = oe1Prog.wmRecordaSchedulEntry
            ds.DRS20.Rows.Add(rw)
        Next

        ' update and forget all already present rows !
        Dim errRows As String = ""
        Dim updatesDone As Boolean = False
        ' this is bloody BRUTE !!
        Do
            Try
                ta.Update(ds.DRS20)
                updatesDone = True
            Catch ex As Exception
                For Each rw As DRSDataSet.DRS20Row In ds.DRS20.GetErrors()
                    errRows &= ex.Message & vbCrLf
                    errRows &= rw.MP3OutFileName & " " & rw.RecordingTime & vbCrLf
                    rw.ClearErrors()
                    rw.AcceptChanges()
                Next
            End Try
        Loop Until updatesDone
        Return errRows
    End Function
    Private Function trimToThisLength(ByVal s As String, ByVal maxLength As Integer) As String
        Dim sLoc As String = s
        If sLoc.Length > maxLength Then sLoc = sLoc.Substring(0, maxLength)
        Return sLoc
    End Function

    Public Function DeleteALLrecordingsINDRS20DATABASE(ByVal viaWebService As Boolean) As String
        ' BRUTE delete all in DRS 2.0 DB 
        ' attach to DRS 2.0 DB 
        Dim ds As New DRSDataSet
        Dim ta As DRSDataSetTableAdapters.WmrecordaTableAdapter = getwmrecordaTableAdapter()
        Dim outtext As String = ""
        Try
            ta.DeleteWholeTableQuery()
            DeleteScheduleFile(viaWebService)
        Catch ex As Exception
            outtext = ex.Message
        End Try
        Return outtext
    End Function

    Public Function writeRecordingInfoFromDRS20Database(ByVal viaWebService As Boolean) As String
        ' attach to DRS 2.0 DB 
        Dim ds As New DRSDataSet
        Dim ta As DRSDataSetTableAdapters.WmrecordaTableAdapter = getwmrecordaTableAdapter()
        ta.Fill(ds.Wmrecorda)

        Dim outText As String = ""

        For Each rw As DRSDataSet.WmrecordaRow In ds.Wmrecorda.Rows
            outText &= rw.WMrecorderEntry.Replace("<<filename>>", replaceUmlaute(rw.MP3OutFileName)) & vbCrLf
        Next
        DeleteScheduleFile(viaWebService)
        Return (writeToSchedFile(outText, viaWebService))
    End Function


    Private Function getDRS20TableAdapter() As DRSDataSetTableAdapters.DRS20TableAdapter
        ' returs a  patched table adapter 
        ' first patch the connection string iff the web service is available 
        ' otherwise try to live with current connection !
        Dim ta As New DRSDataSetTableAdapters.DRS20TableAdapter
        ta.Connection.ConnectionString = patchMyConnectionString(ta.Connection.ConnectionString)
        Return ta
    End Function
    Private Function getwmrecordaTableAdapter() As DRSDataSetTableAdapters.WmrecordaTableAdapter
        ' returs a  patched table adapter 
        ' first patch the connection string iff the web service is available 
        ' otherwise try to live with current connection !
        Dim ta As New DRSDataSetTableAdapters.WmrecordaTableAdapter
        ta.Connection.ConnectionString = patchMyConnectionString(ta.Connection.ConnectionString)
        Return ta
    End Function

    Private Function patchMyConnectionString(ByVal inConnstr As String) As String
        Dim chgConn As String = inConnstr

        'patch the connection via the webservice given  -iff any 
        If FILEHOST <> "" Then
            Dim ss As String = "Data Source="
            chgConn = chgConn.Substring(0, chgConn.IndexOf(ss) + ss.Length) & """" & getDRS20DATABASEPATH() & """"
        End If

        Return chgConn
    End Function

    Public Function SaveRecordingInfofromCheckedListBox(ByVal clb As CheckedListBox, ByVal viaWebService As Boolean) As String
        'this is from Windows APP 
        Dim schedFileText As String = ""
        For Each oe1Prog As OE1Sendung In clb.CheckedItems
            schedFileText &= oe1Prog.wmRecordaSchedulEntry & vbCrLf
        Next
        Return writeToSchedFile(schedFileText, viaWebService)
    End Function
    Public Function SaveRecordingInfofromCheckedListBox(ByVal clb As CheckBoxList, ByVal myooe1s As OE1Sendung(), ByVal viaWebService As Boolean) As String
        ' this is from WebApp
        Dim schedFileText As String = ""
        For Each li As ListItem In clb.Items
            If li.Selected Then
                For Each oe1 As OE1Sendung In myooe1s
                    If li.Text = oe1.ToString Then
                        schedFileText &= oe1.wmRecordaSchedulEntry & vbCrLf
                        Exit For
                    End If
                Next
            End If
        Next
        Return writeToSchedFile(schedFileText, viaWebService)
    End Function

    Public Function writeToSchedFile(ByVal schedContent As String, ByVal viaWebService As Boolean) As String
        Dim outText As String = ""

        If viaWebService Then
            Dim ws As WSfileacc.WSFileAccess = GetWsReferenceForRecordingHost()
            ws.SaveScheduleFile(schedContent)
            outText = OE1Sendung.unRawWmrecordaEntry(ws.RetrieveScheduleFile())
        Else
            Dim schedFileName As String = getSCHEDULETEXTFILEname()
            Try
                outText = My.Computer.FileSystem.ReadAllText(schedFileName)
            Catch ex As Exception
                outText = vbCrLf
            End Try
            outText &= schedContent
            My.Computer.FileSystem.WriteAllText(schedFileName, outText, False, New System.Text.ASCIIEncoding)
            My.Computer.FileSystem.WriteAllText(schedFileName & ".BGINFO", OE1Sendung.unRawWmrecordaEntry(outText, 45), False, New System.Text.ASCIIEncoding)
            outText = My.Computer.FileSystem.ReadAllText(schedFileName)
        End If

        Return outText
    End Function

    Public Function ReadFromScheduleFile(ByVal viaWebService As Boolean) As String
        Return ReadFromScheduleFile(viaWebService, False)
    End Function

    Public Function ReadFromScheduleFile(ByVal viaWebService As Boolean, ByVal rawDisplay As Boolean) As String
        Dim outText As String = ""

        If viaWebService Then
            Dim ws As WSfileacc.WSFileAccess = GetWsReferenceForRecordingHost()
            outText = ws.RetrieveScheduleFile()
        Else
            Try
                outText = My.Computer.FileSystem.ReadAllText(getSCHEDULETEXTFILEname())
            Catch ex As Exception
            End Try
        End If
        If Not rawDisplay Then outText = OE1Sendung.unRawWmrecordaEntry(outText)
        Return outText

    End Function


    Public Sub DeleteScheduleFile(ByVal viaWebService As Boolean)
        If viaWebService Then
            Dim ws As WSfileacc.WSFileAccess = GetWsReferenceForRecordingHost()
            ws.DeleteScheduleFile()
        Else
            Try
                My.Computer.FileSystem.DeleteFile(getSCHEDULETEXTFILEname())
            Catch ex As Exception
            End Try
        End If
    End Sub

    Public Function getSCHEDULETEXTFILEname() As String
        Return My.Settings.SCHEDULETEXTFILE
    End Function

    Public Function getDRS20DATABASEPATH() As String
        ' this will return the path on the WEB Server not neccessarily on the WM recorder running site !
        Dim ws As WSfileacc.WSFileAccess = GetWsReferenceFileHoldingHost()
        Return ws.pathToDRS20Database()
    End Function

    Public Function getPathtoAppData() As String
        ' this will return the path on the WEB Server not neccessarily on the WM recorder running site !
        Dim ws As WSfileacc.WSFileAccess = GetWsReferenceFileHoldingHost()
        Return ws.pathToAppData()
    End Function



    Private Function GetWsReferenceForRecordingHost() As WSfileacc.WSFileAccess

        ' check target host for RECORDING was speced 
        If WMRHOST = "-" Or Trim(WMRHOST) = "" Then
            Throw New Exception("Target HOST for Recorder (WMRHOST) not set !")
        End If

        Dim ws As New WSfileacc.WSFileAccess
        ws.Url = "http://" & WMRHOST & RELATIVEWMRTARGETHOSTWEBSERVICEURL
        Return ws
    End Function

    Private Function GetWsReferenceFileHoldingHost() As WSfileacc.WSFileAccess

        ' check target host for RECORDING was speced 
        If FILEHOST = "-" Then Throw New Exception("Target HOST for File hosting host (FILEHOST) not set !")

        Dim ws As New WSfileacc.WSFileAccess
        ws.Url = "http://" & FILEHOST & RELATIVEWMRTARGETHOSTWEBSERVICEURL
        Return ws
    End Function

    Public Function ReturnTestData(ByVal numberOfItems As Integer) As OE1Sendung()
        'generate some Test data         
        Dim outOE1s() As OE1Sendung
        outOE1s = Array.CreateInstance(GetType(OE1Sendung), 0)

        Dim someDate As Date = Now.AddMinutes(1)

        For i As Integer = 1 To numberOfItems
            Dim newOe1 As OE1Sendung
            newOe1 = New OE1Sendung(someDate.ToString("HH:mm"), someDate.ToString("yyyy-MM-dd"), "test", "prog " & i.ToString("000") & "-min-" & someDate.ToString("mm"), "no more info", "", "TESTTYPE")
            someDate = someDate.AddMinutes(1)
            newOe1.SetEndTime(someDate.ToString("yyyy-MM-dd"), someDate.ToString("HH:mm"))

            Dim tmp As Integer = newOe1.Duration 'debug helper 
            Array.Resize(outOE1s, outOE1s.Length + 1)
            outOE1s(outOE1s.Length - 1) = newOe1

        Next
        Return outOE1s
    End Function

    Function DRSCorrectFileNameChars(ByVal inFil) As String
        Dim regEXP
        ' replace umlaute
        DRSCorrectFileNameChars = inFil
        DRSCorrectFileNameChars = Replace(DRSCorrectFileNameChars, "ä", "ae")
        DRSCorrectFileNameChars = Replace(DRSCorrectFileNameChars, "Ä", "Ae")
        DRSCorrectFileNameChars = Replace(DRSCorrectFileNameChars, "ö", "oe")
        DRSCorrectFileNameChars = Replace(DRSCorrectFileNameChars, "Ö", "Oe")
        DRSCorrectFileNameChars = Replace(DRSCorrectFileNameChars, "ü", "ue")
        DRSCorrectFileNameChars = Replace(DRSCorrectFileNameChars, "Ü", "Ue")
        DRSCorrectFileNameChars = Replace(DRSCorrectFileNameChars, "ß", "ss")
        regEXP = CreateObject("vbscript.regEXP")
        regEXP.Global = True
        regEXP.IgnoreCase = True
        regEXP.Pattern = "[^a-z0-9\_\-\+\(\)\ \:\\]"
        Return regEXP.Replace(DRSCorrectFileNameChars, "_")
    End Function

    Public Function replaceUmlaute(ByVal str As String) As String
        Return str.Replace("ä", "ae").Replace("ö", "oe").Replace("ü", "ue").Replace("Ä", "Ae").Replace("Ö", "Oe").Replace("Ü", "Ue").Replace("ß", "ss")
    End Function
End Module
