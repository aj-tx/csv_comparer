using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchResultsChecker
{
    public static class DataTableExtensions
    {
        public static void SetColumnsOrder(this DataTable table, params String[] columnNames)
        {
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                table.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }

        // set columns order usage 
        //table.SetColumnsOrder("Qty", "Unit", "Id");

        //or singles.

        //table.SetColumnsOrder(new string[]{"Qty", "Unit", "Id"});
    }
}
