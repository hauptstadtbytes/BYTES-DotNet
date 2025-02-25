//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Math
{

    public class DataMatrix<TValue>
    {
        #region protected variable(s)

        protected TValue[,] _values = null;

        #endregion

        #region public propertie(s)

        public int XLength
            {
                get
                {
                    return _values.GetLength(0);
                }
            }

        public int YLength
            {
                get
                {
                    return _values.GetLength(1);
                }
            }

        /// <summary>
        /// </summary>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <returns></returns>
        /// <remarks>corrdinates are 1-based</remarks>
        public TValue this[int xCoordinate, int yCoordinate]
            {
                get
                {
                    Console.WriteLine(xCoordinate + " " + yCoordinate);
                    ValidateCoordinates(xCoordinate, yCoordinate);
                    return _values[xCoordinate - 1, yCoordinate - 1];
                }
                set
                {
                    ValidateCoordinates(xCoordinate, yCoordinate);

                    TValue oldValue = _values[xCoordinate - 1, yCoordinate - 1];
                    _values[xCoordinate - 1, yCoordinate - 1] = value;

                    OnValueUpdate(xCoordinate, yCoordinate, oldValue, value);
                }
            }

        #endregion

        #region public constructor(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="values"></param>
        public DataMatrix(TValue[,] values)
        {
            _values = values;
        }

        /// <summary>
        /// overloaded constructor, accepting dimensions 
        /// </summary>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        public DataMatrix(int xLength, int yLength)
        {
            _values = new TValue[xLength, yLength];
        }

        #endregion

        #region protected method(s)

        /// <summary>
        /// method called after a value is updated
        /// </summary>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnValueUpdate(int xCoordinate, int yCoordinate, TValue oldValue, TValue newValue)
        {
        }

        #endregion

        #region public method(s)

        /// <summary>
        /// overloads the default 'toArray()' method
        /// </summary>
        /// <returns></returns>
        public TValue[,] ToArray()
        {
            return _values;
        }

        #endregion

        #region private method(s)

        /// <summary>
        /// Validates the given coordinates.
        /// </summary>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        private void ValidateCoordinates(int xCoordinate, int yCoordinate)
            {
                if (xCoordinate < 1)
                {
                    throw new ArgumentException("Failed to validate x-coordinate. All coordinates have to be 1-based." + xCoordinate + " " + yCoordinate);
                }

                if (xCoordinate > XLength)
                {
                    throw new ArgumentException("Failed to validate x-coordinate. The value '" + xCoordinate + "' given is bigger than the rurrent x-Length of '" + XLength.ToString() + "'.");
                }

                if (yCoordinate < 1)
                {
                    throw new ArgumentException("Failed to validate y-coordinate. All coordinates have to be 1-based.");
                }

                if (yCoordinate > YLength)
                {
                    throw new ArgumentException("Failed to validate y-coordinate. The value '" + yCoordinate + "' given is bigger than the current y-Length of '" + YLength.ToString() + "'.");
                }
            }

         #endregion
    }
}