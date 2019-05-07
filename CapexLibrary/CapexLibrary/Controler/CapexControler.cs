using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapexLibrary.Utils;
using CapexLibrary.Controler;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;
using CapexLibrary.Model;

namespace CapexLibrary.Controler
{
    public class CapexControler
    {
        private Excel.Application xllApp;
        private Excel.Workbook wrkBook;

        private object[,] config;
        private Dictionary<string, Object[,]> configDico;
        private Dictionary<string, CapexObjectManager> objectManagerDico;

        public CapexControler(object[,] config)
        {
            xllApp = (Excel.Application)ExcelDnaUtil.Application;
            wrkBook = xllApp.ActiveWorkbook;
            this.config = config;
            configDico = new Dictionary<string, object[,]>();
            LoadInputs();
            objectManagerDico = new Dictionary<string, CapexObjectManager>();
            CreateObjectManagers();
            CreateModelStructure();
        }
        public Dictionary<string, CapexObjectManager> ObjectManagerDico
        {
            get { return objectManagerDico; }
        }
        public Excel.Application XllApp
        {
            get { return xllApp; }
        }
        public Excel.Workbook WrkBook
        {
            get { return wrkBook; }
        }

        private void LoadInputs()
        {
            Object[,] input = null;
            double firstRow;
            string inputName;
            string nameSheet;

            for (int i = 0; i < config.GetLength(0); i++)
            {
                inputName = (string)config[i, 0];
                if (string.IsNullOrEmpty(inputName))
                    break;
                nameSheet = (string)config[i, 1];
                firstRow = (double)config[i, 2];

                input = ExcelReader.LoadSheet(WrkBook, nameSheet, (int)firstRow);
                configDico[inputName] = input;
            }
        }
        private void CreateObjectManagers()
        {
            string year;
            CapexObjectManager capexObj = null;
            Object[,] pdvInput = configDico[Legende.PDV];

            for (int i = 3; i < pdvInput.GetLength(1); i++)
            {
                year = pdvInput[0, i].ToString();
                capexObj = new CapexObjectManager(year);
                objectManagerDico[year] = capexObj;
                capexObj.GenerateFinalProducts(pdvInput, i);
            }
        }     
        private void CreateModelStructure()
        {
            CreateMinesAndExtractions();
            UpdateExtractionsCapacities();
            CreateAndUpdateLaveriesCapacities();
            CreateLines();
            UpdateLinesCapacities();
            CreateMinesLaveriesConnexions();
            UpdateConsumptions();
            UpdateCosts();
        }

        private void UpdateConsumptions()
        {
            UpdateRockConsumption();
            UpdateACSConsumption();
            UpdateACPConsumption();
            UpdateACP54Consumption();
            UpdateNH3Consumption();
            UpdateSConsumption();
        }
        private void CreateMinesAndExtractions()
        {
            Object[,] capaMineInput = configDico[Legende.CAPAMINE];
            for (int i = 1; i < capaMineInput.GetLength(0); i++)
            {
                string axeName = (string)capaMineInput[i, 1];
                string mineName = (string)capaMineInput[i, 0];
                foreach (CapexObjectManager objManager in objectManagerDico.Values)
                {
                    (objManager.GetOrCreateAxe(axeName)).AddMine(mineName);
                }
            }
        }         
        private void UpdateExtractionsCapacities()
        {
            Object[,] capaMineInput = configDico[Legende.CAPAMINE];

            for (int i = 5; i < capaMineInput.GetLength(1); i++)
            {
                CapexObjectManager capexObj = objectManagerDico[capaMineInput[0,i].ToString()];

                for(int j=1;j<capaMineInput.GetLength(0);j++)
                {
                    string extractionName = (string)capaMineInput[j, 0] + Legende.EXTRACTION;                   
                    Unit extractionUnit = capexObj.GetUnit((string)capaMineInput[j, 1], (string)capaMineInput[j, 0], extractionName);
                    if (capaMineInput[j, i] == null) capaMineInput[j, i] = 0.0;
                    extractionUnit.Capacity = (double)capaMineInput[j, i];
                }
            }
        }
        private void CreateAndUpdateLaveriesCapacities()
        {
            Object[,] capaLaverieInput = configDico[Legende.CAPALAV];

            for (int i = 1; i < capaLaverieInput.GetLength(0); i++)
            {
                string axeName = (string)capaLaverieInput[i, 0];
                string mineName = (string)capaLaverieInput[i, 1];
                string laverieName = (string)capaLaverieInput[i, 2];    
                foreach (CapexObjectManager objManager in objectManagerDico.Values)
                {
                    Entity variableMine = objManager.GetEntity(axeName, mineName);
                    variableMine.AddTreatment(laverieName,TreatmentType.LAVERIE);
                }
            }
            
            for (int i = 6; i < capaLaverieInput.GetLength(1); i++)
            {
                CapexObjectManager capexObj = objectManagerDico[capaLaverieInput[0, i].ToString()];

                for (int j = 1; j < capaLaverieInput.GetLength(0); j++)
                {
                    Unit treatmentUnit = capexObj.GetUnit((string)capaLaverieInput[j, 0], (string)capaLaverieInput[j, 1], (string)capaLaverieInput[j, 2]);
                    if (capaLaverieInput[j, i] == null) capaLaverieInput[j, i] = 0.0;
                    treatmentUnit.Capacity = (double)capaLaverieInput[j, i];
                }
            }
        }
        private void CreateLines()
        {
            Object[,] capaFacilityInput = configDico[Legende.CAPAPROD];

            for (int i = 1; i < capaFacilityInput.GetLength(0); i++)
            {
                string axeName = (string)capaFacilityInput[i, 0];
                string facilityName = (string)capaFacilityInput[i, 1];
                string treatmentName = (string)capaFacilityInput[i, 3];
                
                foreach (CapexObjectManager objManager in objectManagerDico.Values)
                {
                    Axe variableAxe = objManager.GetOrCreateAxe(axeName);
                    variableAxe.AddFacility(facilityName);
                    Entity variableFacility = variableAxe.GetEntity(facilityName);
                    variableFacility.AddTreatment(treatmentName, TreatmentType.FACILITY_LINE);

                }
            }

        }
        private void UpdateLinesCapacities()
        {
            Object[,] capaFacilityInput = configDico[Legende.CAPAPROD];

            for (int i = 8; i < capaFacilityInput.GetLength(1); i++)
            {
                CapexObjectManager capexObj = objectManagerDico[capaFacilityInput[0, i].ToString()];

                for (int j = 1; j < capaFacilityInput.GetLength(0); j++)
                {
                    Unit treatmentUnit = capexObj.GetUnit((string)capaFacilityInput[j, 0], (string)capaFacilityInput[j, 1], (string)capaFacilityInput[j, 3]);
                    string productName = (string)capaFacilityInput[j, 2];  
                    if (capaFacilityInput[j, i] == null) capaFacilityInput[j, i] = 0.0;
                    treatmentUnit.Capacity = (double)capaFacilityInput[j, i];
                    treatmentUnit.ProductItProduces = productName;
                }
            }
        }
        private void CreateMinesLaveriesConnexions()
        {
            Object[,] connexionsInput = configDico[Legende.CONNEXIONS];

            for (int i = 1; i < connexionsInput.GetLength(0); i++)
            {
                string axeName = (string)connexionsInput[i, 1];
                string mineName = (string)connexionsInput[i, 2];
                string extractionName = mineName + Legende.EXTRACTION;
                string connexionName = (string)connexionsInput[i, 4];
                Product interProduct = IntermediateProducts.GetInterProduct((string)connexionsInput[i, 3]);

                CapexObjectManager capexObj = objectManagerDico[connexionsInput[i, 0].ToString()];
                capexObj.UpdateLaveriesDico();
                Entity variableMine = capexObj.GetEntity(axeName, mineName);

                for (int j = 5; j < connexionsInput.GetLength(1); j++)
                {
                    string laverieName = (string)connexionsInput[0, j];
                    if (connexionsInput[i, j].ToString() == "N")
                        continue;
                    
                    Unit laverie = capexObj.GetLaverie(laverieName);
                    Unit extractionUnit = variableMine.GetUnit(extractionName);
                    capexObj.AddConnexion(connexionName, extractionUnit, laverie, (double)connexionsInput[i, j], (double)laverie.Capacity, interProduct);
                    
                }
            }
        }
        private void UpdateRockConsumption()
        {
            Object[,] rockConsumptionInput = configDico[Legende.ROCKCONSO];

            for (int i = 3; i < rockConsumptionInput.GetLength(1); i++)
            {
                CapexObjectManager capexObj = objectManagerDico[rockConsumptionInput[0, i].ToString()];
                capexObj.UpdateLinesDico();

                for (int j = 1; j < rockConsumptionInput.GetLength(0); j++)
                {
                    Unit lineUnit = capexObj.GetLine((string)rockConsumptionInput[j, 2]);
                    if (rockConsumptionInput[j, i] == null) rockConsumptionInput[j, i] = 0.0;
                    lineUnit.RockConsumption = (double)rockConsumptionInput[j, i];
                }
            }
        }
        private void UpdateACSConsumption()
        {
            Object[,] ACSConsumptionInput = configDico[Legende.ROCKCONSO];

            for (int i = 3; i < ACSConsumptionInput.GetLength(1); i++)
            {
                CapexObjectManager capexObj = objectManagerDico[ACSConsumptionInput[0, i].ToString()];
                capexObj.UpdateLinesDico();

                for (int j = 1; j < ACSConsumptionInput.GetLength(0); j++)
                {
                    Unit lineUnit = capexObj.GetLine((string)ACSConsumptionInput[j, 2]);
                    if (ACSConsumptionInput[j, i] == null) ACSConsumptionInput[j, i] = 0.0;
                    lineUnit.ACSConsumption = (double)ACSConsumptionInput[j, i];
                }
            }
        }
        private void UpdateACPConsumption()
        {
            Object[,] ACPConsumptionInput = configDico[Legende.ACPCONSO];

            for (int i = 3; i < ACPConsumptionInput.GetLength(1); i++)
            {
                CapexObjectManager capexObj = objectManagerDico[ACPConsumptionInput[0, i].ToString()];
                capexObj.UpdateLinesDico();

                for (int j = 1; j < ACPConsumptionInput.GetLength(0); j++)
                {
                    Unit lineUnit = capexObj.GetLine((string)ACPConsumptionInput[j, 2]);
                    if (ACPConsumptionInput[j, i] == null) ACPConsumptionInput[j, i] = 0.0;
                    lineUnit.ACPConsumption = (double)ACPConsumptionInput[j, i];
                }
            }
        }
        private void UpdateACP54Consumption()
        {
            Object[,] ACP54ConsumptionInput = configDico[Legende.ROCKCONSO];

            for (int i = 3; i < ACP54ConsumptionInput.GetLength(1); i++)
            {
                CapexObjectManager capexObj = objectManagerDico[ACP54ConsumptionInput[0, i].ToString()];
                capexObj.UpdateLinesDico();

                for (int j = 1; j < ACP54ConsumptionInput.GetLength(0); j++)
                {
                    Unit lineUnit = capexObj.GetLine((string)ACP54ConsumptionInput[j, 2]);
                    if (ACP54ConsumptionInput[j, i] == null) ACP54ConsumptionInput[j, i] = 0.0;
                    lineUnit.ACP54Consumption = (double)ACP54ConsumptionInput[j, i];
                }
            }
        }
        private void UpdateNH3Consumption()
        {
            Object[,] NH3ConsumptionInput = configDico[Legende.NH3CONSO];

            for (int i = 3; i < NH3ConsumptionInput.GetLength(1); i++)
            {
                CapexObjectManager capexObj = objectManagerDico[NH3ConsumptionInput[0, i].ToString()];
                capexObj.UpdateLinesDico();

                for (int j = 1; j < NH3ConsumptionInput.GetLength(0); j++)
                {
                    Unit lineUnit = capexObj.GetLine((string)NH3ConsumptionInput[j, 2]);
                    if (NH3ConsumptionInput[j, i] == null) NH3ConsumptionInput[j, i] = 0.0;
                    lineUnit.NH3Consumption = (double)NH3ConsumptionInput[j, i];
                }
            }
        }
        private void UpdateSConsumption()
        {
            Object[,] SConsumptionInput = configDico[Legende.SCONSO];

            for (int i = 3; i < SConsumptionInput.GetLength(1); i++)
            {
                CapexObjectManager capexObj = objectManagerDico[SConsumptionInput[0, i].ToString()];
                capexObj.UpdateLinesDico();

                for (int j = 1; j < SConsumptionInput.GetLength(0); j++)
                {
                    Unit lineUnit = capexObj.GetLine((string)SConsumptionInput[j, 2]);
                    if (SConsumptionInput[j, i] == null) SConsumptionInput[j, i] = 0.0;
                    lineUnit.SConsumption = (double)SConsumptionInput[j, i];
                }
            }
        }
        private void UpdateCosts()
        {
            Object[,] costsInput = configDico[Legende.COSTS];

            for (int i = 3; i < costsInput.GetLength(1); i++)
            {
                CapexObjectManager capexObj = objectManagerDico[costsInput[0, i].ToString()];
                capexObj.UpdateLinesDico();

                for (int j = 1; j < costsInput.GetLength(0); j++)
                {
                    Unit lineUnit = capexObj.GetLine((string)costsInput[j, 2]);
                    if (costsInput[j, i] == null) costsInput[j, i] = 0.0;
                    lineUnit.Cost = (double)costsInput[j, i];
                }
            }
        }
    }
}