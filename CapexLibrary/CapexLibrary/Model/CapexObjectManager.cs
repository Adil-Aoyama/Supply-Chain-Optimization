using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapexLibrary.Model
{
    public class CapexObjectManager
    {
        private string year;
        private PDV pdv;
        private Dictionary<string,Axe> axesDico;
        private Dictionary<string,Connexion> connexionsDico;
        private Dictionary<string, Unit> laveriesDico;
        private Dictionary<string, Unit> linesDico;
        private CapexSolverManager solverManager;

        public CapexObjectManager(string year)
        {
            this.year = year;
            pdv = new PDV();
            axesDico = new Dictionary<string, Axe>();
            connexionsDico = new Dictionary<string,Connexion>();
            laveriesDico = new Dictionary<string, Unit>();
            linesDico = new Dictionary<string, Unit>();
        }
        public Dictionary<string, Unit> LinesDico
        {
            get { return linesDico; }
        }
        public PDV Pdv
        {
            get { return pdv; }
        }
        public string Year
        {
            get { return year; }
        }
        public Dictionary<string, Axe> AxesDico
        {
            get { return axesDico; }
        }
        public void GenerateFinalProducts(object[,] pdvInput, int numCol)
        {
            for(int j=1;j<pdvInput.GetLength(0);j++)
            {
                if (pdvInput[j, numCol] == null) pdvInput[j, numCol] = 0.0;
                pdv.addProduct((string)pdvInput[j, 0], (string)pdvInput[j, 1], (string)pdvInput[j, 2], (double)pdvInput[j, numCol]);
            }
        }
        public Axe GetOrCreateAxe(string axeName)
        {
            if (!axesDico.ContainsKey(axeName))
                axesDico[axeName] = new Axe(axeName);
            return axesDico[axeName];
        }

        public Entity GetEntity(string axeName, string entityName)
        {
            Axe axe = GetOrCreateAxe(axeName);
            return axe.GetEntity(entityName);
        }
        public Unit GetUnit(string axeName, string entityName, string unitName)
        {
            Entity entity = GetEntity(axeName, entityName);
            if(entity==null)
                return null;
            return entity.GetUnit(unitName);
        }
        
        public Connexion GetConnexion(string connexionName)
        {
            if (connexionsDico.ContainsKey(connexionName))
                return connexionsDico[connexionName];
            return null;
        }

        public void AddConnexion(string connexionName, Unit origin, Unit destination,object connexionCost, double capacity, Product product)
        {
            if (connexionsDico.ContainsKey(connexionName))
                return;
            Connexion connexion = new Connexion(connexionName, origin, destination, connexionCost, capacity, product);
            connexionsDico[connexionName] = connexion;
        }

        public void UpdateLaveriesDico()
        {
            if (laveriesDico.Count > 0)
                return;
            foreach(Axe axe in axesDico.Values)
            {
                foreach(Entity entity in axe.Entities)
                {
                    foreach(Unit unit in entity.Units)
                    {
                        if(unit is Treatment && ((Treatment)unit).TreatmentType == TreatmentType.LAVERIE)
                        {
                            laveriesDico[unit.UnitName] = unit;
                        }
                    }
                }
            }
        }
        public Unit GetLaverie(string laverieName)
        {
            if(laveriesDico.ContainsKey(laverieName))
                return laveriesDico[laverieName];
            return null;
        }

        public void UpdateLinesDico()
        {
            if (linesDico.Count > 0)
                return;
            foreach (Axe axe in axesDico.Values)
            {
                foreach (Entity entity in axe.Entities)
                {
                    foreach (Unit unit in entity.Units)
                    {
                        if (unit is Treatment && ((Treatment)unit).TreatmentType == TreatmentType.FACILITY_LINE)
                        {
                            linesDico[unit.UnitName] = unit;
                        }
                    }
                }
            }
        }

        public Unit GetLine(string facilityLineName)
        {
            if (linesDico.ContainsKey(facilityLineName))
                return linesDico[facilityLineName];
            return null;
        }

        public object[,] LaunchSolver(string solverName)
        {
            solverManager = new CapexSolverManager(solverName);
            solverManager.TransferData(this);
            return solverManager.Solve();
        }
    } 
}
