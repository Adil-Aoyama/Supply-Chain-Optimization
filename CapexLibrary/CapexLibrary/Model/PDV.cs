using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapexLibrary.Model
{
    public class PDV
    {
        private Dictionary<string,Product> productsToProduce;

        public PDV()
        {
            productsToProduce = new Dictionary<string, Product>();
        }
        public Dictionary<string, Product> ProductsToProduce
        {
            get { return productsToProduce; }
            set { productsToProduce = value; }
        }

        public void addProduct(string type, string name,string unity,double quantity)
        {
            productsToProduce[name] = new Product(type, name, unity, quantity);
        }


    }
}
