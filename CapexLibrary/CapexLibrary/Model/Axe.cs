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
    public class Axe
    {
        private string axeName;
        private Dictionary<string, Entity> entitiesDico;

        public Axe(string axeName)
        {
            this.axeName = axeName;
            entitiesDico = new Dictionary<string, Entity>();
        }
        public string AxeName
        {
            get { return axeName; }
        }
        public Dictionary<string, Entity> EntitiesDico
        {
            get { return entitiesDico; }
        }

        public void AddMine(string mineName)
        {
            if (entitiesDico.ContainsKey(mineName))
                return;
            Mine mine =  new Mine(mineName);
            entitiesDico[mineName] = mine;
            mine.AddExtraction(mineName + Legende.EXTRACTION);
        }

        public void AddFacility(string facilityName)
        {
            if (!entitiesDico.ContainsKey(facilityName))
                entitiesDico[facilityName] = new Facility(facilityName);
        }

        public void AddLogistics(string logisticsName)
        {
            if (!entitiesDico.ContainsKey(logisticsName))
                entitiesDico[logisticsName] = new Logistics(logisticsName);
        }
        public void AddPort(string portName)
        {
            if (!entitiesDico.ContainsKey(portName))
                entitiesDico[portName] = new Port(portName);
        }
        public Entity GetEntity(string entityName)
        {
            if (!entitiesDico.ContainsKey(entityName))
                return null;
            return entitiesDico[entityName];
        }

        public List<Entity> Entities
        {
            get { return entitiesDico.Values.ToList(); }
        }
    }
}
