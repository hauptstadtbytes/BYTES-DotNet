'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.IO.LDAP

'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.IO

Namespace ViewModels.IO.LDAP

    Public Class LDAPSampleVM

        Inherits SampleVM

#Region "private avriable(s)"

        Private _myView As LDAPSampleView = Nothing

        Private _ou As String = Nothing

        Private _users As String() = Nothing
        Private _groups As String() = Nothing

#End Region

#Region "public properties inherited from base-class"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "LDAP Browser"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New LDAPSampleView
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

        Public ReadOnly Property Domain As String
            Get

                Return Manager.GetCurrentDomain().Name

            End Get
        End Property

        Public ReadOnly Property OUs As String()
            Get

                Dim output As List(Of String) = New List(Of String) From {"<Root>"}

                Dim manager As Manager = New Manager()

                For Each item As Dictionary(Of String, Object) In manager.Search("(objectCategory=organizationalUnit)", {"distinguishedname"})

                    output.Add(item("distinguishedname"))

                Next

                Return output.ToArray

            End Get
        End Property

        Public Property OU As String
            Get

                If IsNothing(_ou) And OUs.Count > 0 Then
                    _ou = OUs.First
                End If

                Return _ou

            End Get
            Set(value As String)

                _ou = value
                OnPropertyChanged()
                UpdateCache()

            End Set
        End Property

        Public ReadOnly Property Users As String()
            Get

                Return _users

            End Get
        End Property

        Public ReadOnly Property Groups As String()
            Get

                Return _groups

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

            'update the cache
            UpdateCache()

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method updating the items cache
        ''' </summary>
        Private Sub UpdateCache()

            'parse the ou selected
            Dim path As String = "LDAP://" & OU

            If OU = "<Root>" Then
                path = Manager.GetCurrentDomain(True)
            End If

            'create a new manager class instance
            Dim mngr As Manager = New Manager(path)

            'get all users
            Dim usrs As List(Of String) = New List(Of String)

            For Each item As Dictionary(Of String, Object) In mngr.Search("(&(objectCategory=User)(objectClass=person))", {"name", "distinguishedname"})

                usrs.Add(item("name") & " (" & item("distinguishedname") & ")")

            Next

            _users = usrs.ToArray
            OnPropertyChanged("Users")

            'get all groups
            Dim grps As List(Of String) = New List(Of String)

            For Each item As Dictionary(Of String, Object) In mngr.Search("(objectCategory=Group)", {"name", "distinguishedname"})

                grps.Add(item("name") & " (" & item("distinguishedname") & ")")

            Next

            _groups = grps.ToArray
            OnPropertyChanged("Groups")

        End Sub

#End Region

    End Class

End Namespace