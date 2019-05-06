
/* Bibliothèque des fonctions standardisés faisant appel à la bibliothèque LpSolve55 */

using System;
using System.Collections.Generic;
using System.Text;
using lpsolve55;

namespace Switcher.Lib.Adapters
{
    public class LpSolveAdapter : AbstractSolverAdapter
    {
        private int NUM_COLS;
        private double[] row_temp;
        
        private double[] UpdateRow(double[] row)
        {
            double[] row_temp = new double[NUM_COLS + 1];
            row_temp[0] = 0;

            for (int i = 1; i < NUM_COLS + 1; i++)
            {
                row_temp[i] = row[i-1];
            }
            return row_temp;
        }

        public override bool Init()
        {
            return lpsolve.Init();
        }
       
        public override bool AddColumn(object solver,double[] column)
        {
            return lpsolve.add_column((IntPtr)solver, column);
        }

        public override bool AddColumnEx(object solver, int count, double[] column, int[] rowno)
        {
            return lpsolve.add_columnex((IntPtr)solver, count, column, rowno);
        }

        public override bool AddConstraint(object solver, double[] row, SolverAdapterConstraintTypes constr_type, double rh)
        {
            row_temp = UpdateRow(row);
            return lpsolve.add_constraint((IntPtr)solver, row_temp, (lpsolve.lpsolve_constr_types)GetAdaptedConstraintType(constr_type), rh);
        }

        public override bool AddConstraintEx(object solver, int count, double[] row, int[] colno, SolverAdapterConstraintTypes constr_type, double rh)
        {
            return lpsolve.add_constraintex((IntPtr)solver, count, row, colno, (lpsolve.lpsolve_constr_types)GetAdaptedConstraintType(constr_type), rh);
        }

        public override int AddSos(object solver, string name, int sostype, int priority, int count, int[] sosvars, double[] weights)
        {
            return lpsolve.add_SOS((IntPtr)solver, name, sostype, priority, count, sosvars, weights);
        }

        public override object CopyLp(object solver)
        {
            return lpsolve.copy_lp((IntPtr)solver);
        }

        public override object CreateProblem(int columns)
        {
            lpsolve.Init();
            NUM_COLS = columns;
            return lpsolve.make_lp(1, columns);
        }

        public override bool DelColumn(object solver, int column)
        {
            return lpsolve.del_column((IntPtr)solver, column);
        }

        public override bool DelConstraint(object solver, int row)
        {
            return lpsolve.del_constraint((IntPtr)solver, row);
        }

        public override int DeleteLp(object solver)
        {
            lpsolve.delete_lp((IntPtr)solver);
            return 1;
        }

        public override bool DualizeLp(object solver)
        {
            return lpsolve.dualize_lp((IntPtr)solver);
        }

        public override bool GetColumn(object solver, int col_nr, double[] column)
        {
            return lpsolve.get_column((IntPtr)solver, col_nr, column);
        }

        public override int GetColumnEx(object solver, int col_nr, double[] column, int[] nzrow)
        {
            return lpsolve.get_columnex((IntPtr)solver, col_nr, column, nzrow);
        }

        public override double GetConstrValue(object solver, int row, int count, double[] primsolution, int[] nzindex)
        {
            return lpsolve.get_constr_value((IntPtr)solver, row, count, primsolution, nzindex);
        }

        public override bool GetConstraints(object solver, double[] constr)
        {
            return lpsolve.get_constraints((IntPtr)solver, constr);
        }

        public override int GetNonZeros(object solver)
        {
            return lpsolve.get_nonzeros((IntPtr)solver);
        }

        public override double GetObjective(object solver)
        {
            return lpsolve.get_objective((IntPtr)solver);
        }

        public override bool GetVariables(object solver, double[] var)
        {
            return lpsolve.get_variables((IntPtr)solver, var);
        }

        public override void PrintLp(object solver)
        {
            lpsolve.print_lp((IntPtr)solver);
        }

        public override void PrintObjective(object solver)
        {
            lpsolve.print_objective((IntPtr)solver);
        }

        public override void PrintSolution(object solver, int columns)
        {
            lpsolve.print_solution((IntPtr)solver, columns);
        }

        public override void PrintStr(object solver, string str)
        {
            lpsolve.print_str((IntPtr)solver, str);
        }

        public override bool SetColumn(object solver, int col_no, double[] column)
        {
            return lpsolve.set_column((IntPtr)solver, col_no, column);
        }

        public override bool SetColumnEx(object solver, int col_no, int count, double[] column, int[] rowno)
        {
            return lpsolve.set_columnex((IntPtr)solver, col_no, count, column, rowno);
        }

        public override bool SetConstraintName(object solver, int constraintIndex, string constraintName)
        {
            return lpsolve.set_row_name((IntPtr)solver, constraintIndex, constraintName);
        }

        public override bool SetObj(object solver, int Column, double Value)
        {
            return lpsolve.set_obj((IntPtr)solver, Column, Value);
        }

        public override bool SetObjFn(object solver, double[] row)
        {
            row_temp = UpdateRow(row);
            return lpsolve.set_obj_fn((IntPtr)solver, row_temp);
        }

        public override bool SetOutputFile(object solver, string filename)
        {
            return lpsolve.set_outputfile((IntPtr)solver, filename);
        }

        public override bool SetVariableName(object solver, int varIndex, string varName)
        {
            return lpsolve.set_col_name((IntPtr)solver, varIndex, varName);
        }

        public override object Solve(object solver)
        {
            return lpsolve.solve((IntPtr)solver);
        }

        public override bool StrAddColumn(object solver, string col_string)
        {
            return lpsolve.str_add_column((IntPtr)solver, col_string);
        }

        public override bool StrAddConstraint(object solver, string row_string, SolverAdapterConstraintTypes constr_type, double rh)
        {

            return lpsolve.str_add_constraint((IntPtr)solver, row_string, (lpsolve.lpsolve_constr_types)GetAdaptedConstraintType(constr_type), rh);
        }

        public override double TimeElapsed(object solver)
        {
            return lpsolve.time_elapsed((IntPtr)solver);
        }

        public override void Unscale(object solver)
        {
            lpsolve.unscale((IntPtr)solver);
        }
        public override bool WriteLp(object solver, string fileName)
        {
            return lpsolve.write_lp((IntPtr)solver, fileName);
        }

        protected override object GetAdaptedConstraintType(SolverAdapterConstraintTypes constr_type)
        {
            switch(constr_type)
            {
                case SolverAdapterConstraintTypes.LE:
                    return lpsolve.lpsolve_constr_types.LE;
                case SolverAdapterConstraintTypes.GE:
                    return lpsolve.lpsolve_constr_types.GE;

                case SolverAdapterConstraintTypes.EQ:
                    return lpsolve.lpsolve_constr_types.EQ;
                default:
                    return lpsolve.lpsolve_constr_types.LE;
            }
        }


    }
}
