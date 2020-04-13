'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.WPF.MVVM

'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.Views.MVVM

Namespace ViewModels.MVVM

    Public Class GUIThreadSampleVM

        Inherits GUIThreadViewModel(Of GUIThreadSampleView)

#Region "private variable(s)"

        Private _greetings As String = "Hello World!"

#End Region

#Region "public properties"

        Public Property Greetings As String
            Get

                Return _greetings

            End Get
            Set(value As String)

                _greetings = value

                OnPropertyChanged()

            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method(s)
        ''' </summary>
        Public Sub New()

            'create a new base-class instance
            MyBase.New(True)

        End Sub

#End Region

    End Class

End Namespace