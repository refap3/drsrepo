Imports System
Imports System.Globalization
Imports System.Threading

Public Class DTconvs
    Public Shared Function FormatDateTimeToLocale(ByVal inDate As Date, ByVal trgLocale As String, ByVal fmtString As String) As String
        Dim dt As Date = inDate
        Dim ci As New CultureInfo(trgLocale)
        Dim savedCi As CultureInfo = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = ci
        Dim retedDatestr As String = (Format(dt, fmtString))
        Thread.CurrentThread.CurrentCulture = savedCi
        Return retedDatestr
    End Function

    Public Shared Function FormatDateTimeToLocale(ByVal inDate As Date, ByVal trgLocale As String) As String
        Return FormatDateTimeToLocale(inDate, trgLocale, "MMM dd, yyyy hh:mm:ss tt")
    End Function
    Public Shared Function FormatDateTimeToLocale(ByVal inDate As Date) As String
        Return FormatDateTimeToLocale(inDate, "en-US")
    End Function

    Public Shared Function FormatDateTimeToLocale(ByVal inDate As String, ByVal srcLocale As String, ByVal trgLocale As String, ByVal fmtString As String) As String
        Dim dt As Date
        Dim ci As New CultureInfo(srcLocale)
        Dim savedCi As CultureInfo = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = ci
        dt = inDate
        Thread.CurrentThread.CurrentCulture = savedCi
        Return FormatDateTimeToLocale(dt, trgLocale, fmtString)
    End Function

End Class
