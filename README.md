# Graduation_Project

// Summary
This project was done as part of my engineering graduation internship at OCP Group (http://www.ocpgroup.ma/en) in Morocco. It integrates all the major elements of the company's production chain (Mines, Logistics, Products, Production Lines, Capacities, ...) for a better manipulation. This project gave the CAPEX Planing team the possibility for more interaction instead of their first black box system. It also gives an instant calculation for the optimal productions instead of the 10~20 min with Excel-VBA.


// How to run the code?
1- Open CapexLibrary Folder, then open CapexLibrary.sln project.
2- Run the project and wait for an Excel to run automatically then, in Excel, open the file Capex_Planning.xlsm.
3- Go to Sheet "Config", then enter the funtion "=CAPEXLoadConfig(A2:C13)" in the test upload. If the test result is "True", then all the inputs (all the excel sheets) have been successfully loaded to the code and waiting for the process of calculation.
4- Finally, Go to the sheet "Run Chimie", select all the table and choose which solver you want to use, then click enter.


//What Happened?
The code, through a "Controller", load all the tables on the different requested sheets of the Excel file. It transform them to C# Objects, and then, according to what is requested from it, set the optimization problem. After setting the optimization problem (which we can read and verify through text files named Facility_LpFile_2020), the code calls the Optimization solver, through the SwitcherLibrary to solve it and give outputs in form of Excel table on the original Excel File.
