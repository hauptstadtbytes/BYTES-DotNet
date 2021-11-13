'import .net namespace(s) required
Imports System.IO

'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.IO.UNC

Imports BYTES.NET.WPF.MVVM

Namespace ViewModels.IO

    Public Class FolderVM

        Inherits ViewModel

#Region "private variable(s)"

        Private _connection As Connection = Nothing
        Private _path As String = Nothing

        Private _name As String = Nothing
        Private _children As FolderVM() = {}

#End Region

#Region "public properties"

        Public ReadOnly Property Name As String
            Get

                Return _name

            End Get
        End Property

        Public ReadOnly Property Children As FolderVM()
            Get

                Return _children

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="connection"></param>
        ''' <param name="path"></param>
        Public Sub New(ByRef connection As Connection, Optional ByVal path As String = Nothing)

            'create a new base-class instance
            MyBase.New

            'set the variable(s)
            _connection = connection
            _path = path

            If IsNothing(_path) Then

                _name = _connection.Path

            Else

                _name = _path

            End If

            'load the first sub-layer
            If IsNothing(_path) Then

                Dim children As List(Of FolderVM) = New List(Of FolderVM)

                For Each folder As DirectoryInfo In _connection.GetFolders(_path)

                    Children.Add(New FolderVM(_connection, folder.FullName.Replace(_connection.Path, "")))

                Next

                _children = children.ToArray
                OnPropertyChanged("Children")

            End If

        End Sub

#End Region

    End Class

End Namespace