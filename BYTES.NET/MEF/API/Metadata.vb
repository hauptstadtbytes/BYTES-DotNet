'import .net namespace(s) required
Imports System.ComponentModel.Composition

Namespace MEF.API

    ''' <summary>
    ''' the basic (extension) metadata class 
    ''' </summary>
    ''' <remarks>based on the article found at 'https://stefanhenneken.wordpress.com/2011/06/05/mef-teil-2-metadaten-und-erstellungsrichtlinien'</remarks>
    <MetadataAttribute>
    <AttributeUsage(AttributeTargets.Class)>
    Public Class Metadata

        Inherits ExportAttribute

#Region "public properties"

        Public Property ID As String

        Public ReadOnly Property InterfaceType As Type
            Get

                Return MyBase.ContractType

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="interfaceType"></param>
        Public Sub New(ByVal interfaceType As Type)

            MyBase.New(interfaceType)

        End Sub

#End Region

    End Class

End Namespace