'import .net namespace(s) required
Imports System.ComponentModel.Composition.Hosting
Imports System.ComponentModel.Composition

Imports System.Collections.ObjectModel

Imports System.IO

Namespace MEF

    ''' <summary>
    ''' the basic MEF-based extensions manager
    ''' </summary>
    ''' <typeparam name="TInterface"></typeparam>
    ''' <typeparam name="TMetadata"></typeparam>
    Public Class ExtensionsManager(Of TInterface, TMetadata)

#Region "private variable(s)"

        <ImportMany()>
        Private _allExtensions As ObservableCollection(Of Lazy(Of TInterface, TMetadata)) = New ObservableCollection(Of Lazy(Of TInterface, TMetadata))
        Private _validExtensions As ObservableCollection(Of Lazy(Of TInterface, TMetadata)) = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property Extensions As Lazy(Of TInterface, TMetadata)()
            Get

                Return _validExtensions.ToArray

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method(s)
        ''' </summary>
        Public Sub New()
        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method for loading the extensions from path(s) given
        ''' </summary>
        ''' <param name="paths"></param>
        Public Sub Update(ByVal paths As String())

            'assemble the catalog
            Dim catalog = New AggregateCatalog()

            For Each path As String In paths

                'parse the path
                Dim tmp As Uri = Nothing
                If Uri.TryCreate(path, UriKind.Absolute, tmp) Then 'URIs will be kept and file/ folder paths will be converted to URI

                    path = tmp.LocalPath 'set the local path of the URI item

                End If

                'add extensions for a single file
                If File.Exists(path) Then

                    catalog.Catalogs.Add(New AssemblyCatalog(path))

                End If

                'add extensions for entire directories
                If path.Contains("*") Then

                    Dim pathSplit As String() = path.Split("\")
                    Dim lastElement As String = pathSplit(pathSplit.Length - 1)

                    If lastElement.Contains("*") Then 'the last element contains a wildcard

                        Dim rootPath As String = path.Substring(0, path.Length - (lastElement.Length + 1))

                        If Directory.Exists(rootPath) Then 'check for the root path

                            Dim rootDir As DirectoryInfo = New DirectoryInfo(rootPath)

                            If lastElement.Contains(".") Then 'the last element is a file

                                For Each file As FileInfo In rootDir.GetFiles(lastElement)

                                    catalog.Catalogs.Add(New AssemblyCatalog(file.FullName))

                                Next

                            Else 'the last element is a folder

                                For Each dir As DirectoryInfo In rootDir.GetDirectories(lastElement)

                                    catalog.Catalogs.Add(New DirectoryCatalog(dir.FullName))

                                Next

                            End If

                        End If

                    End If

                Else 'the last element contains no wildcard

                    If Directory.Exists(path) Then

                        catalog.Catalogs.Add(New DirectoryCatalog(path))

                    End If

                End If

            Next

            'load extensions from current assembly
            'catalog.Catalogs.Add(New AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()))

            'compose the collection
            Dim container As CompositionContainer = New CompositionContainer(catalog)
            container.ComposeParts(Me)

            'validate the extensions
            ValidateExtensions()

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method validating the extension(s) by type
        ''' </summary>
        Private Sub ValidateExtensions()

            _validExtensions = New ObservableCollection(Of Lazy(Of TInterface, TMetadata))

            For Each extension In _allExtensions

                Try

                    If ImplementsInterface(extension) Then

                        _validExtensions.Add(extension)

                    End If

                Catch ex As Exception

                End Try

            Next

        End Sub

        ''' <summary>
        ''' method checking if a class implements the 'TInterface' interface
        ''' </summary>
        ''' <param name="extension"></param>
        ''' <returns></returns>
        Private Function ImplementsInterface(ByRef extension) As Boolean

            For Each iFace In extension.Value.GetType.GetInterfaces

                If iFace.ToString = GetType(TInterface).ToString Then

                    Return True

                End If

            Next

            Return False

        End Function

#End Region

    End Class

End Namespace