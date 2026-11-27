using BYTES.NET.Math.Statistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

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

        // since its a 10x10 array, and we start with 0, there are 19 0s in the array
        Dictionary<int,int> expected = new Dictionary<int, int>
        {
            { 0, 19 },
            { 1, 1 },  { 2, 2 },  { 3, 2 },  { 4, 3 },  { 5, 2 },  { 6, 4 },
            { 7, 2 },  { 8, 4 },  { 9, 3 },  { 10, 2 }, { 12, 4 }, { 14, 2 },
            { 15, 2 }, { 16, 3 }, { 18, 4 }, { 20, 2 }, { 21, 2 }, { 24, 4 },
            { 25, 1 }, { 27, 2 }, { 28, 2 }, { 30, 2 }, { 32, 2 }, { 35, 2 },
            { 36, 3 }, { 40, 2 }, { 42, 2 }, { 45, 2 }, { 48, 2 }, { 49, 1 },
            { 54, 2 }, { 56, 2 }, { 63, 2 }, { 64, 1 }, { 72, 2 }, { 81, 1 }
        };

        CollectionAssert.AreEquivalent(mtrx1.Distribution, expected);
        
    }
}
