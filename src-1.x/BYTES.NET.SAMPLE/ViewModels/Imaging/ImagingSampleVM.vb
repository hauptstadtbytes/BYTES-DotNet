'import .net namespace(s) required
Imports Microsoft.Win32

Imports System.IO

'import namespace(s) required from 'BYTES.NET' assembly
Imports BYTES.NET.MEF

Imports BYTES.NET.Imaging.API

Imports BYTES.NET.WPF.MVVM

Imports BYTES.NET.IO

'import internal namepsace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.Imaging

Namespace ViewModels.Imaging

    Public Class ImagingSampleVM

        Inherits SampleVM

#Region "private variable(s)"

        Private _myView As ImagingSampleView = Nothing

        Private _parsers As ExtendedExtensionsManager(Of IImageParser, IImageParserMetadata) = New ExtendedExtensionsManager(Of IImageParser, IImageParserMetadata)

        Private _source As String = Nothing
        Private _parser As Lazy(Of IImageParser, IImageParserMetadata) = Nothing

        Private _images As ImageVM() = Nothing
        Private _image As ImageVM = Nothing

#End Region

#Region "public properties inherited from base-class instance"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "Imaging"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New ImagingSampleView
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

        Public ReadOnly Property Source As String
            Get

                Return _source

            End Get
        End Property

        Public ReadOnly Property Parsers As Lazy(Of IImageParser, IImageParserMetadata)()
            Get

                Return _parsers.Extensions

            End Get
        End Property

        Public Property Parser As Lazy(Of IImageParser, IImageParserMetadata)
            Get

                Return _parser

            End Get
            Set(value As Lazy(Of IImageParser, IImageParserMetadata))

                _parser = value
                OnPropertyChanged()

            End Set
        End Property

        Public ReadOnly Property Images As ImageVM()
            Get

                Return _images

            End Get
        End Property

        Public Property Image As ImageVM
            Get

                Return _image

            End Get
            Set(value As ImageVM)

                _image = value

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

            'load the image parser(s)
            Dim settings As ExtensionsSettings = New ExtensionsSettings With {.Source = "%BYTES.NET%"}
            _parsers.Settings = {settings}

            'add the command(s)
            Me.Commands.Add("BrowseForSourceCmd", New ViewModelRelayCommand(New Action(AddressOf BrowseForSource)))
            Me.Commands.Add("LoadImageCmd", New ViewModelRelayCommand(New Action(AddressOf LoadImage)))

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method browsing for a source file
        ''' </summary>
        Private Sub BrowseForSource()

            'create a new dialog
            Dim dialog As OpenFileDialog = New OpenFileDialog

            With dialog
                '.InitialDirectory = Helper.ExpandPath("%InstallationDir%..\..\..\..\Samples\Imaging")
                .RestoreDirectory = True
                .Multiselect = False
            End With

            If dialog.ShowDialog Then

                'set the image source
                _source = dialog.FileName
                OnPropertyChanged("Source")

                'try to get the correct parser
                Dim info As FileInfo = New FileInfo(dialog.FileName)

                For Each extension As Lazy(Of IImageParser, IImageParserMetadata) In Me.Parsers

                    If Not IsNothing(extension.Metadata.FileExtensions) Then

                        For Each ext As String In extension.Metadata.FileExtensions

                            If ext.ToLower = info.Extension.ToLower().Replace(".", "") Then

                                Me.Parser = extension

                                Exit For

                            End If

                        Next

                    End If

                Next

            End If

        End Sub

        ''' <summary>
        ''' method loading the image
        ''' </summary>
        Private Sub LoadImage()

            'validate the argument(s)
            If IsNothing(Me.Source) OrElse String.IsNullOrEmpty(Me.Source) Then

                MsgBox("The source must not be empty")

                Return

            End If

            If IsNothing(Me.Parser) Then

                MsgBox("The parser must not be empty")

                Return

            End If

            'load the image(s)
            Dim img As List(Of ImageVM) = New List(Of ImageVM)

            For Each image As IImage In Me.Parser.Value.Load(Me.Source)

                img.Add(New ImageVM(image))

            Next

            _images = img.ToArray

            OnPropertyChanged("Images")

            'pre-select the first image
            If Me.Images.Count > 0 Then

                Me.Image = Me.Images.First

            End If

        End Sub

#End Region

    End Class

End Namespace