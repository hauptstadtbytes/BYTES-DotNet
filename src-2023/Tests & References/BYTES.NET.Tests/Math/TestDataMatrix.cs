//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//import namespace(s) required from 'BYTES.NET.Math' framework
using BYTES.NET.Math;

namespace BYTES.NET.Tests.Math
{
    [TestClass]
    public class TestDataMatrix
    {
        [TestMethod]
        public void TestMatrix()
        {
            //get a new integer data matrix
            DataMatrix<int> matrxOne = GetIntMatrix();

            //check the matrix dimensions
            Assert.AreEqual(10, matrxOne.XLength);
            Assert.AreEqual(11, matrxOne.YLength);

            //check the minimum and maximum
            Assert.AreEqual(1, matrxOne[1, 1]);
            Assert.AreEqual(10, matrxOne[10, 1]);
            Assert.AreEqual(110, matrxOne[10, 11]);

            //create a new instance from another instance's data
            DataMatrix<int> matrxTwo = new DataMatrix<int>(matrxOne.ToArray());

            //check the matrix dimensions
            Assert.AreEqual(10, matrxTwo.XLength);
            Assert.AreEqual(11, matrxTwo.YLength);

            //check the minimum and maximum
            Assert.AreEqual(1, matrxTwo[1, 1]);
            Assert.AreEqual(10, matrxTwo[10, 1]);
            Assert.AreEqual(110, matrxTwo[10, 11]);

        }

        [TestMethod]
        public void TestStatistics()
        {
            //get a new integer data matrix
            DataMatrix<int> matrxOne = GetIntMatrix();

            //check the matrix minimum and maximum
            Assert.AreEqual(1, matrxOne.Minimum());
            Assert.AreEqual(110, matrxOne.Maximum());

            //check the distribution
            SortedDictionary<int,int> dist = matrxOne.Distribution();
            Assert.AreEqual(1, dist[1]);
            Assert.AreEqual(2, dist[2]);

        }

        private DataMatrix<int> GetIntMatrix()
        {
            //create a new data matrix
            DataMatrix<int> output = new DataMatrix<int>(10, 11);

            //insert values
            for (int y = 1; y <= 11; y++)
            {
                for (int x = 1; x <= 10; x++)
                {
                    output[x, y] = x * y;
                }
            }

            return output;
        }

    }
}
