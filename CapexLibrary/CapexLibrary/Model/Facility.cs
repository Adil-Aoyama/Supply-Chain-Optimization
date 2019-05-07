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
    public class Facility : Entity
    {
        public Facility(string entityName) : base(entityName) { }

    }
}
