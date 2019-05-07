
/* Bibliothèque des fonctions standardisés faisant appel à la bibliothèque Gurobi70.Net */

using System;
using System.Collections.Generic;
using System.Text;
using Gurobi;


namespace Switcher.Lib.Adapters
{
    class GurobiAdapter : AbstractSolverAdapter
    {
        
       
        private GRBEnv env;
        private string output_file_name;

        int ind = 0;
        int NUM_COLS;

        public override bool Init()
        {
            env = new GRBEnv("Log_Gurobi.txt");
            
            return true;
        }

        public override bool AddColumn(object solver, double[] column)
        {
            throw new NotImplementedException();
        }

        public override bool AddColumnEx(object solver, int count, double[] column, int[] rowno)
        {
            throw new NotImplementedException();
        }

        public override bool AddConstraint(object solver, double[] row, SolverAdapterConstraintTypes constr_type, double rh)
        {
            
            GRBModel grbSolver = solver as GRBModel;
            if (grbSolver == null)
                return false;
            GRBLinExpr expr = 0.0;
            ind++;
            expr.AddTerms(row, grbSolver.GetVars());
            grbSolver.AddConstr(expr, (char)GetAdaptedConstraintType(constr_type), rh, "R" + ind);
            return true;
        }

        public override bool AddConstraintEx(object solver, int count, double[] row, int[] colno, SolverAdapterConstraintTypes constr_type, double rh)
        {
            throw new NotImplementedException();
        }

        public override int AddSos(object solver, string name, int sostype, int priority, int count, int[] sosvars, double[] weights)
        {
            GRBModel grbSolver = solver as GRBModel;
            if (grbSolver == null)
                return 0;
            if ((sostype!=2)&(sostype!=1))
            {
                return 0;
            }
            GRBVar[] sosGrbVars = new GRBVar[count];
            GRBVar[] allgrbVars = grbSolver.GetVars();
            for (int i = 0; i < count; i++)
                sosGrbVars[i] = allgrbVars[sosvars[i]];
            if (sostype == 2)
            {
                grbSolver.AddSOS(sosGrbVars, weights, GRB.SOS_TYPE2);
            }
            if (sostype==1)
            {
                grbSolver.AddSOS(sosGrbVars, weights, GRB.SOS_TYPE1);
            }
             
            return 1;
        }

        public override object CopyLp(object solver)
        {
            throw new NotImplementedException();
        }

        public override object CreateProblem(int columns)
        {
            NUM_COLS = columns;
            GRBModel model = new GRBModel(env);
            for (int i = 0; i < NUM_COLS; i++)
            {
                model.AddVar(Double.NegativeInfinity, Double.PositiveInfinity, 0.0, GRB.CONTINUOUS, "C" + i);
            }
            model.Update();
            return model;
        }

        public override bool DelColumn(object solver, int column)
        {
            GRBModel model = solver as GRBModel;
            if (model == null)
                return false;
            model.Remove(model.GetVars()[column]);
            model.Update();
            return true;
        }

        public override bool DelConstraint(object solver, int row)
        {
            GRBModel model = solver as GRBModel;
            if (model == null)
                return false;
            model.Remove(model.GetConstrs()[row]);
            model.Update();
            return true;
        }

        public override int DeleteLp(object solver)
        {
            GRBModel grbSolver = solver as GRBModel;
            if (grbSolver == null)
                return -1;
            grbSolver.Reset();
            return 0;
        }

        public override bool DualizeLp(object solver)
        {
            throw new NotImplementedException();
        }

        public override bool GetColumn(object solver, int col_nr, double[] column)
        {
            throw new NotImplementedException();
        }

        public override int GetColumnEx(object solver, int col_nr, double[] column, int[] nzrow)
        {
            throw new NotImplementedException();
        }

        public override double GetConstrValue(object solver, int row, int count, double[] primsolution, int[] nzindex)
        {
            throw new NotImplementedException();
        }

        public override bool GetConstraints(object solver, double[] constr)
        {
            throw new NotImplementedException();
        }

        public override int GetNonZeros(object solver)
        {
            throw new NotImplementedException();
        }

        public override double GetObjective(object solver)
        {
            GRBModel grbSolver = solver as GRBModel;
            if (grbSolver == null)
                return 0.0;
            return ((GRBLinExpr)grbSolver.GetObjective()).Value;
        }

        public override bool GetVariables(object solver, double[] var)
        {
            GRBModel grbSolver = solver as GRBModel;
            if (grbSolver == null)
                return false;
            GRBVar[] vars = grbSolver.GetVars();
            for (int i = 0; i < var.Length; i++)
                var[i] = vars[i].X;
            return true;
        }

        public override void PrintLp(object solver)
        {
            GRBModel grbSolver = solver as GRBModel;
            grbSolver.Write(output_file_name);
        }

        public override void PrintObjective(object solver)
        {
            throw new NotImplementedException();
        }

        public override void PrintSolution(object solver, int columns)
        {
            throw new NotImplementedException();
        }

        public override void PrintStr(object solver, string str)
        {
            throw new NotImplementedException();
        }

        public override bool SetColumn(object solver, int col_no, double[] column)
        {
            throw new NotImplementedException();
        }

        public override bool SetColumnEx(object solver, int col_no, int count, double[] column, int[] rowno)
        {
            throw new NotImplementedException();
        }

        public override bool SetConstraintName(object solver, int constraintIndex, string constraintName)
        {
            throw new NotImplementedException();
        }

        public override bool SetObj(object solver, int Column, double Value)
        {
            GRBModel grbSolver = solver as GRBModel;
            if (grbSolver == null)
                return false;
            GRBLinExpr exp = grbSolver.GetObjective() as GRBLinExpr;
            exp.AddTerm(Value, exp.GetVar(Column));
            return true;
        }

        public override bool SetObjFn(object solver, double[] row)
        {
            GRBModel grbSolver = solver as GRBModel;
            if (grbSolver == null)
                return false;
            GRBLinExpr obj = new GRBLinExpr();

            obj.AddTerms(row, grbSolver.GetVars());
            grbSolver.SetObjective(obj, GRB.MINIMIZE);
            return true;
        }

        public override bool SetOutputFile(object solver, string filename)
        {
            output_file_name = filename;
            return true;
        }

        public override bool SetVariableName(object solver, int varIndex, string varName)
        {
            throw new NotImplementedException();
        }

        public override object Solve(object solver)
        {
            GRBModel grbSolver = solver as GRBModel;
            if (grbSolver == null)
                return false;
            grbSolver.Optimize();
            Console.WriteLine("Obj: " + grbSolver.ObjVal);
            return grbSolver;
        }

        public override bool StrAddColumn(object solver, string col_string)
        {
            throw new NotImplementedException();
        }

        public override bool StrAddConstraint(object solver, string row_string, SolverAdapterConstraintTypes constr_type, double rh)
        {
            throw new NotImplementedException();
        }

        public override double TimeElapsed(object solver)
        {
            throw new NotImplementedException();
        }

        public override void Unscale(object solver)
        {
            throw new NotImplementedException();
        }
        public override bool WriteLp(object solver, string fileName)
        {
            throw new NotImplementedException();
        }

        
        protected override object GetAdaptedConstraintType(SolverAdapterConstraintTypes constr_type)
        {
            switch (constr_type)
            {
                case SolverAdapterConstraintTypes.LE:
                    return GRB.LESS_EQUAL;
                case SolverAdapterConstraintTypes.GE:
                    return GRB.GREATER_EQUAL;
                case SolverAdapterConstraintTypes.EQ:
                    return GRB.EQUAL;
                default:
                    return GRB.LESS_EQUAL;
            }
        }
    }
}
