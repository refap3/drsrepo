Imports System.Text.RegularExpressions

Module ROTTENildisassembly
    'old fucking code 


    Public Function ParseOe1ProgTextByRegexOLDNEVERUSELEAVEfordoconly(ByVal prgText As String, ByVal day As Date) As OE1Sendung()






        Dim outOE1s() As OE1Sendung
        outOE1s = Array.CreateInstance(GetType(OE1Sendung), 0)

        ' new regex werks including more, tye an link ...

        ' here is how i did it:

        'sample HTML Code 


        '<div id="mid-section" class="light">
        '<div id="mid-col-1">
        '	<b class="head2">06:57</b><br />
        '	<b class="head1">RELIGION
        '	<br />
        '	 </b>
        '	</div>
        '<div id="mid-col-2">
        '		<div id="mid-col-2x" class="list">
        '		<b class="head2">Gedanken für den Tag</b><br />von Werner Horn zum 400. Geburtstag von Paul Gerhardt<br />
        '		<a href='/programm/20070307065700.html' class="red">mehr...</a><br />
        '		</div>
        '	</div> 
        '</div>


        '' NEW format .....

        '        <div id="mid-section" class="light
        '">
        '	<div id="mid-col-1y">

        '    <b class="head1s">06:57</b><br />
        '	<b class="head1">RELIGION

        '	</b>
        '	</div>
        '	<div id="mid-col-2">
        '		<div id="mid-col-2x" class="list">
        '		<b class="head2">Gedanken für den Tag *</b><br />
        '		von Bischof Johann Weber<br />

        '		<a href='20070407501.html' class="red">mehr...</a>

        '        <br />



        '    </div>
        '	</div> 
        '</div>


        Dim regexstr As String = ""
        regexstr &= "<div id=""mid-section"" class=""[lightdark].*\n.*\n" ' This ist the MAIN Entry point -- class can be light OR dark -- somewhat shammy Regex here 
        regexstr &= "(.*\n){2}"                                         ' just noise 
        regexstr &= "\s*<b class=""head1s"">(?<time>\d\d:\d\d).*\n"     ' contains time 

        regexstr &= "\s*<b class=""head1"">(?<type>.*)\n"               ' contains type 
        regexstr &= "(.*\n){5,7}\s*<b class=""head2"">(?<prog>.*)\n" ' contains program 
        regexstr &= "(?<more>.*)\n"
        regexstr &= "(.*\n){0,1}"                                       ' kann auch mehr als eine zeile sein !! 
        regexstr &= "((\s*<a href='(?<mlink>.*)'.*)|\s*<br />\s*)\n" ' link or a couple of empty lines 

        'regexstr &= "(?<dumm1>.*)\n"     ' dummy test expression 
        'regexstr &= "(?<dumm2>.*)\n"     ' dummy test expression 
        'regexstr &= "(?<dumm3>.*)\n"     ' dummy test expression 
        'regexstr &= "(?<dumm4>.*)\n"     ' dummy test expression 



        Dim regX As New Regex(regexstr, RegexOptions.IgnoreCase)


        Dim mtch As MatchCollection = regX.Matches(prgText)


        Dim lastOe1Prog As OE1Sendung = Nothing
        Dim thisOe1Prog As OE1Sendung = Nothing
        Dim timFound As String = ""
        Dim typeFound As String = ""
        Dim progFound As String = ""
        Dim moreInfFound As String = ""
        Dim lastTimefound As String = "00:00"
        Dim mehrLink As String = ""

        'Dim dumm1 As String = "DUMMY 1"
        'Dim dumm2 As String = "DUMMY 2"
        'Dim dumm3 As String = "DUMMY 3"
        'Dim dumm4 As String = "DUMMY 4"


        For Each ms As Match In mtch

            'dumm1 = ms.Result("${dumm1}")
            'dumm2 = ms.Result("${dumm2}")
            'dumm3 = ms.Result("${dumm3}")
            'dumm4 = ms.Result("${dumm4}")

            timFound = ms.Result("${time}")
            typeFound = ms.Result("${type}")
            progFound = removeControlcharsAndTagsAndSpaces(ms.Result("${prog}").Replace("</b><br />", "")) ' remove trailing tags -- cannot be grepped??
            ' remove downloadable indication (" *")
            If progFound.Substring(progFound.Length - 2, 2) = " *" Then progFound = progFound.Substring(0, progFound.Length - 2)
            moreInfFound = removeControlcharsAndTagsAndSpaces(ms.Result("${more}")).Replace("<br />", "").Replace("<br>", "")

            mehrLink = ms.Result("${mlink}")
            If mehrLink <> "" Then mehrLink = ORFBASEURL & "/" & mehrLink

            If timFound < lastTimefound Then day = day.AddDays(1)
            lastTimefound = timFound

            thisOe1Prog = New OE1Sendung(timFound, Format(day, "yyyy-MM-dd"), "", progFound, moreInfFound, mehrLink, typeFound)
            If Not IsNothing(lastOe1Prog) Then
                lastOe1Prog.SetEndTime(Format(day, "yyyy-MM-dd"), timFound)
            End If

            Array.Resize(outOE1s, outOE1s.Length + 1)
            outOE1s(outOE1s.Length - 1) = thisOe1Prog
            lastOe1Prog = thisOe1Prog

        Next


        If Not IsNothing(lastOe1Prog) Then
            timFound = "05:59" ' hard coded end tima last program 
            If timFound < lastTimefound Then day = day.AddDays(1)
            lastOe1Prog.SetEndTime(Format(day, "yyyy-MM-dd"), timFound) ' HARD coded endtime 
        End If
        Return outOE1s
    End Function





    Public Function THISFUNCTIONISNOTUSEDANYMORE_ParseOe1ProgText(ByVal prgText As String, ByVal day As Date) As OE1Sendung()
        Dim wrk As String = prgText
        Dim outOE1s() As OE1Sendung
        outOE1s = Array.CreateInstance(GetType(OE1Sendung), 0)

        Const DELIM1 = "<!--  Content ROW -->"
        Const DELIM2 = "<!-- - Content ROW  -->"

        ' remove all outside delimiters 

        Dim i As Integer = InStr(wrk, DELIM1)
        wrk = wrk.Substring(i - 1 + DELIM1.Length, wrk.Length - i - DELIM1.Length)

        ' remove trailing part 
        i = InStr(wrk, DELIM2)
        wrk = wrk.Substring(0, i - 1)

        Const INTROTIM = "<b class=""head2"">"
        Const INTROPROG = "<b class=""head2"">"
        Const INTROTYPE = "<b class=""head1"">"

        Dim lastpos As Integer = 0
        Dim lastOe1Prog As OE1Sendung = Nothing
        Dim thisOe1Prog As OE1Sendung = Nothing
        Dim timFound As String = ""
        Dim progFound As String = ""
        Dim typFound As String = ""
        Dim lastTimefound As String = "00:00"

        Do
            lastpos = wrk.IndexOf(INTROTIM, lastpos + 1)
            If lastpos = -1 Then Exit Do
            Dim bracPos1 As Integer = wrk.IndexOf(">", lastpos)
            Dim bracPos2 As Integer = wrk.IndexOf("<", bracPos1)
            timFound = wrk.Substring(bracPos1 + 1, bracPos2 - bracPos1 - 1)
            If timFound < lastTimefound Then day = day.AddDays(1)
            lastTimefound = timFound

            lastpos = wrk.IndexOf(INTROTYPE, lastpos + 1)
            If lastpos = -1 Then Exit Do
            bracPos1 = wrk.IndexOf(">", lastpos)
            bracPos2 = wrk.IndexOf("<", bracPos1)
            typFound = wrk.Substring(bracPos1 + 1, bracPos2 - bracPos1 - 1)
            typFound = removeControlcharsAndTagsAndSpaces(typFound)


            lastpos = wrk.IndexOf(INTROPROG, lastpos + 1)
            bracPos1 = wrk.IndexOf(">", lastpos)
            bracPos2 = wrk.IndexOf("<", bracPos1)
            progFound = wrk.Substring(bracPos1 + 1, bracPos2 - bracPos1 - 1)


            thisOe1Prog = New OE1Sendung(timFound, Format(day, "yyyy-MM-dd"), "", progFound, typFound, "", "")
            If Not IsNothing(lastOe1Prog) Then
                lastOe1Prog.SetEndTime(Format(day, "yyyy-MM-dd"), timFound)
            End If
            Array.Resize(outOE1s, outOE1s.Length + 1)
            outOE1s(outOE1s.Length - 1) = thisOe1Prog
            lastOe1Prog = thisOe1Prog

        Loop Until lastpos = -1

        If Not IsNothing(lastOe1Prog) Then
            timFound = "05:59" ' hard coded end tima last program 
            If timFound < lastTimefound Then day = day.AddDays(1)
            lastOe1Prog.SetEndTime(Format(day, "yyyy-MM-dd"), timFound) ' HARD coded endtime 
        End If

        Return outOE1s
    End Function
End Module
