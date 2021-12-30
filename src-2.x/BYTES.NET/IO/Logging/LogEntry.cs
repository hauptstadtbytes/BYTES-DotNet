//import .net namespce(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.Primitives.Extensions;

namespace BYTES.NET.IO.Logging
{
    public class LogEntry : EventArgs
    {
        #region public variable(s)

        public enum InformationLevel
        {
            Debug = 0,
            Info = 1,
            Warning = 2,
            Exception = 3,
            Fatal = 4
        }

        #endregion

        #region protected vairable(s)

        protected DateTime _timeStamp = DateTime.Now;
        protected InformationLevel _level = InformationLevel.Info;

        protected string _message = string.Empty;
        protected object _details = null;

        #endregion

        #region public properties

        public DateTime TimeStamp
        {
            get
            {
                return _timeStamp;
            }
        }

        public InformationLevel Level
        {
            get
            {
                return _level;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public object Details
        {
            get
            {
                return _details;
            }
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public LogEntry(string message, InformationLevel level)
        {
            _timeStamp = DateTime.Now;
            _message = message;
            _level = level;
        }

        /// <summary>
        /// (overloaded) new instance method supporting additional details
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="details"></param>
        public LogEntry(string message, InformationLevel level, object details)
        {
            _timeStamp = DateTime.Now;
            _message = message;
            _level = level;
            _details = details;
        }

        /// <summary>
        /// (overloaded) new instance method, supporting a message string only
        /// </summary>
        /// <param name="message"></param>
        /// <remarks>information level 'Info' will be used</remarks>
        public LogEntry(string message)
        {
            _timeStamp = DateTime.Now;
            _message = message;
        }

        #endregion

        #region public method(s)

        public bool MatchesThreshold(LogEntry.InformationLevel threshold)
        {
            //return always 'true' on debug level thresholding
            if (threshold == InformationLevel.Debug)
            {
                return true;
            }

            //compare the local information level with the reshold
            int threshVal = (int)Enum.Parse(typeof(LogEntry.InformationLevel), threshold.ToString());
            int entryVal = (int)Enum.Parse(typeof(LogEntry.InformationLevel), _level.ToString());

            if (entryVal >= threshVal)
            {
                return true;
            }

            return false;

        }

        /// <summary>
        /// returns a string equivalent of the 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public string ToString(string pattern = "%TimeStamp% %Level% [] - %Message%")
        {

            //create the variables dictionary
            Dictionary<string, string> variables = new Dictionary<string, string>();
            variables.Add("%TimeStamp%", this.TimeStamp.ToString());
            variables.Add("%Level%", this.Level.ToString());
            variables.Add("%Message%", this.Message);

            if (this.Details != null)
            {

                if(this.Details.GetType() == typeof(Exception)){

                    Exception exeption = (Exception)this.Details;

                    variables.Add("%Exception%", exeption.Message);
                    variables.Add("%ExceptionStack%", exeption.StackTrace);

                } else if(this.Details.GetType() == typeof(string))
                {

                    variables.Add("%Details%", this.Details.ToString());

                }

            }
            else
            {
                variables.Add("%Exception%", "");
                variables.Add("%ExceptionStack%", "");
                variables.Add("%Details%", "");
            }

            //return the output value
            return pattern.Expand(variables);

        }

        #endregion
    }
}
