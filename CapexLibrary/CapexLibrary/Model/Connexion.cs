using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapexLibrary.Model
{
    public class Connexion
    {
        private string connexionName;
        private Unit origin;
        private Unit destination;
        private object connexionCost;
        private double capacity;
        private Product product;

        public Connexion(string connexionName, Unit origin, Unit destination, object connexionCost, double capacity, Product product)
        {
            this.connexionName = connexionName;
            this.origin = origin;
            this.destination = destination;
            this.connexionCost = connexionCost;
            this.capacity = capacity;
            this.product = product;
        }

        public string ConnexionName
        {
            get { return connexionName; }
        }
        public Unit Origin
        {
            get { return origin; }
        }
        public Unit Destination
        {
            get { return destination; }
        }
        public object ConnexionCost
        {
            get { return connexionCost; }
        }
        public double Capacity
        {
            get { return capacity; }
        }
        public Product Product
        {
            get { return product; }
        }
    }
}
