'import namespace(s) required from 'BYTES.NET' assembly
Imports BYTES.NET.Imaging.API

Imports BYTES.NET.WPF.MVVM

Namespace ViewModels.Imaging

    Public Class ImageVM

        Inherits ViewModel

#Region "private variable(s)"

        Private _image As IImage = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property Name As String
            Get

                Return _image.Name

            End Get
        End Property

        Public ReadOnly Property Image As ImageSource
            Get

                Return _image.GetBitmapSource

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New(ByRef image As IImage)

            'create a new base-class instance
            MyBase.New

            'set the variable(s)
            _image = image

        End Sub

#End Region

    End Class

End Namespace