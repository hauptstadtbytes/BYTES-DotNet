# MatrixStatistics
Using MatrixData as a data type, we can calculate specific properties regarding the matrix. 
The class saves
- the minimum of the matrix
- the maximum of the matrix
- the distribution of the matrix, so how many times a value appears

You can create a MatrixStatistics object, and access the values, with
```
MatrixStatistics ms = new MatrixStatistics<int>();

int min = ms.Minimum;
int max = ms.Maximum;
int dist = ms.Distribution;

```