'import .net namespace(s) required
Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Namespace WPF

    Public MustInherit Class Observable

        Implements INotifyPropertyChanged

#Region "'INotifypropertyChanged' interface implementation"

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        ''' <summary>
        ''' method for raising the 'PropertyChanged' event on property changed
        ''' </summary>
        ''' <param name="propertyName"></param>
        ''' <remarks></remarks>
        Public Overridable Sub OnPropertyChanged(<CallerMemberName> Optional ByVal propertyName As String = Nothing)

            'raise the 'PropertyChanged' event
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))

        End Sub

        ''' <summary>
        ''' method for notifying on all properties changed
        ''' </summary>
        ''' <remarks>based on the article found at 'http://jobijoy.blogspot.de/2009/07/easy-way-to-update-all-ui-property.html'</remarks>
        Public Overridable Sub OnAllPropertiesChanged()

            'raise the 'PropertyChanged' event
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(Nothing))

        End Sub

#End Region

    End Class

End Namespace