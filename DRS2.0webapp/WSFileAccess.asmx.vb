Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WSFileAccess
    Inherits System.Web.Services.WebService


    <WebMethod()> _
    Public Sub SaveScheduleFile(ByVal schedFileContent As String)

        'WRITING of bginfo file is OK without ws but des NOT werk with WS 
        ' i bloody do NOT KNOW Y?

        'Debug.WriteLine("WS--ENTER  SaveScheduleFile ")
        Try
            'Debug.WriteLine("WS--ABOUT to write BGINFO file ")
            'My.Computer.FileSystem.WriteAllText(getSCHEDULETEXTFILEname() & ".BGINFO", OE1Sendung.unRawWmrecordaEntry(schedFileContent), False, New System.Text.ASCIIEncoding)
            'Debug.WriteLine("WS--ABOUT to write schedule file ")
            My.Computer.FileSystem.WriteAllText(getSCHEDULETEXTFILEname(), schedFileContent, False, New System.Text.ASCIIEncoding)
        Catch ex As Exception
            Debug.WriteLine("WS--EXECPTION: " & ex.Message)
        End Try

    End Sub

    <WebMethod()> _
        Public Function RetrieveScheduleFile() As String
        Dim outText As String = ""
        Try
            outText = My.Computer.FileSystem.ReadAllText(getSCHEDULETEXTFILEname())
        Catch ex As Exception
        End Try
        Return outText
    End Function

    <WebMethod()> _
        Public Sub DeleteScheduleFile()
        'Debug.WriteLine("WS--ENTER  DeleteScheduleFile ")
        Try
            My.Computer.FileSystem.DeleteFile(getSCHEDULETEXTFILEname())
        Catch ex As Exception
            Debug.WriteLine("WS--EXECPTION: " & ex.Message)
        End Try
    End Sub
    <WebMethod()> _
        Public Function pathToDRS20Database() As String
        Return Server.MapPath("~/App_Data/DRS2.0.mdb")
    End Function
    <WebMethod()> _
        Public Function pathToAppData() As String
        Return Server.MapPath("~/App_Data/")
    End Function


End Class