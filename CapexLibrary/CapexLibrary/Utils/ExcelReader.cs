using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;

namespace CapexLibrary.Utils
{
    public class ExcelReader
    {
        public static Excel.Range Range;

        public static Object[,] LoadSheet(Excel.Workbook WrkBook, string SheetName, int FirstRow)
        {
            Object[,] result = null;

            foreach (Excel.Worksheet sheet in WrkBook.Worksheets)
            {
                if (sheet.Name != SheetName)
                    continue;

                int nbrRows = FirstRow, nbrCols = 1;
                while (sheet.Cells[nbrRows, nbrCols] != null && sheet.Cells[nbrRows, nbrCols].Value2 != null)
                    nbrRows++;
                while (sheet.Cells[FirstRow, nbrCols] != null && sheet.Cells[FirstRow, nbrCols].Value2 != null)
                    nbrCols++;

                result = new Object[nbrRows - FirstRow, nbrCols - 1];
                for (int i = FirstRow; i < nbrRows; i++)
                {
                    for (int j = 1; j < nbrCols; j++)
                    {
                        result[i - FirstRow, j - 1] = sheet.Cells[i, j].Value2;
                    }
                }
            }
            return result;
        }
                
        }
}
