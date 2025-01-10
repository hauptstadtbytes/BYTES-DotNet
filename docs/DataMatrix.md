# DataMatrix\<TValue\>

The `DataMatrix<TValue>` class represents a two-dimensional matrix of generic values. It provides methods and properties for managing and accessing data in a matrix format.

## Namespace

```csharp
using BYTES.NET.Math;
```

## Summary

This class allows you to create, manipulate, and retrieve values from a two-dimensional data structure using 1-based indexing.

## Constructors

### `DataMatrix(TValue[,] values)`

Creates a new instance of `DataMatrix` with the provided matrix values.

**Parameters:**
- `values` (TValue[,]): A two-dimensional array of values.

### `DataMatrix(int xLength, int yLength)`

Creates a new instance of `DataMatrix` with the specified dimensions.

**Parameters:**
- `xLength` (int): The number of rows.
- `yLength` (int): The number of columns.

## Properties

### `int XLength`

Gets the number of rows in the matrix.

### `int YLength`

Gets the number of columns in the matrix.

### `TValue this[int xCoordinate, int yCoordinate]`

Provides access to the value at the specified 1-based coordinates.

**Parameters:**
- `xCoordinate` (int): The 1-based row index.
- `yCoordinate` (int): The 1-based column index.

**Returns:** `TValue`

**Remarks:** Coordinates are 1-based.

## Methods

### `TValue[,] ToArray()`

Returns the underlying two-dimensional array.

**Returns:** `TValue[,]`

### Protected Methods

#### `void OnValueUpdate(int xCoordinate, int yCoordinate, TValue oldValue, TValue newValue)`

A virtual method called after a value in the matrix is updated. Can be overridden in derived classes.

**Parameters:**
- `xCoordinate` (int): The 1-based row index.
- `yCoordinate` (int): The 1-based column index.
- `oldValue` (TValue): The previous value at the specified coordinates.
- `newValue` (TValue): The new value at the specified coordinates.

### Private Methods

#### `void ValidateCoordinates(int xCoordinate, int yCoordinate)`

Validates that the provided coordinates are within the valid range.

**Parameters:**
- `xCoordinate` (int): The 1-based row index.
- `yCoordinate` (int): The 1-based column index.

**Exceptions:**
- Throws `ArgumentException` if the coordinates are out of bounds.

## Remarks

- All coordinates used in this class are **1-based**.
- This class is designed to handle generic types, making it flexible for various use cases.
