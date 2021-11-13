'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.IO

Namespace ViewModels.IO

    Public Class LocalHostSampleVM

        Inherits SampleVM

#Region "private avriable(s)"

        Private _myView As LocalHostSampleView = Nothing

        Private _info As NET.IO.System.Info = New NET.IO.System.Info()

#End Region

#Region "public properties inherited from base-class"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "Localhost"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New LocalHostSampleView
                    _myView.DataContext = Me

                End If

                Return _myView

            End Get
            Set(value As UserControl)

                _myView = value
                _myView.DataContext = Me

                OnPropertyChanged()

            End Set
        End Property

#End Region

#Region "public properties"

        Public ReadOnly Property FQDN As String
            Get

                Return _info.Name & "." & _info.Domain

            End Get
        End Property

        Public ReadOnly Property User As String
            Get

                Return _info.CurrentUser.Domain & "\" & _info.CurrentUser.Name

            End Get
        End Property

        Public ReadOnly Property RAM As String
            Get

                Return _info.Memory.ToString & " GB"

            End Get
        End Property

        Public ReadOnly Property CPUs As String
            Get

                Return _info.Processors.ToString

            End Get
        End Property

        Public ReadOnly Property Drives As String()
            Get

                Dim output As List(Of String) = New List(Of String)

                For Each group As KeyValuePair(Of System.IO.DriveType, List(Of BYTES.NET.IO.System.Drive)) In _info.Drives

                    For Each item As BYTES.NET.IO.System.Drive In group.Value

                        output.Add(item.Path & "; " & item.Type.ToString & " (" & item.FreeSpace.ToString & "/" & item.TotalSpace.ToString & ")")

                    Next

                Next

                Return output.ToArray

            End Get
        End Property

        Public ReadOnly Property NICs As String()
            Get

                Dim output As List(Of String) = New List(Of String)

                For Each group As KeyValuePair(Of System.Net.NetworkInformation.NetworkInterfaceType, List(Of BYTES.NET.IO.System.Adapter)) In _info.Adapters

                    For Each item As BYTES.NET.IO.System.Adapter In group.Value

                        output.Add(item.Name & "; " & item.Type.ToString & " (" & item.Address & ")")

                    Next

                Next

                Return output.ToArray

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New()

            'create a new base-class instance
            MyBase.New

        End Sub

#End Region

    End Class

End Namespace