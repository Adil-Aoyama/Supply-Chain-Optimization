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
    public abstract class Unit
    {
        private string unitName;
        private double capacity;
        private double cost;
        private string productItProduces;
        private double rockConsumption;
        private double acsConsumption;
        private double acpConsumption;
        private double acp54Consumption;
        private double nh3Consumption;
        private double sConsumption;


        public Unit(string unitName)
        {
            this.unitName = unitName;
            capacity = 0.0;
            cost = 0.0;
            rockConsumption = 0.0;
            acsConsumption = 0.0;
            acpConsumption = 0.0;
            acp54Consumption = 0.0;
            nh3Consumption = 0.0;
            sConsumption = 0.0;

        }

        public string UnitName
        {
            get { return unitName;}
        }

        public double Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }
        public double Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        public double RockConsumption
        {
            get { return rockConsumption; }
            set { rockConsumption = value; }
        }
        public double ACSConsumption
        {
            get { return acsConsumption; }
            set { acsConsumption = value; }
        }
        public double ACPConsumption
        {
            get { return acpConsumption; }
            set { acpConsumption = value; }
        }
        public double ACP54Consumption
        {
            get { return acp54Consumption; }
            set { acp54Consumption = value; }
        }
        public double NH3Consumption
        {
            get { return nh3Consumption; }
            set { nh3Consumption = value; }
        }
        public double SConsumption
        {
            get { return sConsumption; }
            set { sConsumption = value; }
        }
        public string ProductItProduces 
        {
            get { return productItProduces; }
            set { productItProduces = value; }
        }
    }
}
