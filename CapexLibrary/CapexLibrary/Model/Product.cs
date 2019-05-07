using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;


namespace CapexLibrary.Model
{
    public class Product
    {
        private string productName;
        private string productType;
        private string productUnity;
        private double productquantity;

        public Product(string type, string name, string unity, double quantity)
        {
            this.productType = type;
            this.productName = name;
            this.productUnity = unity;
            this.productquantity = quantity;
        }
        public string ProductName
        {
            get { return productName; }
        }
        public double Productquantity
        {
            get { return productquantity; }
        }

        public string ProductType 
        {
            get { return productType; } 
        }
    }
}
