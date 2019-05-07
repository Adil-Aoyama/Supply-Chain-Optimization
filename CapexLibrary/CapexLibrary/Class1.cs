using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDna.Integration;

namespace CapexLibrary
{
    public class Class1
    {
        [ExcelFunction(Description = "Ma première function ExcelDna")]
        public static double[] Jmaa(double[] a, double[] b)
        {
            double[] c = new double[100];

            for (int i = 0; i < 100; i++)
            {
                c[i] = a[i] + b[i];
            }
            
            return c;
        }

        [ExcelFunction(Description = "Ma deuxième function ExcelDna")]
        public static double[] Chaal(Object[,] a)
        {
            int counto;
            double[] c = new double[3];

            for (int i = 0; i < 3; i++)
            {
                counto = 0;
                c[i]=0;
                for(int j = 0; j < 18; j++)
                {
                    if(String.Equals(a[j,i], "oui")==true) counto++;
                }

                c[i] = counto;
            }
            
            return c;
        }
    }
}
