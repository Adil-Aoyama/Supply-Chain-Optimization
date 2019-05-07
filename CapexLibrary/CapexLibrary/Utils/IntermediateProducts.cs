using CapexLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapexLibrary.Utils
{
    public static class IntermediateProducts
    {
        private static Dictionary<string, Product> InterProducts;

        static IntermediateProducts()
        {
            InterProducts = new Dictionary<string, Product>();
            InitIntermediateProducts();
        }

        public static Product GetInterProduct(string productName)
        {
            if (InterProducts.ContainsKey(productName))
                return InterProducts[productName];
            return null;
        }

        private static void InitIntermediateProducts()
        {
            Product tbt = new Product(Legende.PRODUCT_ROCHE, Legende.TBT, Legende.ROCHEUNITY, 0);
            InterProducts[tbt.ProductName] = tbt;

            Product bt = new Product(Legende.PRODUCT_ROCHE, Legende.BT, Legende.ROCHEUNITY, 0);
            InterProducts[bt.ProductName] = bt;

            Product ht = new Product(Legende.PRODUCT_ROCHE, Legende.HT, Legende.ROCHEUNITY, 0);
            InterProducts[ht.ProductName] = ht;

            Product sechage = new Product(Legende.PRODUCT_ROCHE, Legende.SECHAGE, Legende.ROCHEUNITY, 0);
            InterProducts[sechage.ProductName] = sechage;

            Product acs = new Product(Legende.PRODUCT_ACIDE, Legende.ACS, Legende.ACSUNITY, 0);
            InterProducts[acs.ProductName] = acs;
        }
    }
}
