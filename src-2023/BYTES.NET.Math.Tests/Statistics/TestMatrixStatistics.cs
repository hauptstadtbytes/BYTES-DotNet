using BYTES.NET.Math.Statistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BYTES.NET.Math.Tests;

[TestClass]
public class TestMatrixStatistics
{
    private MatrixStatistics<int> mtrx1 = new MatrixStatistics<int>(10, 10);

    [TestMethod]
    public void GetMatrixStatistics()
    {
        mtrx1 = new MatrixStatistics<int>(10, 10);
        for(int i = 1; i <= 10; i++)
        {
            for(int j = 1; j <= 10; j++)
            {
                mtrx1[i, j] = (i - 1) * (j - 1);
            }
        }

        Assert.AreEqual(mtrx1.Maximum, 81);
        Assert.AreEqual(mtrx1.Minimum, 0);
    }
}
