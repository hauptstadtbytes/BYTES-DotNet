using BYTES.NET.Math.Matrix;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;


namespace BYTES.NET.Math.Tests;

[TestClass]
public class TestMatrixData
{
    MatrixData<int> controlMatrixEmpty = new MatrixData<int>(new int[,] { { 0, 0 }, { 0, 0 } }); // creates an empty matrix with only the lengths
    MatrixData<int> controlMatrixWithValues = new MatrixData<int>(new int[,] { { 1, 2 }, { 3, 4 } });
    MatrixData<int> controlMatrixZeroStart = new MatrixData<int>(new int[,] { { 1, 2 }, { 3, 4 } }, 0);

    [TestMethod]
    public void TestBaseConstructor()
    {
        MatrixData<int> testMatrix = new MatrixData<int>(new int[,] { { 1,2 }, { 3,4 } });

        // compare the lines of the matrix
        // https://stackoverflow.com/questions/2046121/how-to-compare-two-objects-in-unit-test
        testMatrix.Should().BeEquivalentTo(controlMatrixWithValues);
    }

    [TestMethod]
    public void TestEmptyConstructor()
    {
        MatrixData<int> testMatrix = new MatrixData<int>(2,2);
        testMatrix.Should().BeEquivalentTo(controlMatrixEmpty);
    }

    [TestMethod]
    public void TestConstructorWithStartIndex()
    {
        MatrixData<int> testMatrix = new MatrixData<int>(new int[,] { { 0, 0 }, { 0, 0 } }, 0);
        Assert.AreEqual(controlMatrixZeroStart.arrayStart, testMatrix.arrayStart);
    }
}
