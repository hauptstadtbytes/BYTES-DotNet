Namespace Math

    Public Class MatrixData(Of TValue)

#Region "protected variable(s)"

        Protected _values As TValue(,) = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property xLength As Integer
            Get

                Return _values.GetLength(0)

            End Get
        End Property

        Public ReadOnly Property yLength As Integer
            Get

                Return _values.GetLength(1)

            End Get
        End Property

        ''' <summary>
        ''' </summary>
        ''' <param name="xCoordinate"></param>
        ''' <param name="yCoordinate"></param>
        ''' <returns></returns>
        ''' <remarks>coordinates have to be given 1-based</remarks>
        Public Property Value(ByVal xCoordinate As Integer, ByVal yCoordinate As Integer) As TValue
            Get

                ValidateCoordinates(xCoordinate, yCoordinate)

                Return _values(xCoordinate - 1, yCoordinate - 1)

            End Get
            Set(value As TValue)

                ValidateCoordinates(xCoordinate, yCoordinate)

                Dim oldValue As TValue = _values(xCoordinate - 1, yCoordinate - 1) 'get the old value
                _values(xCoordinate - 1, yCoordinate - 1) = value 'set the new value

                OnValueUpdate(xCoordinate, yCoordinate, oldValue, value)

            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="values"></param>
        Public Sub New(ByVal values As TValue(,))

            'set the variable(s)
            _values = values

        End Sub

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="xLength"></param>
        ''' <param name="yLength"></param>
        Public Sub New(ByVal xLength As Integer, ByVal yLength As Integer)

            'set the variable(s)
            _values = New TValue(xLength - 1, yLength - 1) {} 'due to the default behavior of multidimensional arrays, the dimensions have to be reduced by one

        End Sub

#End Region

#Region "protected method(s)"

        ''' <summary>
        ''' method called after value update
        ''' </summary>
        ''' <param name="xCoordinate"></param>
        ''' <param name="yCoordinate"></param>
        ''' <param name="oldValue"></param>
        ''' <param name="newValue"></param>
        Protected Overridable Sub OnValueUpdate(ByVal xCoordinate As Integer, ByVal yCoordinate As Integer, ByVal oldValue As TValue, ByVal newValue As TValue)
        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method validating coordinates given
        ''' </summary>
        ''' <param name="xCoordinate"></param>
        ''' <param name="yCoordinate"></param>
        Private Sub ValidateCoordinates(ByVal xCoordinate As Integer, ByVal yCoordinate As Integer)

            If (xCoordinate - 1 < 0) Then

                Throw New ArgumentException("Failed to validate x-coordinate. All coordinates have to be 1-based.")

            End If

            If (xCoordinate - 1 > Me.xLength) Then

                Throw New ArgumentException("Failed to validate x-coordinate. The value given is bigger than the x-Length.")

            End If

            If (yCoordinate - 1 < 0) Then

                Throw New ArgumentException("Failed to validate y-coordinate. All coordinates have to be 1-based.")

            End If

            If (yCoordinate - 1 > Me.yLength) Then

                Throw New ArgumentException("Failed to validate y-coordinate. The value given is bigger than the y-Length.")

            End If

        End Sub

#End Region

    End Class

End Namespace