using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;
using CapexLibrary.Utils;
using CapexLibrary.Model;


namespace CapexLibrary.Controler
{
    public static class ExposedFunctions
    {

        public static Object[,] result = null;
        public static object[,] outputResult;
        public static CapexControler capexControler;
        public static Excel.Worksheet wrkSheet;

        [ExcelFunction(Name = "CAPEXLoadConfig", Description = "Chargement de la configuration")]
        public static bool CAPEXLoadConfig(Object[,] config)
        {
            result = config;
            capexControler = new CapexControler(result);
            return true;
        }
        
        /*[ExcelFunction(Name = "CAPEXShowMeData", Description = "TestLoading")]
        public static object[,] CAPEXShowMeData(string year)
        {
            CapexObjectManager objManager = capexControler.ObjectManagerDico[year];
            Dictionary<string, Unit> linesDico = objManager.LinesDico;
            int nbrVar = linesDico.Values.Count;
            outputResult = new object[nbrVar + 1, 9];
            outputResult[0, 0] = "Line Name";
            outputResult[0, 1] = "Line Capacity";
            outputResult[0, 2] = "Line Cost";
            outputResult[0, 3] = "productItProduces";
            outputResult[0, 4] = "ROCK Consumption";
            outputResult[0, 5] = "ACS Consumption";
            outputResult[0, 6] = "ACP Consumption";
            outputResult[0, 7] = "NH3 Consumption";
            outputResult[0, 8] = "S Consumption";

            for (int i = 0; i < linesDico.Values.Count; i++)
            {
                outputResult[i + 1, 0] = linesDico.Values.ElementAt(i).UnitName;
                outputResult[i + 1, 1] = linesDico.Values.ElementAt(i).Capacity;
                outputResult[i + 1, 2] = linesDico.Values.ElementAt(i).Cost;
                outputResult[i + 1, 3] = linesDico.Values.ElementAt(i).ProductItProduces;
                outputResult[i + 1, 4] = linesDico.Values.ElementAt(i).RockConsumption;
                outputResult[i + 1, 5] = linesDico.Values.ElementAt(i).ACSConsumption;
                outputResult[i + 1, 6] = linesDico.Values.ElementAt(i).ACPConsumption;
                outputResult[i + 1, 7] = linesDico.Values.ElementAt(i).NH3Consumption;
                outputResult[i + 1, 8] = linesDico.Values.ElementAt(i).SConsumption;
            }
            return outputResult;
        }
        */
        [ExcelFunction(Name = "CAPEXFacilitiesAllocationPerYear", Description = "Problem Solving")]
        public static object[,] CAPEXFacilitiesAllocationPerYear(string solverName, string year)
        {
            CapexObjectManager objManager = capexControler.ObjectManagerDico[year];
            outputResult = objManager.LaunchSolver(solverName);
            return outputResult;
        }
        
        [ExcelFunction(Name = "CAPEXFacilitiesAllocation", Description = "Problem Solving")]
        public static object[,] CAPEXFacilitiesAllocation(string solverName)
        {
            object[,] finalResult;
            CapexObjectManager objManager = capexControler.ObjectManagerDico.Values.ElementAt(0);
            finalResult = objManager.LaunchSolver(solverName);
            outputResult = new object[finalResult.GetLength(0), capexControler.ObjectManagerDico.Values.Count + 1];

            for (int j = 0; j < finalResult.GetLength(0); j++)
            {
                outputResult[j, 0] = finalResult[j, 0];
                outputResult[j, 1] = finalResult[j, 1];
            }

            for (int k = 1; k < capexControler.ObjectManagerDico.Values.Count; k++)
            {
                object[,] result = capexControler.ObjectManagerDico.Values.ElementAt(k).LaunchSolver(solverName);
                for (int i = 0; i < finalResult.GetLength(0); i++)
                {
                    outputResult[i, k + 1] = result[i, 1];
                }
            }
            return outputResult;
        }

    }
}
