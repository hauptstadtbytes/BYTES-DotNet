//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

//import namespace(s) required from 'BYTES.NET' library
using BYTES.NET.Math;

namespace BYTES.NET.TEST.Math
{

    [TestClass]
    public class TestMatrixStatistics
    {

        private MatrixStatistics<int> mtrx1 = new MatrixStatistics<int>(10, 10);

        [TestMethod]
        public void GetMatrixStatistics()
        {

            //add the values
            mtrx1 = new MatrixStatistics<int>(10, 10);

            for (int i = 1; i <= 10; i++)
            {

                for (int k = 1; k <= 10; k++)
                {

                    mtrx1.set_Value(i, k, ((i-1) * (k-1))); //the C# equivalent for using a property with parameters

                }

            }

            //validate the result
            Assert.AreEqual(mtrx1.Maximum, 81); //validate the maximum value
            Assert.AreEqual(mtrx1.Minimum, 0); //validate the minimum value

        }

    }
}
