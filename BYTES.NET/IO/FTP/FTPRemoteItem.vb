'import .net namespace(s) required
Imports System.Globalization
Imports System.Text.RegularExpressions

Namespace IO.FTP

    Public Class FTPRemoteItem

#Region "private variable(s)"

        Private _months As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer) From {{"Jan", 1}, {"Feb", 2}, {"Mar", 3}, {"Apr", 4}, {"May", 5}, {"Jun", 6}, {"Jul", 7}, {"Aug", 8}, {"Sep", 9}, {"Oct", 10}, {"Nov", 11}, {"Dec", 11}}

        Private _details As String = Nothing

        Private _name As String = String.Empty
        Private _modified As DateTime = Nothing

        Private _connection As Connection = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property Name As String
            Get

                Return _name

            End Get
        End Property

        Public ReadOnly Property Modified As DateTime
            Get

                Return _modified

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="details"></param>
        ''' <param name="connection"></param>
        Public Sub New(ByVal details As String, ByRef connection As Connection)

            'set the variable(s)
            _details = details
            _connection = connection

            'parse the details
            ParseDetails()

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method parsing the details string
        ''' </summary>
        ''' <remarks>based on the article found at 'https://stackoverflow.com/questions/7060983/c-sharp-class-to-parse-webrequestmethods-ftp-listdirectorydetails-ftp-response'</remarks>
        Private Sub ParseDetails()

            'create a new regEx
            Dim pattern As Regex = New Regex(".*(?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\s*(?<day>[0-9]*)\s*(?<yearTime>([0-9]|:)*)\s*(?<fileName>.*)", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

            'parse the details
            Dim match As Match = pattern.Match(_details)

            _name = match.Groups("fileName").Value

            Dim modified As DateTime = Nothing

            Try

                modified = New DateTime(CInt(match.Groups("yearTime").Value), _months(match.Groups("month").Value), CInt(match.Groups("day").Value))

            Catch ex As Exception

            End Try

            _modified = modified

        End Sub

#End Region

    End Class

End Namespace