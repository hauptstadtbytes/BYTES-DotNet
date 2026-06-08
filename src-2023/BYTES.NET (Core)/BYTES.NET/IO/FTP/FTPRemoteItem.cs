using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BYTES.NET.IO.FTP
{
    public class FTPRemoteItem
    {

        #region private properties

        private Dictionary<string, int> _months = new Dictionary<string, int>() { 
                { "Jan", 1 }, { "Feb", 2 }, { "Mar", 3 }, { "Apr", 4 }, 
                { "May", 5 }, { "Jun", 6 }, { "Jul", 7 }, { "Aug", 8 }, 
                { "Sep", 9 }, { "Oct", 10 }, { "Nov", 11 }, { "Dec", 12 } };

        private string _name = null;
        private string _details;
        private DateTime _modified;
        private ConnectionInfo _connection;

        #endregion


        #region public properties

        public string Name { get => _name; }
        public DateTime modified { get => _modified; }

        #endregion


        #region constructor

        public FTPRemoteItem(string details, ConnectionInfo connection)
        {
            _details = details;
            _connection = connection;

            ParseDetails();
        }

        #endregion


        #region private methods

        private void ParseDetails()
        {
            Regex pattern = new Regex(
                @".*(?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\s*(?<day>[0-9]*)\s*(?<yearTime>([0-9]|:)*)\s*(?<fileName>.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Match match = pattern.Match(_details);
            _name = match.Groups["fileName"].Value;

            int month = _months[match.Groups["month"].Value];
            int day = Convert.ToInt32(match.Groups["day"].Value);
            string yearTime = match.Groups["yearTime"].Value;

            DateTime modified;

            if (yearTime.Contains(":"))
            {
                TimeSpan time = TimeSpan.Parse(yearTime);
                modified = new DateTime(DateTime.Now.Year, month, day, time.Hours, time.Minutes, 0);
            }
            else
            {
                modified = new DateTime(Convert.ToInt32(yearTime), month, day);
            }
            _modified = modified;
        }

        #endregion
    }
}
