Public Class OE1Sendung
    Private strBeginTime As String
    Private strEndTime As String
    Private strDate As String
    Private strType As String
    Private strProgram As String
    Private datStartDate As Date
    Private datEndDate As Date
    Private strMoreInfo As String = ""
    Private strMehrLink As String = ""
    Private strProgramType As String = ""

    Private Sub New()

    End Sub

    Public Sub New(ByVal startTime As String, ByVal startDate As String, ByVal type As String, ByVal program As String, ByVal moreInfo As String, ByVal mehrLink As String, ByVal progType As String)
        strBeginTime = startTime
        strDate = startDate
        strType = type
        strProgram = program
        datStartDate = CDate(strDate & " " & strBeginTime)
        strMoreInfo = moreInfo
        strMehrLink = mehrLink
        strProgramType = progType
    End Sub


    Public ReadOnly Property StartTime() As DateTime

        Get
            Return datStartDate
        End Get
    End Property
    Public ReadOnly Property EndTime() As DateTime
        Get
            Return datEndDate
        End Get
    End Property
    Public ReadOnly Property Duration() As Integer
        Get
            Dim _dur As Integer = (Me.EndTime - Me.StartTime).TotalMinutes
            'Debug.Assert(_dur > 0) ' this kills bloody debugging wot a fcc!
            Return _dur
        End Get
    End Property
    Public ReadOnly Property Program() As String
        Get
            Dim s As String = (strProgram & " " & strMoreInfo)
            If s.Length > 200 Then s = s.Substring(0, 200)
            Return s
        End Get
    End Property
    Public ReadOnly Property MoreInfo() As String
        Get
            Return strMoreInfo
        End Get
    End Property
    Public ReadOnly Property MehrLink() As String
        Get
            Return strMehrLink
        End Get
    End Property
    Public ReadOnly Property ProgramType() As String
        Get
            Return strProgramType
        End Get
    End Property

    Public ReadOnly Property wmRecordaSchedulEntry() As String
        Get
            '            Return "rtsp://stream4.orf.at/oe1-wort title: " & replaceUmlaute(Me.Program) & " (Windows Media) start: " & DTconvs.FormatDateTimeToLocale(Me.StartTime) & " end: " & DTconvs.FormatDateTimeToLocale(Me.EndTime) & " daily: Once authenticate: "
            '             Return "mms://apasf.apa.at/oe1_live_worldwide title: " & "<<filename>>" & " (Windows Media) start: " & DTconvs.FormatDateTimeToLocale(Me.StartTime) & " end: " & DTconvs.FormatDateTimeToLocale(Me.EndTime) & " daily: Once authenticate: "
            Return "http://mp3ooe1.apasf.sf.apa.at title: " & "<<filename>>" & " (Windows Media) start: " & DTconvs.FormatDateTimeToLocale(Me.StartTime) & " end: " & DTconvs.FormatDateTimeToLocale(Me.EndTime) & " daily: Once authenticate: "

        End Get
    End Property

    Public Shared Function unRawWmrecordaEntry(ByVal entry As String, ByVal maxCols As Integer) As String
        ' split by LF
        Dim entries() As String = entry.Split(vbLf)
        Dim outtext As String = ""
        If entry.Length > 2 Then
            For Each s As String In entries
                If Trim(s) <> "" And s.Length > 2 Then
                    Dim prStart, prEnd, prProgram As String
                    prProgram = strBetween(s, "title: ", " (Windows Media) ")
                    If maxCols > 0 Then
                        If prProgram.Length > maxCols Then
                            prProgram = prProgram.Substring(0, maxCols)
                        End If
                    End If
                    prStart = strBetween(s, "start: ", "end: ")
                    prEnd = strBetween(s, "end: ", "daily: ")
                    prStart = DTconvs.FormatDateTimeToLocale(prStart, "en-US", "de-AT", "yyyy-MM-dd HH:mm")
                    prEnd = DTconvs.FormatDateTimeToLocale(prEnd, "en-US", "de-AT", "HH:mm")
                    outtext &= prStart & "-" & prEnd & " " & prProgram & vbCrLf
                End If
            Next
        End If
        Return outtext
    End Function
    Public Shared Function unRawWmrecordaEntry(ByVal entry As String) As String
        Return unRawWmrecordaEntry(entry, 0)
    End Function

    Private Shared Function strBetween(ByVal entry As String, ByVal s1 As String, ByVal s2 As String) As String
        Dim iStart As Integer = entry.IndexOf(s1) + s1.Length
        Dim iLen As Integer = entry.IndexOf(s2) - iStart
        Return entry.Substring(iStart, iLen)
    End Function

    Public Sub SetEndTime(ByVal daystr As String, ByVal endTime As String)
        strEndTime = endTime
        datEndDate = CDate(daystr & " " & strEndTime)
    End Sub

    Public Overrides Function ToString() As String
        Dim toStr As String
        toStr = Format(Me.StartTime, "dd.MM.yy HH:mm ") & Me.strProgramType & " " & Me.strProgram
        If Trim(Me.MoreInfo) <> "" Then toStr &= "-" & Me.strMoreInfo
        toStr &= " (" & Me.Duration & ") "
        Return toStr
    End Function


End Class
