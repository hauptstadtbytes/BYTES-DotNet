# MatrixStatistics
Using MatrixData as a data type, we can calculate specific properties regarding the matrix. 
The class saves
- the minimum of the matrix
- the maximum of the matrix
- the distribution of the matrix, so how many times a value appears

You can create a MatrixStatistics object, and access the values, with
```csharp
MatrixStatistics<int> ms = new MatrixStatistics<int>(10, 10);

// add values to the matrix
for(int i = 1; i <= 10; i++)
{
    for(int j = 1; j <= 10; j++)
    {
        mtrx1[i, j] = (i - 1) * (j - 1);
    }
}
// access values
int min = ms.Minimum;
int max = ms.Maximum;
int dist = ms.Distribution;

```