'import .net namespace(s) required
Imports System.Collections.ObjectModel

'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.WPF.MVVM

'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.ViewModels.MVVM
Imports BYTES.NET.SAMPLE.ViewModels.MEF
Imports BYTES.NET.SAMPLE.ViewModels.Imaging
Imports BYTES.NET.SAMPLE.ViewModels.IO
Imports BYTES.NET.SAMPLE.ViewModels.Logging

Imports BYTES.NET.SAMPLE.ViewModels.Collections.MSSQL
Imports BYTES.NET.SAMPLE.ViewModels.Collections.MSAccess

Namespace ViewModels

    Public Class MainVM

        Inherits ViewModel

#Region "private variable(s)"

        Private _samples As ObservableCollection(Of SampleVM) = New ObservableCollection(Of SampleVM)

#End Region

#Region "public properties"

        Public Property Samples As ObservableCollection(Of SampleVM)
            Get

                Return _samples

            End Get
            Set(value As ObservableCollection(Of SampleVM))

                _samples = value
                OnPropertyChanged()

            End Set
        End Property

#End Region

#Region "public properties"

        Public ReadOnly Property Title As String
            Get

                Return "BYTES.NET Sample Application"

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

            'set the variable(s)
            Me.Samples.Add(New MVVMSampleVM)
            Me.Samples.Add(New ManagedProcessVM)

            Me.Samples.Add(New LocalHostSampleVM)
            Me.Samples.Add(New UNCSampleVM)
            Me.Samples.Add(New FTPSampleVM)
            'Me.Samples.Add(New LDAPSampleVM)

            Me.Samples.Add(New MEFSampleVM)

            Me.Samples.Add(New ImagingSampleVM)
            Me.Samples.Add(New LogSampleVM)

            Me.Samples.Add(New MSSQLServerSampleVM)
            Me.Samples.Add(New MSSAccessSampleVM)

            Me.Samples.Add(New HTTPSampleVM)
            Me.Samples.Add(New TCPSampleVM)

        End Sub

#End Region

    End Class

End Namespace