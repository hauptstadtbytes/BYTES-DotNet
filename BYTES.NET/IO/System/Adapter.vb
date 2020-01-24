'import .net namespace(s) required
Imports System.Net.NetworkInformation

Namespace IO.System

    Public Class Adapter

#Region "private variable(s)"

        Private _interface As NetworkInterface = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property Name As String
            Get

                Return _interface.Name

            End Get
        End Property

        Public ReadOnly Property Description As String
            Get

                Return _interface.Description

            End Get
        End Property

        Public ReadOnly Property ID As String
            Get

                Return _interface.Id

            End Get
        End Property

        Public ReadOnly Property Address As String
            Get

                Return _interface.GetPhysicalAddress.ToString

            End Get
        End Property

        Public ReadOnly Property Type As NetworkInterfaceType
            Get

                Return _interface.NetworkInterfaceType

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="intrfce"></param>
        Public Sub New(ByVal intrfce As NetworkInterface)

            'set the variable(s)
            _interface = intrfce

        End Sub

#End Region

    End Class

End Namespace