using System;
using System.Collections.Generic;
using System.Text;

namespace Switcher.Lib.Adapters
{
    public abstract class AbstractSolverAdapter : ISolverAdapter
    {

        public static ISolverAdapter GetSolverAadapter(string solverName)
        {
            if (solverName == "LpSolve")
                return new LpSolveAdapter();

            else if (solverName == "CoinMP")
                return new CoinMPAdapter();

            else if (solverName == "Gurobi")
                return new GurobiAdapter();
            else
                throw new NotImplementedException();
        }



        public abstract bool Init();
        
        public abstract bool AddColumn(object solver,double[] column);

        public abstract bool AddColumnEx(object solver, int count, double[] column, int[] rowno);

        public abstract bool AddConstraint(object solver, double[] row, SolverAdapterConstraintTypes constr_type, double rh);

        public abstract bool AddConstraintEx(object solver, int count, double[] row, int[] colno, SolverAdapterConstraintTypes constr_type, double rh);

        public abstract int AddSos(object solver, string name, int sostype, int priority, int count, int[] sosvars, double[] weights);

        public abstract object CopyLp(object solver);

        public abstract object CreateProblem(int columns);

        public abstract bool DelColumn(object solver, int column);

        public abstract bool DelConstraint(object solver, int row);

        public abstract int DeleteLp(object solver);

        public abstract bool DualizeLp(object solver);

        public abstract bool GetColumn(object solver, int col_nr, double[] column);

        public abstract int GetColumnEx(object solver, int col_nr, double[] column, int[] nzrow);

        public abstract double GetConstrValue(object solver, int row, int count, double[] primsolution, int[] nzindex);

        public abstract bool GetConstraints(object solver, double[] constr);

        public abstract int GetNonZeros(object solver);

        public abstract double GetObjective(object solver);

        public abstract bool GetVariables(object solver, double[] var);

        public abstract void PrintLp(object solver);

        public abstract void PrintObjective(object solver);

        public abstract void PrintSolution(object solver, int columns);

        public abstract void PrintStr(object solver, string str);

        public abstract bool SetColumn(object solver, int col_no, double[] column);

        public abstract bool SetColumnEx(object solver, int col_no, int count, double[] column, int[] rowno);

        public abstract bool SetConstraintName(object solver, int constraintIndex, string constraintName);

        public abstract bool SetObj(object solver, int Column, double Value);

        public abstract bool SetObjFn(object solver, double[] row);

        public abstract bool SetOutputFile(object solver, string filename);

        public abstract bool SetVariableName(object solver, int varIndex, string varName);

        public abstract object Solve(object solver);

        public abstract bool StrAddColumn(object solver, string col_string);

        public abstract bool StrAddConstraint(object solver, string row_string, SolverAdapterConstraintTypes constr_type, double rh);

        public abstract double TimeElapsed(object solver);

        public abstract void Unscale(object solver);

        public abstract bool WriteLp(object solver, string fileName);

        protected abstract object GetAdaptedConstraintType(SolverAdapterConstraintTypes constr_type);

    }
}
