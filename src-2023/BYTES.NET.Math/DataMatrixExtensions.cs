//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Math
{
    public static class DataMatrixExtensions
    {
        #region integer type matrix extensions

        /// <summary>
        /// returns the minimum value of a data matrix
        /// </summary>
        /// <param name="mtrx"></param>
        /// <returns></returns>
        public static int? Minimum(this DataMatrix<int> mtrx)
        {
            int[,] mtrxData = mtrx.ToArray();

            int? output = null;

            for (int y = 0; y <= mtrx.YLength-1; y++)
            {
                for (int x = 0; x <= mtrx.XLength - 1; x++)
                {
                    int data = mtrxData[x, y];

                    if (output == null)
                    {
                        output = data;
                    }

                    if(output > data)
                    {
                        output = data;
                    }
                }
            }

            return output;

        }

        /// <summary>
        /// returns the maximum value of a data matrix
        /// </summary>
        /// <param name="mtrx"></param>
        /// <returns></returns>
        public static int? Maximum(this DataMatrix<int> mtrx)
        {
            int[,] mtrxData = mtrx.ToArray();

            int? output = null;

            for (int y = 0; y <= mtrx.YLength - 1; y++)
            {
                for (int x = 0; x <= mtrx.XLength - 1; x++)
                {
                    int data = mtrxData[x, y];

                    if (output == null)
                    {
                        output = data;
                    }

                    if (output < data)
                    {
                        output = data;
                    }
                }
            }

            return output;

        }

        /// <summary>
        /// returns the value value distribution for a data matrix
        /// </summary>
        /// <param name="mtrx"></param>
        /// <returns></returns>
        public static SortedDictionary<int, int> Distribution(this DataMatrix<int> mtrx)
        {
            int[,] mtrxData = mtrx.ToArray();

            SortedDictionary<int,int> output = new SortedDictionary<int, int>();

            for (int y = 0; y <= mtrx.YLength - 1; y++)
            {
                for (int x = 0; x <= mtrx.XLength - 1; x++)
                {
                    int data = mtrxData[x, y];
                    if (!output.ContainsKey(data)){
                        output[data] = 1;
                    } else
                    {
                        output[data]++;
                    }

                }
            }

            return output;

        }

        #endregion

        #region double type matrix extensions

        /// <summary>
        /// returns the minimum value of a data matrix
        /// </summary>
        /// <param name="mtrx"></param>
        /// <returns></returns>
        public static double? Minimum(this DataMatrix<double> mtrx)
        {
            double[,] mtrxData = mtrx.ToArray();

            double? output = null;

            for (int y = 0; y <= mtrx.YLength - 1; y++)
            {
                for (int x = 0; x <= mtrx.XLength - 1; x++)
                {
                    double data = mtrxData[x, y];

                    if (output == null)
                    {
                        output = data;
                    }

                    if (output > data)
                    {
                        output = data;
                    }
                }
            }

            return output;

        }

        /// <summary>
        /// returns the maximum value of a data matrix
        /// </summary>
        /// <param name="mtrx"></param>
        /// <returns></returns>
        public static double? Maximum(this DataMatrix<double> mtrx)
        {
            double[,] mtrxData = mtrx.ToArray();

            double? output = null;

            for (int y = 0; y <= mtrx.YLength - 1; y++)
            {
                for (int x = 0; x <= mtrx.XLength - 1; x++)
                {
                    double data = mtrxData[x, y];

                    if (output == null)
                    {
                        output = data;
                    }

                    if (output < data)
                    {
                        output = data;
                    }
                }
            }

            return output;

        }

        /// <summary>
        /// returns the value value distribution for a data matrix
        /// </summary>
        /// <param name="mtrx"></param>
        /// <returns></returns>
        public static SortedDictionary<double, int> Distribution(this DataMatrix<double> mtrx)
        {
            double[,] mtrxData = mtrx.ToArray();

            SortedDictionary<double, int> output = new SortedDictionary<double, int>();

            for (int y = 0; y <= mtrx.YLength - 1; y++)
            {
                for (int x = 0; x <= mtrx.XLength - 1; x++)
                {
                    double data = mtrxData[x, y];
                    if (!output.ContainsKey(data))
                    {
                        output[data] = 1;
                    }
                    else
                    {
                        output[data]++;
                    }

                }
            }

            return output;

        }

        #endregion

    }
}
