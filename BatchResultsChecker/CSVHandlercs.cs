using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;

namespace BatchResultsChecker
{
   public static class CSVHandlercs
    {
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }


            return dt;
        }

        public static DataTable AreTablesTheSame(DataTable goldenTable, DataTable destTable, string sortColumn)
        {

            if (goldenTable.Columns.Count != destTable.Columns.Count)
            {
                Console.WriteLine("Source and destination tables don't have matching column count");
                // column numbers dont match
                return null;
            }

            if(goldenTable.Rows.Count != destTable.Rows.Count)
            {
                Console.WriteLine("Source and destination tables don't have matching rows count");
                // column numbers dont match
                return null;
            }


            //Sort both tables to ensure accuracy if rows positions are different

            DataView dvGolden = goldenTable.DefaultView;
            dvGolden.Sort = string.Format("{0} asc", sortColumn);
            goldenTable = dvGolden.ToTable();

            DataView dvDest = destTable.DefaultView;
            dvDest.Sort = string.Format("{0} asc", sortColumn);
            destTable = dvDest.ToTable();

           

            var comparisonTable = goldenTable.Copy();
            comparisonTable.Rows.Clear();


            //rearrange columns in desttable to match those for source one.
            List<string> goldenColumns = new List<string>();


            foreach (DataColumn col in goldenTable.Columns)
            {
                goldenColumns.Add(col.ColumnName);
            }




            //re-arrange the destination table columns to match the source one for more accurate comparison
            DataTableExtensions.SetColumnsOrder(destTable, goldenColumns.ToArray());



            //print tables after sort and re-order
            PrintRows(goldenTable, destTable, goldenColumns);


            // comparison should hold accurate at this point, since we've established count and order equality of columns in both tables.


            for (int i = 0; i < goldenTable.Rows.Count; i++)
            {
                for (int c = 0; c < goldenTable.Columns.Count; c++)
                {
                    string goldenRow = goldenTable.Rows[i][c].ToString();
                    string destRow = destTable.Rows[i][c].ToString();

                    // TODO :: need to match column names before comparing to make sure we're comparing teh same column
                    //if (goldenTable.Columns[c].ColumnName.ToLower()== goldenTable.Columns[c].ColumnName.ToLower())

                    if (IsDouble(goldenRow))
                    {
                        //check againstNumeric

                        double goldenDouble = Math.Round(double.Parse(goldenRow), 2);
                        double destDouble = Math.Round(double.Parse(destRow), 2);

                        if (goldenDouble != destDouble)
                        {
                            //return the row.
                            //comparisonTable.Rows.Add(destRow);
                            comparisonTable.Rows.Add(destTable.Rows[i].ItemArray);
                            continue;
                        }


                    }
                    else
                    {

                        if (goldenRow.ToLower().Trim() != destRow.ToLower().Trim())
                        {

                            // comparisonTable.Rows.Add(destRow);
                            comparisonTable.Rows.Add(destTable.Rows[i].ItemArray);
                            continue;
                        }

                    }
                    //if (!Equals(goldenTable.Rows[i][c], destTable.Rows[i][c]))
                    //    return false;
                }
            }
            return comparisonTable;
        }


        public static void ConvertToCSV(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText("test.csv", sb.ToString());


        }


        private static void PrintRows(DataTable goldenTable, DataTable destTable, List<string> columns)
        {
            Console.WriteLine("Golden Data Table Rows after Sort \n\n");
            Console.WriteLine(string.Join(" | ", columns));
            Console.WriteLine("\n====================================");

         

            foreach (DataRow dataRow in goldenTable.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.Write(item+" | ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n====================================");

            Console.WriteLine("Destination Data Table Rows after Sort ");
            Console.WriteLine("\n====================================");
            foreach (DataRow dataRow in destTable.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.Write(item + " | ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n====================================");
        }

        private static bool IsDouble(string str)
        {
            double n=0;
            bool isNumeric = double.TryParse(str, out n);

            return isNumeric;
        }


      



    }
}
