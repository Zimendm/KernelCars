using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KernelCars.Models;
using KernelCars.Data;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using DocumentFormat.OpenXml;

namespace KernelCars.Controllers
{
    public class CarController : Controller
    {
        private readonly DataContext _context;
        public CarController(DataContext context)
        {
            _context = context;
        }
        public string Index()
            //public IActionResult Index()
        {

            string str = ReadExcelasJSON();











            //using (SpreadsheetDocument doc = SpreadsheetDocument.Open(@"C:\Users\dzime\Source\Repos\Zimendm\KernelCars\wwwroot\InFiles\Cars.xlsx", false))
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(@"C:\Users\dzime\Source\Repos\Zimendm\KernelCars\wwwroot\InFiles\Автотранспорт.xlsx", false))
            {
                WorkbookPart workbookPart = doc.WorkbookPart;
                Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                StringBuilder excelResult = new StringBuilder();



                //using for each loop to get the sheet from the sheetcollection  
                foreach (Sheet thesheet in thesheetcollection)
                {
                    if (thesheet.Name=="Исходник")
                    {

                   
                    excelResult.AppendLine("Excel Sheet Name : " + thesheet.Name);
                    excelResult.AppendLine("----------------------------------------------- ");
                    //statement to get the worksheet object by using the sheet id  
                    Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;

                    SheetData thesheetdata = (SheetData)theWorksheet.GetFirstChild<SheetData>();
                    
                    foreach (Row thecurrentrow in thesheetdata)
                    {
                        foreach (Cell thecurrentcell in thecurrentrow)
                        {
                            //statement to take the integer value  
                            string currentcellvalue = string.Empty;
                            if (thecurrentcell.DataType != null)
                            {
                                if (thecurrentcell.DataType == CellValues.SharedString)
                                {
                                    int id;
                                    if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                    {
                                        SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                        if (item.Text != null)
                                        {
                                            //code to take the string value  
                                            excelResult.AppendLine(item.Text.Text + " ");
                                        }
                                        else if (item.InnerText != null)
                                        {
                                            currentcellvalue = item.InnerText;
                                        }
                                        else if (item.InnerXml != null)
                                        {
                                            currentcellvalue = item.InnerXml;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //excelResult.Append(Convert.ToInt16(thecurrentcell.InnerText) + " ");
                            }
                        }
                        excelResult.AppendLine();
                    }
                    excelResult.Append("");


                    }
                    int shdsghds = 0;
                }















           ////////////////         int z = 0;


           ////////////////     List<Car> persons = new List<Car>()
           //////////////// {
           ////////////////     new Car {Id=1,  RegistrationNumber="11", VinNumber="111"},
           ////////////////     new Car {Id=2,  RegistrationNumber="22", VinNumber="222"}

           ////////////////};

           ////////////////     // Lets converts our object data to Datatable for a simplified logic.
           ////////////////     // Datatable is most easy way to deal with complex datatypes for easy reading and formatting. 
           ////////////////     DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(persons), (typeof(DataTable)));

           ////////////////     using (SpreadsheetDocument document = SpreadsheetDocument.Create(@"C:\Users\dzime\Source\Repos\Zimendm\KernelCars\wwwroot\OutFiles\CarsOut.xlsx", SpreadsheetDocumentType.Workbook))
           ////////////////     {
           ////////////////         WorkbookPart workbookPart1 = document.AddWorkbookPart();
           ////////////////         workbookPart1.Workbook = new Workbook();

           ////////////////         WorksheetPart worksheetPart = workbookPart1.AddNewPart<WorksheetPart>();
           ////////////////         var sheetData = new SheetData();
           ////////////////         worksheetPart.Worksheet = new Worksheet(sheetData);

           ////////////////         Sheets sheets = workbookPart1.Workbook.AppendChild(new Sheets());
           ////////////////         Sheet sheet = new Sheet() { Id = workbookPart1.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

           ////////////////         sheets.Append(sheet);

           ////////////////         Row headerRow = new Row();

           ////////////////         List<String> columns = new List<string>();
           ////////////////         foreach (System.Data.DataColumn column in table.Columns)
           ////////////////         {
           ////////////////             columns.Add(column.ColumnName);

           ////////////////             Cell cell = new Cell();
           ////////////////             cell.DataType = CellValues.String;
           ////////////////             cell.CellValue = new CellValue(column.ColumnName);
           ////////////////             headerRow.AppendChild(cell);
           ////////////////         }

           ////////////////         sheetData.AppendChild(headerRow);

           ////////////////         foreach (DataRow dsrow in table.Rows)
           ////////////////         {
           ////////////////             Row newRow = new Row();
           ////////////////             foreach (String col in columns)
           ////////////////             {
           ////////////////                 Cell cell = new Cell();
           ////////////////                 cell.DataType = CellValues.String;
           ////////////////                 cell.CellValue = new CellValue(dsrow[col].ToString());
           ////////////////                 newRow.AppendChild(cell);
           ////////////////             }

           ////////////////             sheetData.AppendChild(newRow);
           ////////////////         }

           ////////////////         workbookPart1.Workbook.Save();
           ////////////////     }





















                return excelResult.ToString();
            }





                //var cars = _context.Cars.Include(c => c.CarModel).ThenInclude(c => c.Manufacturer);
            //return View(cars.ToList());
            
        }




        static string ReadExcelasJSON()
        {
            try
            {
                DataTable dtTable = new DataTable();
                //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(@"C:\Users\dzime\Source\Repos\Zimendm\KernelCars\wwwroot\InFiles\Автотранспорт.xlsx", false))
                {
                    //create the object for workbook part  
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();

                    string sheetId = "";
                    foreach (Sheet thesheet in thesheetcollection)
                    {
                        if (thesheet.Name == "Исходник")
                        {


                            //excelResult.AppendLine("Excel Sheet Name : " + thesheet.Name);
                            //excelResult.AppendLine("----------------------------------------------- ");
                            ////statement to get the worksheet object by using the sheet id  
                            //Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;
                            sheetId = thesheet.Id;
                            break;
                        }
                    }



                            //using for each loop to get the sheet from the sheetcollection  
                    //foreach (Sheet thesheet in thesheetcollection.OfType<Sheet>())
                    //{
                        //statement to get the worksheet object by using the sheet id  
                        //Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;
                        Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(sheetId)).Worksheet;


                        SheetData thesheetdata = theWorksheet.GetFirstChild<SheetData>();



                        for (int rCnt = 4; rCnt < thesheetdata.ChildElements.Count()-4; rCnt++)
                        {
                            List<string> rowList = new List<string>();

                        int[] columns = new int[] { 1,2,4,6,9,12,13,15,20,21,31,32,33,34,36,41 };

                        //foreach (var item in collection)
                        //{

                        //}
                        for (int c = 0; c < columns.Count(); c++)
                            //for (int rCnt1 = 0; rCnt1 < thesheetdata.ElementAt(rCnt).ChildElements.Count()-10; rCnt1++)
                            {

                            //Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(rCnt1);
                            Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(columns[c]);
                            //statement to take the integer value  
                            string currentcellvalue = string.Empty;
                                if (thecurrentcell.DataType != null)
                                {
                                    if (thecurrentcell.DataType == CellValues.SharedString)
                                    {
                                        int id;
                                        if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                        {
                                            SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                            if (item.Text != null)
                                            {
                                                //first row will provide the column name.
                                                if (rCnt == 4)
                                                {
                                                    dtTable.Columns.Add(item.Text.Text);
                                                }
                                                else
                                                {
                                                    rowList.Add(item.Text.Text);
                                                }
                                            }
                                            else if (item.InnerText != null)
                                            {
                                                currentcellvalue = item.InnerText;
                                            }
                                            else if (item.InnerXml != null)
                                            {
                                                currentcellvalue = item.InnerXml;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (rCnt != 4)//reserved for column values
                                    {
                                        rowList.Add(thecurrentcell.InnerText);
                                    }
                                }
                            }
                            if (rCnt != 4)//reserved for column values
                            {
                            if (rowList[4] == "Легкові")
                            {
                                dtTable.Rows.Add(rowList.ToArray());
                            }
                        }
                        }

                    //}

                    return JsonConvert.SerializeObject(dtTable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}