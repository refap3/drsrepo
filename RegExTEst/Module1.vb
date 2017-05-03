



Module Module1

    Sub Main()
        Dim testOe1() As OE1Sendung
        Dim oTx As String = My.Computer.FileSystem.ReadAllText("..\..\oe1_ORF_at.htm", New System.Text.UTF8Encoding)

        '' out next lines !
        'testOe1 = ParseOe1ProgTextByRegexTest(oTx, Now)
        'Console.ReadLine()
        'Exit Sub

        testOe1 = ParseOe1ProgTextByRegex(oTx, Now)
        Console.WriteLine("OE1 Sendungs found: " & testOe1.Length)

        Dim safetyExit As Integer = 0
        For Each oe As OE1Sendung In testOe1
            safetyExit += 1
            Console.WriteLine(oe.StartTime & " " & oe.Program)
            Console.WriteLine("I: " & oe.MoreInfo)
            'Console.WriteLine("L: " & oe.MehrLink) ' Link is dead

            If safetyExit > 10 Then Exit For ' COMMENT this line for full coverage 
        Next
        Console.ReadLine()

    End Sub




End Module