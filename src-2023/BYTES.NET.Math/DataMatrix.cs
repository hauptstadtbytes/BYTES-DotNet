using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Math
{
        public class DataMatrix<TValue>
        {
            #region Protected Variables

            protected TValue[,] _values = null;

            #endregion

            #region Public Properties

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
            /// Coordinates are 1-based.
            /// </summary>
            /// <param name="xCoordinate"></param>
            /// <param name="yCoordinate"></param>
            /// <returns></returns>
            public TValue this[int xCoordinate, int yCoordinate]
            {
                get
                {
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

            #region Constructors

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="values"></param>
            public DataMatrix(TValue[,] values)
            {
                _values = values;
            }

            /// <summary>
            /// Overloaded constructor.
            /// </summary>
            /// <param name="xLength"></param>
            /// <param name="yLength"></param>
            public DataMatrix(int xLength, int yLength)
            {
                // Initialize the values array. Dimensions are reduced by one.
                _values = new TValue[xLength, yLength];
            }

            #endregion

            #region Protected Methods

            /// <summary>
            /// Method called after a value is updated.
            /// </summary>
            /// <param name="xCoordinate"></param>
            /// <param name="yCoordinate"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected virtual void OnValueUpdate(int xCoordinate, int yCoordinate, TValue oldValue, TValue newValue)
            {
            }

            #endregion

            #region Private Methods

            /// <summary>
            /// Validates the given coordinates.
            /// </summary>
            /// <param name="xCoordinate"></param>
            /// <param name="yCoordinate"></param>
            private void ValidateCoordinates(int xCoordinate, int yCoordinate)
            {
                if (xCoordinate - 1 < 0)
                {
                    throw new ArgumentException("Failed to validate x-coordinate. All coordinates have to be 1-based.");
                }

                if (xCoordinate - 1 >= XLength)
                {
                    throw new ArgumentException("Failed to validate x-coordinate. The value given is bigger than the x-Length.");
                }

                if (yCoordinate - 1 < 0)
                {
                    throw new ArgumentException("Failed to validate y-coordinate. All coordinates have to be 1-based.");
                }

                if (yCoordinate - 1 >= YLength)
                {
                    throw new ArgumentException("Failed to validate y-coordinate. The value given is bigger than the y-Length.");
                }
            }

            #endregion
        }
    }
