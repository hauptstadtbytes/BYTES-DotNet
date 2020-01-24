Namespace Logging

    Public Class LogEntry

#Region "public variable(s)"

        Public Enum InformationLevel
            Debug = 0
            Info = 1
            Warning = 2
            Exception = 3
            Fatal = 4
        End Enum

#End Region

#Region "private variable(s)"

        Private _timeStamp As DateTime = Nothing

        Private _level As LogEntry.InformationLevel = Nothing

        Private _message As String = Nothing
        Private _details As Object = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property TimeStamp As DateTime
            Get

                Return _timeStamp

            End Get
        End Property

        Public ReadOnly Property Level As LogEntry.InformationLevel
            Get

                Return _level

            End Get
        End Property

        Public ReadOnly Property Message As String
            Get

                Return _message

            End Get
        End Property

        Public ReadOnly Property Details As Object
            Get

                Return _details

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method(s)
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="level"></param>
        ''' <param name="details"></param>
        Public Sub New(ByVal message As String, Optional ByVal level As LogEntry.InformationLevel = InformationLevel.Info, Optional ByRef details As Object = Nothing)

            'set the variable(s)
            _timeStamp = DateTime.Now

            _level = level

            _message = message
            _details = details

        End Sub

#End Region

    End Class

End Namespace