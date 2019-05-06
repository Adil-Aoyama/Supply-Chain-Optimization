using System;
using System.Collections.Generic;
using System.Text;


namespace Switcher.Lib.Adapters
{

    public interface ISolverAdapter
    {
        
        bool Init();
        
        bool AddColumn(object solver, double[] column);

        bool AddColumnEx(object solver, int count, double[] column, int[] rowno);

        bool AddConstraint(object solver, double[] row, SolverAdapterConstraintTypes constr_type, double rh);

        bool AddConstraintEx(object solver, int count, double[] row, int[] colno, SolverAdapterConstraintTypes constr_type, double rh);

        int AddSos(object solver, string name, int sostype, int priority, int count, int[] sosvars, double[] weights);

        object CopyLp(object solver);

        bool DelColumn(object solver, int column);

        bool DelConstraint(object solver, int row);

        int DeleteLp(object solver);

        bool DualizeLp(object solver);

        bool GetColumn(object solver, int col_nr, double[] column);

        int GetColumnEx(object solver, int col_nr, double[] column, int[] nzrow);

        double GetConstrValue(object solver, int row, int count, double[] primsolution, int[] nzindex);

        bool GetConstraints(object solver, double[] constr);

        int GetNonZeros(object solver);

        double GetObjective(object solver);

        bool GetVariables(object solver, double[] var);

        object CreateProblem(int columns);

        void PrintLp(object solver);

        void PrintObjective(object solver);

        void PrintSolution(object solver, int columns);

        void PrintStr(object solver, string str);

        bool SetColumn(object solver, int col_no, double[] column);

        bool SetColumnEx(object solver, int col_no, int count, double[] column, int[] rowno);

        bool SetConstraintName(object solver, int constraintIndex, string constraintName);

        bool SetObj(object solver, int Column, double Value);

        bool SetObjFn(object solver, double[] row);

        bool SetOutputFile(object solver, string filename);

        bool SetVariableName(object solver, int varIndex, string varName);

        object Solve(object solver);

        bool StrAddColumn(object solver, string col_string);

        bool StrAddConstraint(object solver, string row_string, SolverAdapterConstraintTypes constr_type, double rh);

        double TimeElapsed(object solver);

        void Unscale(object solver);

        bool WriteLp(object solver, string fileName);

    }
  
}
