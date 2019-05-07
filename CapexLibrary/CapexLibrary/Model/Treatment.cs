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
    public class Treatment : Unit
    {
        private TreatmentType treatmentType;
        public Treatment(string unitName, TreatmentType treatmentType) : base(unitName)
        {
            this.treatmentType = treatmentType;
        }

        public TreatmentType TreatmentType
        {
            get { return treatmentType; }
        }

    }
}
