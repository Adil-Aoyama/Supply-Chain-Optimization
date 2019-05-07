using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDna.Integration;
using Excel = Microsoft.Office.Interop.Excel;

namespace CapexLibrary
{
    public abstract class Legende
    {
        public static string fichierCible = @"C:\Users\rida.ahroum\Desktop\Projet_Capex\Capex_Planning_Utilities_Ports";
        public static string Config = "config";
        public static string Produit = "Produit";
        public static string TYPE = "Type";
        public static string AXE = "Axe";
        public static string PDV = "PDV";
        public static string CAPAMINE = "capaMine";
        public static string CAPAPROD = "capaProd";
        public static string CAPALAV = "capaLav";
        public static string CONNEXIONS = "connexions";
        public static string EXTRACTION = "_Extraction";
        public static string ROCKCONSO = "rockConsumption";
        public static string ACSCONSO = "acsConso";
        public static string ACPCONSO = "acpConso";
        public static string ACP54CONSO = "acp54Conso";
        public static string NH3CONSO = "nh3Conso";
        public static string SCONSO = "sConso";
        public static string COSTS = "costs";
        public static string OUTPUTS = "outputs";

        public static string PRODUCT_ROCHE = "Roche";
        public static string PRODUCT_ACIDE = "Acide";
        public static string PRODUCT_ENGRAIS = "Engrais";

        public static string TBT = "TBT";
        public static string BT = "BT";
        public static string HT = "HT";
        public static string SECHAGE = "Sechage";
        public static string ACS = "ACS";
        public static string PRODUCT_ACP = "ACP";

        public static string ROCHEUNITY = "KT SM";
        public static string ACPUNITY = "KT P2O5";
        public static string ENGRAISUNITY = "KT Produit";
        public static string ACSUNITY = "KT P2O5";
    }
}
