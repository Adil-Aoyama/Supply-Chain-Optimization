using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDna.Integration;
using Excel = Microsoft.Office.Interop.Excel;

namespace CapexLibrary.Model
{
    public abstract class Entity
    {
        private string entityName;
        private double capacity;
        private Dictionary<string, Unit> unitsDico;
        
        public Entity(string entityName)
        {
            this.entityName = entityName;
            unitsDico = new Dictionary<string, Unit>();
        }
        public string EntityName
        {
            get { return entityName; }
        }
        public double Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }
        public void AddInventory(string inventoryname)
        {
            if (!unitsDico.ContainsKey(inventoryname))
                unitsDico[inventoryname] = new Inventory(inventoryname);
        }
        public void AddTreatment(string treatmentName,TreatmentType treatmentType)
        {
            if (!unitsDico.ContainsKey(treatmentName))
                unitsDico[treatmentName] = new Treatment(treatmentName, treatmentType);
        }
        public void AddExtraction(string extractionName)
        {
            if (!unitsDico.ContainsKey(extractionName))
                unitsDico[extractionName] = new Extraction(extractionName);
        }
        public void AddBlending(string blendingName)
        {
            if (!unitsDico.ContainsKey(blendingName))
                unitsDico[blendingName] = new Blending(blendingName);
        }
        public void AddDock(string dockName)
        {
            if (!unitsDico.ContainsKey(dockName))
                unitsDico[dockName] = new Dock(dockName);
        }
        public Unit GetUnit(string unitName)
        {
            if (!unitsDico.ContainsKey(unitName))
                return null;
            return unitsDico[unitName];
        }

        public List<Unit> Units
        {
            get { return unitsDico.Values.ToList(); }
        }

    }
}
