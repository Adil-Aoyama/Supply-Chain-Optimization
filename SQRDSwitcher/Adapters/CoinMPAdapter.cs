
/* Bibliothèque des fonctions standardisés faisant appel à la bibliothèque CoinMP */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;




namespace Switcher.Lib.Adapters
{
    public class CoinMPAdapter : AbstractSolverAdapter
    {
        private static int NUM_COLS, NUM_ROWS;
        private double[,] Matrix;
        private int[] MatrixBegin;
        private int[] MatrixCount;
        private int[] MatrixIndex;
        private double[] MatrixValues;
        private double[] ObjectCoeffs;
        private char[] RowTypes;
        private double[] RhsValues;
        private double[] RangeValues;
        private double[] UpperBounds;
        private double[] LowerBounds;
        private string[] ColNames;
        private string[] RowNames;
        private string ObjName = "Fonction Objectif";

        private int SosCount = 0, SosNZCount = 0;
        private int[] SosType, SosPrior, SosBegin, SosIndex;
        private double[] sosRef;

        private double[,] UpdateMatrix(double[,] matrix, int numrow)
        {
            double[,] matrix_temp;

            matrix_temp = new double[numrow, NUM_COLS];

            for (int i = 0; i < numrow - 1; i++)
            {
                for (int j = 0; j < NUM_COLS; j++)
                {
                        matrix_temp[i, j] = matrix[i, j];
                }
             }

            return matrix_temp;
        }

        private int[] UpdateMatrixIndex(int[] MatrixIndex, int numrow, int numcol)
        {
            MatrixIndex = new int[numrow * numcol];

            return MatrixIndex;
        }

        private  char[] UpdateRowTypes(char[] rowtype, int numrow)
        {
           char[] RowTypes_temp = new char[numrow];

            for (int i = 0; i < numrow - 1; i++)
                {
                    RowTypes_temp[i] = rowtype[i];
                }

            RowTypes_temp[numrow - 1] = 'L';
            return RowTypes_temp;
        }

        private double[] UpdateRhsValues(double[] rhsvalue, int numrow)
        {
            double[] RhsValues_temp = new double[numrow];

            for (int i = 0; i < numrow - 1; i++)
            {
                RhsValues_temp[i] = rhsvalue[i];
            }

            return RhsValues_temp;
        }

        private double[] UpdateRangeValues(double[] rangevalue, int numrow)
        {
            double[] RangeValues_temp = new double[numrow];

            for (int i = 0; i < numrow - 1; i++)
            {
                RangeValues_temp[i] = rangevalue[i];
            }

            return RangeValues_temp;
        }

        private string[] UpdateRowNames(string[] rowname, int numrow)
        {
            string[] RowNames_temp = new string[numrow];

            for (int i = 0; i < numrow - 1; i++)
            {
                RowNames_temp[i] = rowname[i];
            }

            RowNames_temp[numrow - 1] = "R" + numrow;
            return RowNames_temp;
        }

        private double[] UpdateUpperBounds(double[] upperbounds, int numrow)
        {
            double[] UpperBounds_temp = new double[NUM_ROWS + 1];

            for (int i = 0; i < NUM_ROWS; i++)
            {
                UpperBounds_temp[i] = upperbounds[i];
            }

            UpperBounds_temp[NUM_ROWS] = Double.PositiveInfinity;
            return UpperBounds_temp;
        }

        private double[] UpdateLowerBounds(double[] lowerbounds, int numrow)
        {
            double[] LowerBounds_temp = new double[NUM_ROWS + 1];

            for (int i = 0; i < NUM_ROWS; i++)
            {
                LowerBounds_temp[i] = lowerbounds[i];
            }

            LowerBounds_temp[NUM_ROWS] = Double.NegativeInfinity;
            return LowerBounds_temp;
        }


                                                                            // Fonctions Solveurs//
        
        public override bool Init()
        {
            if (CoinMP.CoinInitSolver("") == 0) return false;
            else return true;
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
            NUM_ROWS++;

            if (NUM_ROWS == 1)
            {
                for (int i = 0; i < NUM_COLS; i++)
                {
                    Matrix[0, i] = row[i];
                }

                if ((lpsolve55.lpsolve.lpsolve_constr_types)constr_type == lpsolve55.lpsolve.lpsolve_constr_types.LE)
                {
                    RhsValues[0] = rh;
                    RangeValues[0] = Double.NegativeInfinity;
                }

                if ((lpsolve55.lpsolve.lpsolve_constr_types)constr_type == lpsolve55.lpsolve.lpsolve_constr_types.GE)
                {
                    RhsValues[0] = Double.PositiveInfinity;
                    RangeValues[0] = rh;
                }

                RowTypes[0] = 'L';
                RowNames[0] = "R" + NUM_ROWS;
            }
            else
            {
                Matrix = UpdateMatrix(Matrix, NUM_ROWS);

                for (int i = 0; i < NUM_COLS; i++)
                {
                    Matrix[NUM_ROWS - 1, i] = row[i];
                }

                RowTypes = UpdateRowTypes(RowTypes, NUM_ROWS);
                RowNames = UpdateRowNames(RowNames, NUM_ROWS);
                RhsValues = UpdateRhsValues(RhsValues, NUM_ROWS);
                RangeValues = UpdateRangeValues(RangeValues, NUM_ROWS);

                if ((lpsolve55.lpsolve.lpsolve_constr_types)constr_type == lpsolve55.lpsolve.lpsolve_constr_types.LE)
                {
                    RhsValues[NUM_ROWS - 1] = rh;
                    RangeValues[NUM_ROWS - 1] = Double.NegativeInfinity;
                }

                if ((lpsolve55.lpsolve.lpsolve_constr_types)constr_type == lpsolve55.lpsolve.lpsolve_constr_types.GE)
                {
                    RhsValues[NUM_ROWS - 1] = Double.PositiveInfinity;
                    RangeValues[NUM_ROWS - 1] = rh;
                }
            }

            return true;
        }

        public override bool AddConstraintEx(object solver, int count, double[] row, int[] colno, SolverAdapterConstraintTypes constr_type, double rh)
        {
            throw new NotImplementedException();
        }

        public override int AddSos(object solver, string name, int sostype, int priority, int count, int[] sosvars, double[] weights)
        {
            SosCount++;

            return 1;
        }

        public override object CopyLp(object solver)
        {
            throw new NotImplementedException();
        }

        public override object CreateProblem(int columns)
        {
            NUM_COLS = columns;
            NUM_ROWS = 0;
            ObjectCoeffs = new double[columns];
            Matrix = new double[1, columns];
            RowTypes = new char[1];
            RhsValues = new double[1];
            RangeValues = new double[1];
            ColNames = new string[NUM_COLS];
            RowNames = new string[1];
            SosType = new int[1];
            SosPrior = new int[1];
            SosBegin = new int[1];
            SosIndex = new int[1];

            for(int i = 0; i < NUM_COLS; i++)
            {
                ColNames[i] = "C" + i;
            }

            return (IntPtr)CoinMP.CoinCreateProblem("Problem_CoinMP");
        }

        public override bool DelColumn(object solver, int column)
        {
            throw new NotImplementedException();
        }

        public override bool DelConstraint(object solver, int row)
        {
            throw new NotImplementedException();
        }

        public override int DeleteLp(object solver)
        {
            return CoinMP.CoinFreeSolver();
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
            return CoinMP.CoinGetObjectValue((IntPtr)solver);
        }

        public override bool GetVariables(object solver, double[] var)
        {
            throw new NotImplementedException();
        }

        public override void PrintLp(object solver)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override bool SetObjFn(object solver, double[] row)
        {
            for (int i = 0; i < NUM_COLS; i++)
            {
                ObjectCoeffs[i] = row[i];
            }

            return true;
        }

        public override bool SetOutputFile(object solver, string filename)
        {
            if (CoinMP.CoinWriteFile((IntPtr)solver, CoinMP.SOLV_FILE_MPS, filename) == 0) return false;
            return true;
        }

        public override bool SetVariableName(object solver, int varIndex, string varName)
        {
            throw new NotImplementedException();
        }

        public override object Solve(object solver)     /* La construction de la matrice des données du problème n'est pas prise en charge par le solveur Coin   */
        {                                               /* Celle-ci se fait au fur et à mesure qu'on ajoute des données par le biais du code de la fonction Solve() */
            int ind=0;
            double objectConst=0.0;
            int NZCount=0;
            int result;
            UpperBounds = new double[NUM_COLS];
            LowerBounds = new double [NUM_COLS];
            MatrixBegin = new int [NUM_COLS+1];
            MatrixCount = new int [NUM_COLS];

            for (int i = 0; i < NUM_COLS; i++)
            {
                UpperBounds[i] = Double.PositiveInfinity;
                LowerBounds[i] = Double.NegativeInfinity;
                MatrixBegin[i] = i*2;
            }
            MatrixBegin[NUM_COLS] = NUM_COLS * 2;


            for(int i=0; i<NUM_COLS; i++)
            {
                int count=0;
                for(int j=0; j<NUM_ROWS; j++)
                {
                    if(Matrix[j,i]!=0)
                    {
                        NZCount++;
                        count++;
                    }
                }
                MatrixCount[i]=count;
            }

            MatrixValues = new double[NZCount];
            MatrixIndex = new int[NZCount];

            for (int i = 0; i < NUM_COLS; i++)
            {
                for (int j = 0; j < NUM_ROWS; j++)
                {
                    if (Matrix[j, i] != 0)
                    {
                        MatrixValues[ind] = Matrix[j, i];
                        MatrixIndex[ind] = j;
                        ind++;
                    }
                }
            }

            CoinMP.CoinLoadProblem((IntPtr)solver, NUM_COLS, NUM_ROWS, NZCount, 0, CoinMP.SOLV_OBJSENS_MAX, objectConst, ObjectCoeffs, LowerBounds, UpperBounds,
                                    RowTypes, RhsValues, RangeValues, MatrixBegin, MatrixCount, MatrixIndex, MatrixValues, ColNames, RowNames, ObjName);

            result = CoinMP.CoinOptimizeProblem((IntPtr)solver);

            result = CoinMP.CoinWriteFile((IntPtr)solver, CoinMP.SOLV_FILE_MPS, "Result_CoinMP");

            return CoinMP.CoinGetSolutionTextIntPtr((IntPtr)solver);
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
            return null;
        }
    }
}
