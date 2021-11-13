'import .net namespace(s) required
Imports System.Collections.Specialized
Imports System.ComponentModel

Namespace Collections

    ''' <summary>
    ''' an observable dictionary
    ''' </summary>
    ''' <typeparam name="TKey"></typeparam>
    ''' <typeparam name="TValue"></typeparam>
    ''' <remarks>based on the article found at 'http://blogs.microsoft.co.il/shimmy/2010/12/02/observabledictionaryof-tkey-tvalue-vbnet/'</remarks>
    Public Class ObservableDictionary(Of TKey, TValue)

#Region "interface method(s)"

        Implements IDictionary(Of TKey, TValue)

        Implements INotifyPropertyChanged
        Implements INotifyCollectionChanged

        ''' <summary>
        ''' event definitions
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

        Public Event CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs) Implements INotifyCollectionChanged.CollectionChanged

        ''' <summary>
        ''' method for adding new item(s)
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Public Sub Add(item As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add

            Insert(item.Key, item.Value, True)

        End Sub

        Public Sub Add(key As TKey, value As TValue) Implements IDictionary(Of TKey, TValue).Add

            Insert(key, value, True)

        End Sub

        ''' <summary>
        ''' method(s) for removing single item(s)
        ''' </summary>
        ''' <param name="item"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Remove(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Remove

            Return Remove(item.Key)

        End Function

        Public Function Remove(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove

            If IsNothing(key) Then
                Throw New ArgumentNullException("'" & Me.GetType.ToString & "' failed to remove item: 'Key' value may not be 'Nothing'.")
            End If

            Dim removed = _dictionary.Remove(key)

            If removed Then
                OnCollectionChanged()
            End If

            Return removed

        End Function

        ''' <summary>
        ''' method for clearing the inner dictionay from any item
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Clear

            If _dictionary.Count > 0 Then 'check if there are any items listed

                _dictionary.Clear()
                OnCollectionChanged()

            End If

        End Sub

        ''' <summary>
        ''' method for copying the values to an array
        ''' </summary>
        ''' <param name="array"></param>
        ''' <param name="arrayIndex"></param>
        ''' <remarks></remarks>
        Public Sub CopyTo(array() As KeyValuePair(Of TKey, TValue), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo

            _dictionary.CopyTo(array, arrayIndex)

        End Sub

        ''' <summary>
        ''' items count property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Count
            Get

                Return _dictionary.Count

            End Get
        End Property

        ''' <summary>
        ''' property indicating wheather the dictionary is read-only
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).IsReadOnly
            Get

                Return _dictionary.IsReadOnly

            End Get
        End Property

        ''' <summary>
        ''' method for checking for a specific item
        ''' </summary>
        ''' <param name="item"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Contains(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains

            Return _dictionary.Contains(item)

        End Function

        Public Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey

            Return _dictionary.ContainsKey(key)

        End Function

        ''' <summary>
        ''' property for getting/ settings a specific item
        ''' </summary>
        ''' <param name="key"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Property Item(key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
            Get

                If _dictionary.ContainsKey(key) Then
                    Return _dictionary(key)
                Else
                    Return Nothing
                End If

            End Get
            Set(ByVal value As TValue)

                Insert(key, value, False)

            End Set
        End Property

        ''' <summary>
        ''' property containing a list of keys
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Keys As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
            Get

                Return _dictionary.Keys

            End Get
        End Property

        ''' <summary>
        ''' method for 'TryGet'
        ''' </summary>
        ''' <param name="key"></param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue

            Return _dictionary.TryGetValue(key, value)

        End Function

        ''' <summary>
        ''' property containing a list of values
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Values As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
            Get

                Return _dictionary.Values

            End Get
        End Property

        ''' <summary>
        ''' method(s) for getting the enums
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey, TValue)).GetEnumerator

            Return DirectCast(_dictionary, IEnumerable).GetEnumerator

        End Function

        Public Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator

            Return _dictionary.GetEnumerator

        End Function

#End Region

#Region "private variable(s)"

        Private _dictionary As IDictionary(Of TKey, TValue)

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <remarks>needed for serialization</remarks>
        Public Sub New()

            _dictionary = New Dictionary(Of TKey, TValue)

        End Sub

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="dictionary"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal dictionary As IDictionary(Of TKey, TValue))

            _dictionary = New Dictionary(Of TKey, TValue)(dictionary)

        End Sub

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="comparer"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal comparer As IEqualityComparer(Of TKey))

            _dictionary = New Dictionary(Of TKey, TValue)(comparer)

        End Sub

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="capacity"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal capacity As Integer)

            _dictionary = New Dictionary(Of TKey, TValue)(capacity)

        End Sub

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="dictionary"></param>
        ''' <param name="comparer"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal dictionary As IDictionary(Of TKey, TValue), ByVal comparer As IEqualityComparer(Of TKey))

            _dictionary = New Dictionary(Of TKey, TValue)(dictionary, comparer)

        End Sub

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="capacity"></param>
        ''' <param name="comparer"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal capacity As Integer, ByVal comparer As IEqualityComparer(Of TKey))

            _dictionary = New Dictionary(Of TKey, TValue)(capacity, comparer)

        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method indicating wheather a specific value is listed or not
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ContainsValue(ByVal value As TValue) As Boolean

            Return _dictionary.Values.Contains(value)

        End Function

#End Region

#Region "private data-related method(s)"

        ''' <summary>
        ''' method for inserting a new item
        ''' </summary>
        ''' <param name="key"></param>
        ''' <param name="value"></param>
        ''' <param name="forceNew"></param>
        ''' <remarks></remarks>
        Private Sub Insert(ByVal key As TKey, ByVal value As TValue, Optional ByVal forceNew As Boolean = False)

            If IsNothing(key) Then
                Throw New ArgumentNullException("'" & Me.GetType.ToString & "' failed to insert item: 'Key' value may not be 'Nothing'.")
            End If

            If forceNew Then 'check if a new item should be added

                If Not _dictionary.ContainsKey(key) Then

                    _dictionary.Add(key, value)
                    OnCollectionChanged()

                Else

                    Throw New ArgumentException("'" & Me.GetType.ToString & "' failed to add new item with key '" & key.ToString & "': There is already an item with that key listed.")

                End If

            Else 'continue if an existing item should be updated or created

                If _dictionary.ContainsKey(key) Then

                    Dim item As TValue = _dictionary(key)

                    _dictionary(key) = value
                    OnCollectionChanged()

                Else

                    _dictionary.Add(key, value)
                    OnCollectionChanged()

                End If

            End If


        End Sub

#End Region

#Region "private interface-related method(s)"

        ''' <summary>
        ''' method(s) raising an event when a property was changed
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub OnPropertyChanged()

            OnPropertyChanged("Count")
            OnPropertyChanged("Item")
            OnPropertyChanged("Keys")
            OnPropertyChanged("Values")

        End Sub

        Protected Overridable Sub OnPropertyChanged(ByVal propertyName As String)

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))

        End Sub

        ''' <summary>
        ''' method(s) raising an event when the collection was changed
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub OnCollectionChanged()

            OnPropertyChanged()
            RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))

        End Sub

#End Region

    End Class

End Namespace