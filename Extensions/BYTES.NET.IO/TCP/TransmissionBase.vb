Namespace TCP

    Public MustInherit Class TransmissionBase

#Region "public events"

        Public Event Logged(ByVal message As String)

#End Region

#Region "public properties"

        Public MustOverride ReadOnly Property ID As String

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New()
        End Sub

#End Region

#Region "protected method(s)"

        ''' <summary>
        ''' method for writing to log
        ''' </summary>
        ''' <param name="level"></param>
        ''' <param name="message"></param>
        Public Sub Log(ByVal level As String, ByVal message As String)

            'raise the event
            RaiseEvent Logged("[" & Me.ID & "] " & DateTime.Now.ToString & ";" & level & ";" & message)

        End Sub

        ''' <summary>
        ''' overloaded method for writing to log
        ''' </summary>
        ''' <param name="message"></param>
        ''' <remarks>thought for use in 'Transmitter' instance</remarks>
        Protected Sub Log(ByVal message As String)

            RaiseEvent Logged(message)

        End Sub

#End Region

    End Class

End Namespace