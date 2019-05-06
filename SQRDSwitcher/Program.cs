/* Program main: Résolution du problème */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Switcher.Lib.Adapters;


namespace Switcher.Test
{
    class Program
    {
        public static string SolverChoisi = "LpSolve"; /// Le choix du solveur se fait à ce niveau. Chosissez un des solveurs suivants: LpSolve - CoinMP - Gurobi

        public static string FileName = string.Concat("Test_", SolverChoisi,".txt");

        static void Main(string[] args)
        {
            ISolverAdapter SolverAdapter = AbstractSolverAdapter.GetSolverAadapter(SolverChoisi);

            object solver = SolverAdapter.CreateProblem(4); /// Entrer le nombre des variables (colonnes) comme paramétre de la fonction CreateProblem (ici c'est 4)


            SolverAdapter.SetObjFn(solver, new double[] { 87.9, 0, -40.45, 33.32 });

            SolverAdapter.AddConstraint(solver, new double[] { 3000, 200, 2000, 1000 }, SolverAdapterConstraintTypes.LE, 4000); /// ".LE" pour "Lower than or equal to"
            SolverAdapter.AddConstraint(solver, new double[] { 0, 4000, 330, 112 }, SolverAdapterConstraintTypes.GE, 100); /// ".GE" pour "Greater than or equal to"
            SolverAdapter.AddConstraint(solver, new double[] { 1875, 61, 0, 3152 }, SolverAdapterConstraintTypes.LE, 7200);
            SolverAdapter.AddConstraint(solver, new double[] { -100, 6.3, 7.5, 300 }, SolverAdapterConstraintTypes.LE, 1300);

            SolverAdapter.Solve(solver);

            SolverAdapter.WriteLp(solver, FileName);

            Console.ReadLine();
        }
    }
}
