using BYTES.NET.Math.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Math.Statistics
{
    public class MatrixStatistics<TValue> : MatrixData<TValue>
    {
        #region private properties

        private TValue _minValue = default;
        private TValue _maxValue = default;
        private Dictionary<TValue, int> _distribution = new Dictionary<TValue, int>();

        #endregion


        #region public properties

        public TValue Maximum { get => _maxValue; }
        public TValue Minimum { get => _minValue; }
        public Dictionary<TValue, int> Distribution { get => _distribution; }

        #endregion


        #region constructors

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="values"></param>
        public MatrixStatistics(TValue[,] values) : base(values)
        {
            UpdateStatistics();
        }

        /// <summary>
        /// overloaded new instance method
        /// </summary>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        public MatrixStatistics(int xLength, int yLength): base(xLength, yLength)
        {
            UpdateStatistics();
        }

        #endregion


        #region protected methods

        /// <summary>
        /// method called after value update
        /// </summary>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void OnValueUpdate(int xCoordinate, int yCoordinate, TValue oldValue, TValue newValue)
        {
            UpdateStatistics(oldValue, newValue);
        }

        #endregion


        #region private methods

        /// <summary>
        /// method updating the statistics (initially)
        /// </summary>
        private void UpdateStatistics()
        {
            List<TValue> minCache = new List<TValue>();
            List<TValue> maxCache = new List<TValue>();
            List<Dictionary<TValue, int>> distributionCache = new List<Dictionary<TValue, int>>();

            for (int col = 0; col <= this.xLength - 1; col += 1) // loop for each column
            {
                TValue currMinValue = default(TValue);
                TValue currMaxValue = default(TValue);

                Dictionary<TValue, int> currDistribution = new Dictionary<TValue, int>();

                if (this.yLength >= 1)
                {
                    for (int row = 0; row <= this.yLength - 1; row += 1) // loop for each row
                    {
                        if (!currDistribution.ContainsKey(_values[col, row]))
                        {
                            currDistribution.Add(_values[col, row], 1);
                        }  
                        else
                        {
                            int count = currDistribution[_values[col, row]] + 1;
                            currDistribution[_values[col, row]] = count;
                        }


                        if (currMinValue == null)
                        {
                            currMinValue = _values[col, row];
                        }  
                        else if (IsLess(_values[col, row], currMinValue))
                        {
                            currMinValue = _values[col, row];
                        }
                           
                        
                        if (currMaxValue == null)
                        {
                            currMaxValue = _values[col, row];
                        } 
                        else if (IsMore(_values[col, row], currMaxValue))
                        {
                            currMaxValue = _values[col, row];
                        }
                    }
                }
                minCache.Add(currMinValue);
                maxCache.Add(currMaxValue);
                distributionCache.Add(currDistribution);
            }

            foreach (TValue singleValue in minCache)
            {
                if (_minValue == null)
                {
                    _minValue = singleValue;
                }
                else if (IsLess(singleValue, _minValue))
                {
                    _minValue = singleValue;
                }
            }

            foreach (TValue singleValue in maxCache)
            {
                if (_maxValue == null)
                {
                    _maxValue = singleValue;
                }
                else if (IsMore(singleValue, _maxValue))
                {
                    _maxValue = singleValue;
                }
            }

            Dictionary<TValue, int> dist = null;

            foreach (Dictionary<TValue, int> singleValue in distributionCache)
            {
                if (dist == null)
                {
                    if (distributionCache.Count > 0)
                    {
                        dist = singleValue;
                    }
                }
                else
                    foreach (KeyValuePair<TValue, int> pair in singleValue)
                    {
                        if (!dist.ContainsKey(pair.Key))
                        {
                            dist.Add(pair.Key, pair.Value);
                        }
                        else
                        {
                            int count = dist[pair.Key] + pair.Value;
                            dist[pair.Key] = count;
                        }
                    }
            }

            _distribution = dist;
        }

        /// <summary>
        /// method returning a boolean, indicating if a value is bigger than a reference value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        /// <remarks>https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns</remarks>
        private bool IsMore(TValue value, TValue reference)
        {
            return (value, reference) switch
            {
                (int v, int r) => v > r,
                (double v, double r) => v > r,
                (string v, string r) => v.Length > r.Length,
                _ => throw new NotSupportedException($"Unable to compare value(s) of type '{typeof(TValue)}'")
            };
        }

        /// <summary>
        /// method returning a boolean, indicating if a value is smaller than a reference value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        private bool IsLess(TValue value, TValue reference)
        {
            return (value, reference) switch
            {
                (int v, int r) => v < r,
                (double v, double r) => v < r,
                (string v, string r) => v.Length < r.Length,
                _ => throw new NotSupportedException($"Unable to compare value(s) of type '{typeof(TValue)}'")
            };
        }

        /// <summary>
        /// overloading method updating the statistics (on single value change)
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void UpdateStatistics(TValue oldValue, TValue newValue)
        {
            if (!_distribution.Keys.Contains(newValue))
            {
                _distribution.Add(newValue, 1);
            }
            else
            {
                int currValue = _distribution[newValue];
                _distribution[newValue] = currValue + 1;
            }


            if (_distribution.Keys.Contains(oldValue))
            {
                int currValue = _distribution[oldValue];

                if ((currValue - 1) <= 0)
                {
                    _distribution.Remove(oldValue);
                }
                else
                {
                    _distribution[oldValue] = currValue - 1;
                }
            }


            if (IsMore(newValue, _maxValue))
            {
                _maxValue = newValue;
            }
               

            if (IsLess(newValue, _minValue))
            {
                _minValue = newValue;
            }     
        }

        #endregion
    }
}