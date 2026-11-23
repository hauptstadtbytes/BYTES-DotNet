using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Math.Matrix
{
    public class MatrixData<TValue>
    {
        #region protected properties

        protected TValue[,] _values = null;

        #endregion


        #region public properties

        public int xLength { get => _values.GetLength(0); }

        public int yLength { get => _values.GetLength(1); }

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


        #region constructor

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="values"></param>
        public MatrixData(TValue[,] values)
        {
            _values = values;
        }

        /// <summary>
        /// overloaded new instance method
        /// </summary>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        public MatrixData(int xLength, int yLength)
        {
            _values = new TValue[xLength, yLength];
        }

        #endregion


        #region private methods

        /// <summary>
        /// method called after value update
        /// </summary>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnValueUpdate(int xCoordinate, int yCoordinate, TValue oldValue, TValue newValue)
        {
        }


        /// <summary>
        /// method validating coordinates given
        /// </summary>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        private void ValidateCoordinates(int xCoordinate, int yCoordinate)
        {
            if (xCoordinate < 1)
                throw new ArgumentException("Failed to validate x-coordinate. All coordinates have to be 1-based.");

            if (xCoordinate > this.xLength)
                throw new ArgumentException("Failed to validate x-coordinate. The value given is bigger than the x-Length.");

            if (yCoordinate < 1)
                throw new ArgumentException("Failed to validate y-coordinate. All coordinates have to be 1-based.");

            if (yCoordinate > this.yLength)
                throw new ArgumentException("Failed to validate y-coordinate. The value given is bigger than the y-Length.");
        }

        #endregion
    }
}