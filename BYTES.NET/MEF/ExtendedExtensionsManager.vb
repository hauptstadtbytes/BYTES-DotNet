'import .net namespace(s) required
Imports System.Collections.ObjectModel

Imports System.Text.RegularExpressions

'import internal namespace(s) required
Imports BYTES.NET.IO

Imports BYTES.NET.MEF.API

Namespace MEF

    Public Class ExtendedExtensionsManager(Of TInterface, TMetadata)

#Region "private variable(s)"

        Private _enabledExtensions As ObservableCollection(Of Lazy(Of TInterface, TMetadata)) = Nothing

        Private _settings As ExtensionsSettings() = Nothing
        Private _pathMasks As Dictionary(Of String, String) = Nothing

        Private _ignoreDoublingIDs As Boolean = True

#End Region

#Region "public properties"

        Public ReadOnly Property Extensions As Lazy(Of TInterface, TMetadata)()
            Get

                Return _enabledExtensions.ToArray

            End Get
        End Property

        Public Property Settings As ExtensionsSettings()
            Get

                Return _settings

            End Get
            Set(value As ExtensionsSettings())

                _settings = value

                Update() 'update the extension(s) found

            End Set
        End Property

        Public Property IgnoreDoubling As Boolean
            Get

                Return _ignoreDoublingIDs

            End Get
            Set(value As Boolean)

                _ignoreDoublingIDs = value

                Update() 'update the extension(s) found

            End Set
        End Property

        Public Property PathMasks As Dictionary(Of String, String)
            Get

                Return _pathMasks

            End Get
            Set(value As Dictionary(Of String, String))

                _pathMasks = value

                Update() 'update the extension(s) found

            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method(s)
        ''' </summary>
        Public Sub New()

            'create a new base-class instance
            MyBase.New

        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method for loading the extensions
        ''' </summary>
        ''' <param name="settings"></param>
        ''' <param name="masks">masks for extension search paths; will be resolved using the pattern '%name%'</param>
        Public Sub Update(Optional ByVal settings As ExtensionsSettings() = Nothing, Optional ByVal masks As Dictionary(Of String, String) = Nothing)

            'parse the argumente
            If Not IsNothing(settings) Then

                Me.Settings = settings

            End If

            If Not IsNothing(masks) Then

                Me.PathMasks = masks

            End If

            'reset the list of available extensions
            _enabledExtensions = New ObservableCollection(Of Lazy(Of TInterface, TMetadata))

            If IsNothing(Me.Extensions) Then

                Return

            End If

            'get the extensions
            Dim alreadyListedExtensions As List(Of String) = New List(Of String)

            For Each setting As ExtensionsSettings In Me.Settings

                'parse the source paths
                Dim source As String = Helper.ExpandPath(setting.Source, Me.PathMasks)

                Dim paths As String() = {}

                If source.Contains("|") Then

                    paths = Regex.Split(source, "[\s]?\|[\s]?")

                Else

                    paths = {source}

                End If

                'parse the IDs
                Dim ids As String() = {}

                If setting.ID.Contains("|") Then

                    ids = Regex.Split(setting.ID, "[\s]?\|[\s]?")

                Else

                    ids = {setting.ID}

                End If

                'get the extensions
                Dim manager As ExtensionsManager(Of TInterface, TMetadata) = New ExtensionsManager(Of TInterface, TMetadata)
                manager.Update(paths)

                'add the extensions
                For Each extension As Lazy(Of TInterface, TMetadata) In manager.Extensions

                    If IDMatches(extension, ids) Then

                        If Me.IgnoreDoubling Then 'check for doubling IDs

                            Dim meta As IMetadata = DirectCast(extension.Metadata, IMetadata)

                            If Not alreadyListedExtensions.Contains(meta.ID) Then

                                _enabledExtensions.Add(extension)

                                alreadyListedExtensions.Add(meta.ID)

                            End If

                        Else 'do not handle doubling IDs

                            _enabledExtensions.Add(extension)

                        End If

                    End If

                Next

            Next

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method validating the ID
        ''' </summary>
        ''' <param name="extension"></param>
        ''' <param name="ids"></param>
        ''' <returns></returns>
        Private Function IDMatches(ByRef extension As Lazy(Of TInterface, TMetadata), ByVal ids As String()) As Boolean

            'parse (and validate) the id
            Dim exID As String = Nothing

            Try

                Dim meta As IMetadata = DirectCast(extension.Metadata, IMetadata)
                exID = meta.ID

            Catch ex As Exception
            End Try

            If IsNothing(exID) Then

                Return False

            End If

            'validate ID
            For Each id As String In ids

                Select Case True

                    Case id = "*"
                        Return True

                    Case id.StartsWith("*")
                        Dim toCompare As String = id.Substring(1)

                        If exID.EndsWith(toCompare) Then

                            Return True

                        End If

                    Case id.EndsWith("*")
                        Dim toCompare As String = id.Substring(0, id.Length - 2)

                        If exID.StartsWith(toCompare) Then

                            Return True

                        End If

                End Select

            Next

            Return False

        End Function

#End Region

    End Class

End Namespace