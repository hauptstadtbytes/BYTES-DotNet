'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.WPF.MVVM

'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.Views.MVVM

Namespace ViewModels.MVVM

    Public Class DialogVM

        Inherits DialogViewModel

#Region "private variable(s)"

        Private _myView As DialogView = Nothing

        Private _txt As String = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property Title As String
            Get

                Return "The Sampe Text Editor Dialog"

            End Get
        End Property

        Public Property Text As String
            Get

                Return _txt

            End Get
            Set(value As String)

                _txt = value
                OnPropertyChanged()

            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New()

            'create a new base-class instance
            MyBase.New

            'create the command(s)
            Me.Commands.Add("ApplyCmd", New ViewModelRelayCommand(New Action(AddressOf ApplyDialog)))
            Me.Commands.Add("CancelCmd", New ViewModelRelayCommand(New Action(AddressOf CancelDialog)))

        End Sub

#End Region

#Region "protected method(s) inherited from base-class"

        ''' <summary>
        ''' methdod showing the view
        ''' </summary>
        ''' <remarks>inherited from base-class instance</remarks>
        Protected Overrides Sub ShowView()

            'create a new view instance
            If IsNothing(_myView) Then

                _myView = New DialogView
                _myView.DataContext = Me

            End If

            'show the view
            _myView.Show()

        End Sub

        ''' <summary>
        ''' methdod closing the view
        ''' </summary>
        ''' <remarks>Inherited from base-class instance</remarks>
        Protected Overrides Sub CloseView()

            'close the view
            If Not IsNothing(_myView) Then

                _myView.Close()

            End If

        End Sub

#End Region

#Region "private method(s)"

        Private Sub ApplyDialog()

            Close(True)

        End Sub

        Private Sub CancelDialog()

            If Not Me.DialogResult Then

                Close(False)

            Else 'keep 'applied' (when the 'closed' behavior was attached) 

                Close(True)

            End If

        End Sub

#End Region

    End Class

End Namespace