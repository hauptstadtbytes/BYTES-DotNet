# MatrixData
The MatrixData class is able to save 2D arrays. 
It has a constructor with two overloaded methods. Below are the three constructors and what they accomplish.

## Start index
The array of the matrix starts at the index 1. But the class can also handle a start at 0. We have given a constructor to work with the two different array starts.

# `MatrixData(TValue[,] values)`
The standard constructor. Use it like
```
MatrixData<int> testMatrix = new MatrixData<int>(new int[,] { { 1,2 }, { 3,4 } });
```

to create a 2x2 Matrix. The matrix looks like: 

[1 2]
[3 4]

The created array starts at index 1.

# `MatrixData(int xLength, int yLength)`
The first overloaded constructor. It recieves the two lengths of the matrix and creates an empty matrix.
Use it like

```
MatrixData<int> testMatrix = new MatrixData<int>(2,2);
```
to create an empty matrix:
[0 0]
[0 0]

The created array starts at index 1.

# `MatrixData(TValue[,] values, int arraystart)`
The second overloaded constructor. It recieves the values of the matrix and creates it. In addition it also recieves the start index for the array and saves it. MatrixData's methods can handle both a start index of 0 or 1. 

Use it like 
```
MatrixData<int> testMatrix = new MatrixData<int>(new int[,] { { 1,2 }, { 3,4 } }, 0);
```

This creates the matrix 

[1 2]
[3 4]

which starts at index 0.