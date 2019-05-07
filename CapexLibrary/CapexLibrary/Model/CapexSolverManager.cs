using Switcher.Lib.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapexLibrary.Model
{
    public class CapexSolverManager
    {
        private int constrIndex = 0;
        private string solverName;
        private ISolverAdapter solverAdapter;
        private CapexObjectManager capexObjectManager;
        private Dictionary<string, Axe> axesDico;
        private Dictionary<string, Unit> linesDico;
        private List<String> productsNames;
        private List<Product> productsToProduce;
        private List<String> linesList;
        object solver;

        public CapexSolverManager(string solverName)
        {
            this.solverName = solverName;
            solverAdapter = AbstractSolverAdapter.GetSolverAadapter(solverName);
        }

        public void TransferData(CapexObjectManager capexObjectManager)
        {
            this.capexObjectManager = capexObjectManager;
            this.linesDico = capexObjectManager.LinesDico;
            this.axesDico = capexObjectManager.AxesDico;
            productsNames = capexObjectManager.Pdv.ProductsToProduce.Keys.ToList();
            productsToProduce = capexObjectManager.Pdv.ProductsToProduce.Values.ToList();
            linesList = linesDico.Keys.ToList();
        }

        private void AddCapacityConstraintsAndTargetObj(object[,] finalResult)
        {
            int nbrVar = linesDico.Values.Count;
            double[] coefs = new double[nbrVar];
            for (int k = 0; k < nbrVar; k++)
                coefs[k] = 0.0;
            double[] costs = new double[nbrVar];

            for (int k = 0; k < linesDico.Values.Count; k++)
            {
                Unit unit = linesDico.Values.ElementAt(k);
                solverAdapter.SetVariableName(solver, k+1, unit.UnitName);
                if (unit.ProductItProduces == Legende.ACS || unit.Capacity == 0.0)
                    costs[k] = 0.0;
                else
                {
                    costs[k] = unit.Cost;
                }
            }
            solverAdapter.SetObjFn(solver, costs);

            for (int k = 0; k < linesDico.Values.Count; k++)
            {
                Unit unit = linesDico.Values.ElementAt(k);
                finalResult[k + 1, 0] = unit.UnitName;
                coefs[k] = 1.0;
                solverAdapter.SetConstraintName(solver, k + 1, "Contrainte_Capa_" + unit.UnitName);
                solverAdapter.AddConstraint(solver, (double[])coefs.Clone(), SolverAdapterConstraintTypes.LE, unit.Capacity);
                coefs[k] = 0.0;
            }
            constrIndex = nbrVar;
        }

        private void AddPdvConstraints()
        {
            int nbrVar = linesDico.Values.Count;
            double[] var = new double[nbrVar];
            foreach (string productName in productsNames)
            {
                for (int k = 0; k < nbrVar; k++)
                    var[k] = 0.0;
                if (capexObjectManager.Pdv.ProductsToProduce[productName].ProductType != Legende.PRODUCT_ROCHE)
                {
                    for (int k = 0; k < linesDico.Values.Count; k++)
                    {
                        Unit unit = linesDico.Values.ElementAt(k);
                        if (unit.ProductItProduces == productName && unit.Capacity > 0.0)
                        {
                            var[k] = 1.0;
                        }
                    }
                    constrIndex++;

                    solverAdapter.SetConstraintName(solver, constrIndex, "Contrainte_PDV_" + productName);
                    solverAdapter.AddConstraint(solver, (double[])var.Clone(), SolverAdapterConstraintTypes.GE,
                            capexObjectManager.Pdv.ProductsToProduce[productName].Productquantity);
                }
            }
        }
        private void AddConsoACSConstraints()
        {
            int nbrVar = linesDico.Values.Count;
            double[] coefs = new double[nbrVar];

            for (int k = 0; k < axesDico.Values.Count; k++)
            {
                Axe axe = axesDico.Values.ElementAt(k);
                for (int i = 0; i < axe.EntitiesDico.Values.Count; i++)
                {
                    Entity entity = axe.EntitiesDico.Values.ElementAt(i);
                    if (entity is Facility)
                    {
                        for (int e = 0; e < nbrVar; e++)
                            coefs[e] = 0.0;
                        for (int j = 0; j < linesDico.Values.Count; j++)
                        {
                            Unit unit = linesDico.Values.ElementAt(j);
                            if (entity.Units.Contains(unit))
                            {
                                if (unit.ProductItProduces == Legende.ACS)
                                    coefs[j] = -1;
                                else
                                    coefs[j] = unit.ACSConsumption;
                            }
                        }
                        constrIndex++;
                        solverAdapter.AddConstraint(solver, (double[])coefs.Clone(), SolverAdapterConstraintTypes.LE, 0.0);
                        solverAdapter.SetConstraintName(solver, constrIndex, "Contrainte_ACSConso_in_" + entity.EntityName);
                    }
                }
            }
        }

        private void AddConsoACPConstraints()
        {
            int nbrVar = linesDico.Values.Count;
            double[] coefs = new double[nbrVar];

            for (int k = 0; k < axesDico.Values.Count; k++)
            {
                Axe axe = axesDico.Values.ElementAt(k);
                for (int i = 0; i < axe.EntitiesDico.Values.Count; i++)
                {
                    Entity entity = axe.EntitiesDico.Values.ElementAt(i);
                    if (entity is Facility)
                    {
                        for (int e = 0; e < nbrVar; e++)
                            coefs[e] = 0.0;
                        for (int j = 0; j < linesDico.Values.Count; j++)
                        {
                            Unit unit = linesDico.Values.ElementAt(j);
                            if (entity.Units.Contains(unit))
                            {
                                if (unit.ProductItProduces == Legende.PRODUCT_ACP)
                                    coefs[j] = -1.0;
                                else
                                    coefs[j] = unit.ACPConsumption;
                            }
                        }
                        constrIndex++;
                        solverAdapter.AddConstraint(solver, (double[])coefs.Clone(), SolverAdapterConstraintTypes.LE, 0.0);
                        solverAdapter.SetConstraintName(solver, constrIndex, "Contrainte_ACPConso_in" + entity.EntityName);
                    }
                    
                }
            }
        }
        public object[,] Solve()
        {
            int nbrVar = linesDico.Values.Count;
            object test;
            double[] result = new double[nbrVar + 1];
            object[,] finalResult = new object[nbrVar + 3, 2];
            solver = solverAdapter.CreateProblem(nbrVar);

            AddCapacityConstraintsAndTargetObj(finalResult);
            AddPdvConstraints();
            AddConsoACSConstraints();
            AddConsoACPConstraints();

            test = solverAdapter.Solve(solver);
            solverAdapter.GetVariables(solver, result);
            result[nbrVar] = solverAdapter.GetObjective(solver);

            finalResult[0, 0] = "Production Line";
            finalResult[0, 1] = "Production Quantity" + capexObjectManager.Year;

            finalResult[nbrVar + 1, 0] = "Total Cost";
            finalResult[nbrVar + 2, 0] = "Rock Needs";

            for (int k = 0; k < nbrVar + 1; k++)
            {
                finalResult[k + 1, 1] = result[k];
            }

            finalResult[nbrVar + 2, 1] = 0.0;
            for (int i = 0; i < linesDico.Values.Count; i++)
            {
                Unit unit = linesDico.Values.ElementAt(i);
                finalResult[nbrVar + 2, 1] = (double)finalResult[nbrVar + 2, 1] + result[i] * unit.RockConsumption;
            }

            bool ret = solverAdapter.WriteLp(solver, "LpFile_"+capexObjectManager.Year+".txt");

            return finalResult;
        }
    }
}
