'import .net namespace(s) required
Imports System.Xml.Serialization

Namespace MEF

    ''' <summary>
    ''' the settings to be used in combination with the 'ExtendedExtensionsManager'
    ''' </summary>
    <Serializable>
    Public Class ExtensionsSettings

#Region "private variable(s)"

        Private _source As String = Nothing
        Private _id As String = "*"

#End Region

#Region "public properties"

        <XmlAttribute>
        Public Property Source As String
            Get

                Return _source

            End Get
            Set(value As String)

                _source = value

            End Set
        End Property

        <XmlAttribute>
        Public Property ID As String
            Get

                Return _id

            End Get
            Set(value As String)

                _id = value

            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method(s)
        ''' </summary>
        ''' <remarks>required for XML serialization</remarks>
        Public Sub New()
        End Sub

#End Region

    End Class

End Namespace