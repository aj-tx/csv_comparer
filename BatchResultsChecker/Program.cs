using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchResultsChecker
{
    class Program
    {
        static void Main(string[] args)
        {

            string folderName = @"C:\Users\aalazzawi\Documents\portfolios\";

            string goldenDataSet = "";
            string resultDataSet = "";



             goldenDataSet = @"c:\\porfolio_upload.csv";

           // goldenDataSet = folderName + "goldenTable.CSV";
            var goldenDataTable = CSVHandlercs.ConvertCSVtoDataTable(goldenDataSet);

            

            //"C:\Users\aalazzawi\Documents\porfolio_upload - modified.csv"


             resultDataSet = @"C:\Users\aalazzawi\Documents\porfolio_upload.csv";
           // resultDataSet = folderName + "destTable.csv";
            var resultDataTable = CSVHandlercs.ConvertCSVtoDataTable(resultDataSet);


             var comparisonResultTable = CSVHandlercs.AreTablesTheSame(goldenDataTable, resultDataTable,"LON");


            if (comparisonResultTable != null)
            {
                Console.WriteLine(string.Format("Found mismatching rows {0} ", comparisonResultTable.Rows.Count));
                if (comparisonResultTable.Rows.Count > 0)
                {

                    Console.WriteLine(" \n\ndo you want the results in CSV format ?");
                    Console.WriteLine(" (1) YES, (2) NO");

                    try
                    {
                        int response = int.Parse(Console.ReadLine());
                        if (response == 1)
                            CSVHandlercs.ConvertToCSV(comparisonResultTable);
                    }

                    catch {

                        //user entered bs values
                        Console.WriteLine("you are an idiot , Terminating your ass ! \n\n Shutdown In progress..........");

                    }
                }
            }
            //var comparisonTable = resultDataTable.Copy();
            //comparisonTable.Rows.Clear();

       

            Console.ReadLine();

        }
    }
}
