Namespace Math

    Public Class MatrixStatistics(Of TValue)

        Inherits MatrixData(Of TValue)

#Region "private variable(s)"

        Private _minValue As TValue = Nothing
        Private _maxValue As TValue = Nothing
        Private _distribution As Dictionary(Of TValue, Integer) = New Dictionary(Of TValue, Integer)

#End Region

#Region "public properties"

        Public ReadOnly Property Maximum As TValue
            Get

                Return _maxValue

            End Get
        End Property

        Public ReadOnly Property Minimum As TValue
            Get

                Return _minValue

            End Get
        End Property

        Public ReadOnly Property Distribution As Dictionary(Of TValue, Integer)
            Get

                Return _distribution

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="values"></param>
        Public Sub New(ByVal values As TValue(,))

            'create a new base-class instance
            MyBase.New(values)

            'update the statistics
            UpdateStatistics()

        End Sub

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="xLength"></param>
        ''' <param name="yLength"></param>
        Public Sub New(ByVal xLength As Integer, ByVal yLength As Integer)

            'create a new base-class instance
            MyBase.New(xLength, yLength)

            'update the statistics
            UpdateStatistics()

        End Sub

#End Region

#Region "protected method(s) inherited from base-class"

        ''' <summary>
        ''' method called after value update
        ''' </summary>
        ''' <param name="xCoordinate"></param>
        ''' <param name="yCoordinate"></param>
        ''' <param name="oldValue"></param>
        ''' <param name="newValue"></param>
        Protected Overrides Sub OnValueUpdate(xCoordinate As Integer, yCoordinate As Integer, oldValue As TValue, newValue As TValue)

            UpdateStatistics(oldValue, newValue) 'update the statistics

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method updating the statistics (initially)
        ''' </summary>
        Private Sub UpdateStatistics()

            'create caches
            Dim minCache As List(Of TValue) = New List(Of TValue)
            Dim maxCache As List(Of TValue) = New List(Of TValue)
            Dim distributionCache As List(Of Dictionary(Of TValue, Integer)) = New List(Of Dictionary(Of TValue, Integer))

            'get the local data for each column
            For col = 0 To Me.xLength - 1 Step 1 'loop for each column

                Dim currMinValue As TValue = Nothing
                Dim currMaxValue As TValue = Nothing

                Dim currDistribution As Dictionary(Of TValue, Integer) = New Dictionary(Of TValue, Integer)

                If Me.yLength >= 1 Then 'check if there is one or more rows

                    For row = 0 To Me.yLength - 1 Step 1 'loop for each row

                        'add to distribution list
                        If Not currDistribution.ContainsKey(_values(col, row)) Then

                            currDistribution.Add(_values(col, row), 1)

                        Else

                            Dim count As Integer = currDistribution(_values(col, row)) + 1
                            currDistribution(_values(col, row)) = count

                        End If

                        'check for minimum value
                        If IsNothing(currMinValue) Then

                            currMinValue = _values(col, row)

                        Else

                            If IsLess(_values(col, row), currMinValue) Then

                                currMinValue = _values(col, row)

                            End If

                        End If

                        'check for maximum value
                        If IsNothing(currMaxValue) Then

                            currMaxValue = _values(col, row)

                        Else

                            If IsMore(_values(col, row), currMaxValue) Then

                                currMaxValue = _values(col, row)

                            End If

                        End If

                    Next

                End If

                minCache.Add(currMinValue)
                maxCache.Add(currMaxValue)
                distributionCache.Add(currDistribution)

            Next

            'get the global values
            For Each singleValue As TValue In minCache

                If IsNothing(_minValue) Then

                    _minValue = singleValue

                Else

                    If IsLess(singleValue, _minValue) Then

                        _minValue = singleValue

                    End If

                End If

            Next

            For Each singleValue As TValue In maxCache

                If IsNothing(_maxValue) Then

                    _maxValue = singleValue

                Else

                    If IsMore(singleValue, _maxValue) Then

                        _maxValue = singleValue

                    End If

                End If

            Next

            Dim dist As Dictionary(Of TValue, Integer) = Nothing

            For Each singleValue As Dictionary(Of TValue, Integer) In distributionCache

                If IsNothing(dist) Then

                    If distributionCache.Count > 0 Then

                        dist = singleValue

                    End If

                Else

                    For Each pair As KeyValuePair(Of TValue, Integer) In singleValue

                        If Not dist.ContainsKey(pair.Key) Then

                            dist.Add(pair.Key, pair.Value)

                        Else

                            Dim count As Integer = dist(pair.Key) + pair.Value
                            dist(pair.Key) = count

                        End If

                    Next

                End If

            Next

            _distribution = dist

        End Sub

        ''' <summary>
        ''' method returning a boolean, indicating if a value is bigger than a reference value
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="reference"></param>
        ''' <returns></returns>
        Private Function IsMore(ByVal value As TValue, ByVal reference As TValue) As Boolean

            Select Case True

                Case GetType(TValue).Equals(GetType(Integer)) 'check for 'Integer' type

                    'convert the values
                    Dim currVal As Integer = DirectCast(Convert.ChangeType(value, GetType(TValue)), Integer)
                    Dim refVal As Integer = DirectCast(Convert.ChangeType(reference, GetType(TValue)), Integer)

                    'compare the values
                    If currVal > refVal Then
                        Return True
                    Else
                        Return False
                    End If

                Case GetType(TValue).Equals(GetType(Double)) 'check for 'Double' type

                    'convert the values
                    Dim currVal As Double = DirectCast(Convert.ChangeType(value, GetType(TValue)), Double)
                    Dim refVal As Double = DirectCast(Convert.ChangeType(reference, GetType(TValue)), Double)

                    'compare the values
                    If currVal > refVal Then
                        Return True
                    Else
                        Return False
                    End If

                Case GetType(TValue).Equals(GetType(String)) 'check for 'String' type

                    'convert the values
                    Dim currVal As String = DirectCast(Convert.ChangeType(value, GetType(TValue)), String)
                    Dim refVal As String = DirectCast(Convert.ChangeType(reference, GetType(TValue)), String)

                    If currVal.Length > refVal.Length Then
                        Return True
                    Else
                        Return False
                    End If

                Case Else
                    Throw New Exception("Unable to compare value(s) of type '" & GetType(TValue).ToString() & "'")

            End Select

        End Function

        ''' <summary>
        ''' method returning a boolean, indicating if a value is smaller than a reference value
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="reference"></param>
        ''' <returns></returns>
        Private Function IsLess(ByVal value As TValue, ByVal reference As TValue) As Boolean

            Select Case True

                Case GetType(TValue).Equals(GetType(Integer)) 'check for 'Integer' type

                    'convert the values
                    Dim currVal As Integer = DirectCast(Convert.ChangeType(value, GetType(TValue)), Integer)
                    Dim refVal As Integer = DirectCast(Convert.ChangeType(reference, GetType(TValue)), Integer)

                    'compare the values
                    If currVal < refVal Then
                        Return True
                    Else
                        Return False
                    End If

                Case GetType(TValue).Equals(GetType(Double)) 'check for 'Double' type

                    'convert the values
                    Dim currVal As Double = DirectCast(Convert.ChangeType(value, GetType(TValue)), Double)
                    Dim refVal As Double = DirectCast(Convert.ChangeType(reference, GetType(TValue)), Double)

                    'compare the values
                    If currVal < refVal Then
                        Return True
                    Else
                        Return False
                    End If

                Case GetType(TValue).Equals(GetType(String)) 'check for 'String' type

                    'convert the values
                    Dim currVal As String = DirectCast(Convert.ChangeType(value, GetType(TValue)), String)
                    Dim refVal As String = DirectCast(Convert.ChangeType(reference, GetType(TValue)), String)

                    If currVal.Length < refVal.Length Then
                        Return True
                    Else
                        Return False
                    End If

                Case Else
                    Throw New Exception("Unable to compare value(s) of type '" & GetType(TValue).ToString() & "'")

            End Select

        End Function

        ''' <summary>
        ''' overloading method updating the statistics (on single value change)
        ''' </summary>
        ''' <param name="oldValue"></param>
        ''' <param name="newValue"></param>
        Private Sub UpdateStatistics(ByVal oldValue As TValue, ByVal newValue As TValue)

            'update the distribution
            If Not _distribution.Keys.Contains(newValue) Then

                _distribution.Add(newValue, 1)

            Else

                Dim currValue As Integer = _distribution(newValue)
                _distribution(newValue) = currValue + 1

            End If

            If _distribution.Keys.Contains(oldValue) Then

                Dim currValue As Integer = _distribution(oldValue)

                If (currValue - 1) <= 0 Then

                    _distribution.Remove(oldValue)

                Else

                    _distribution(oldValue) = currValue - 1

                End If

            End If

            'update the minimum and maximum value
            If IsMore(newValue, _maxValue) Then

                _maxValue = newValue

            End If

            If IsLess(newValue, _minValue) Then

                _minValue = newValue

            End If

        End Sub

#End Region

    End Class

End Namespace