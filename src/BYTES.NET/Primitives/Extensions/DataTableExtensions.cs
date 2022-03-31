//import .net namespace(s) required
using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives.Extensions
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// writes the table to disk file
        /// </summary>
        /// <param name="table"></param>
        /// <param name="path"></param>
        /// <param name="hasHeader"></param>
        /// <param name="delimiter"></param>
        public static void ToCSVFile(this DataTable table, string path, bool hasHeader = true, char delimiter = ';')
        {
            //parse the data
            StringBuilder data = new StringBuilder();

            if (hasHeader)
            {
                List<string> headers = new List<string>();

                foreach (DataColumn col in table.Columns)
                {
                    headers.Add(col.ColumnName);
                }

                data.AppendLine(string.Join(delimiter.ToString(), headers.ToArray()));
            }

            foreach (DataRow row in table.Rows)
            {
                List<string> values = new List<string>();

                foreach (DataColumn col in table.Columns)
                {
                    values.Add(row[col].ToString());
                }

                data.AppendLine(string.Join(delimiter.ToString(), values.ToArray()));
            }

            //write the data
            File.WriteAllText(path, data.ToString());
        }

        /// <summary>
        /// loads data from CSV file
        /// </summary>
        /// <param name="table"></param>
        /// <param name="path"></param>
        /// <param name="hasHeader"></param>
        /// <param name="delimiter"></param>
        public static void FromCSV(this DataTable table, string path, bool hasHeader = true, char delimiter = ';')
        {
            //parse the argument(s)
            if (!File.Exists(path))
            {
                throw new ArgumentException("Unable to find source file at '" + path + "'");
            }

            //clear the data table
            table.Clear();

            //read the data
            int rowCounter = 0;

            foreach (string line in File.ReadAllLines(path))
            {
                rowCounter++;
                string[] split = line.Split(delimiter);

                while (table.Columns.Count < split.Length)
                {
                    table.Columns.Add(new DataColumn());
                }

                if (rowCounter == 1 && hasHeader)
                {
                    int columnCounter = -1;

                    foreach (string name in split)
                    {
                        columnCounter++;

                        table.Columns[columnCounter].ColumnName = name;
                    }
                }
                else
                {
                    DataRow row = table.NewRow();

                    int columnCounter = -1;

                    foreach (string value in split)
                    {
                        columnCounter++;

                        row[columnCounter] = value;
                    }

                    table.Rows.Add(row);
                }
            }
        }
    }
}
