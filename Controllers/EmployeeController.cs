using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using KernelCars.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KernelCars.Models;

namespace KernelCars.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DataContext _context;

        public int PageSize = 100;

        public EmployeeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return NotFound();
            }

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), @"wwwroot/InFiles",
                file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            DataTable dataTable = ReadExcelasJSON(path);

            string[] excludeNames = new string[]
            {
                "ПП",
                "ТОВ",
                "ПСП",
                "СТОВ",
                "ХПП",
                "ПРАТ",
                "Спілка",
                "Орендодавці"
            }; 


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                try
                {
                    bool isFirm = false;
                    foreach (var item in excludeNames)
                    {
                        if (((string)dataTable.Rows[i][0]).IndexOf(item)>-1)
                        {
                            isFirm = true;
                        }
                    }

                    if (!isFirm)
                    {
                        string[] fio = ((string)dataTable.Rows[i][0]).Split(" ");

                        if (fio.Length == 3)
                        {
                            var emp = (from e in _context.Employees
                                       where e.LastName.Trim().ToUpper() == fio[0].Trim().ToUpper()
                                       && e.FirstName.Trim().ToUpper() == fio[1].Trim().ToUpper()
                                       && e.MiddleName.Trim().ToUpper() == fio[2].Trim().ToUpper()
                                       select e).Count();
                            if (emp == 0)
                            {
                                Employee em = new Employee
                                {
                                    FirstName = fio[1],
                                    LastName = fio[0],
                                    MiddleName = fio[2]
                                };
                                _context.Employees.Add(em);
                            }
                        }
                    }
                    else
                    {
                        var emp = (from e in _context.Employees
                                   where e.LastName.Replace(" ", "").ToUpper() == ((string)dataTable.Rows[i][0]).Replace(" ", "").ToUpper()
                                   select e).Count();
                        if (emp==0)
                        {
                            Employee em = new Employee
                            {
                                LastName = ((string)dataTable.Rows[i][0]).ToUpper()
                            };
                            _context.Employees.Add(em);
                        }
                    }

                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    //throw;
                }
            }
           // _context.SaveChanges();
            return RedirectToAction("Index");
        }

        static DataTable ReadExcelasJSON(string fileName)
        {
            DataTable dtTable = new DataTable();

            try
            {
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fileName, false))
                {
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();

                    string sheetId = "";
                    foreach (Sheet thesheet in thesheetcollection)
                    {
                        if (thesheet.Name == "Исходник")
                        {
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
                    IEnumerable<Row> rows = thesheetdata.Descendants<Row>();

                    //Выделяет все символы из строки
                    Regex regex = new Regex(@"\D+");

                    //Выделяет все цифры из строки
                    Regex regex1 = new Regex(@"\d+");

                    //Поиск колонки с признаком классификатора ОС
                    string[] rowHeaders = new string[] { "Власник/Орендодавець" };

                    int? startRowIndex = null;
                    int ozColumnIndex = 0;

                    //string[] vechicleType = new string[] { 
                    //    "АТП-2004 ТОВ".Replace(" ",""), 
                    //    "ТОВ Кернел-Трейд".Replace(" ", ""), 
                    //    "ПРАТ Кропивницький ОЕЗ".Replace(" ", ""), 
                    //    "СТОВ Дружба - Нова".Replace(" ", ""),
                    //    "СТОВ Придніпровський край".Replace(" ", ""),
                    //    "Коритня-Агро КМ    ТОВ".Replace(" ", ""),
                    //    "Приколотнянский МЭЗ ТОВ".Replace(" ", ""),
                    //    "Агро Інвест Україна, ТОВ".Replace(" ", ""),
                    //    "СТОВ Дружба-Нова".Replace(" ", ""),
                    //    "Вовчанський ОЕЗ ПРАТ".Replace(" ", ""),
                    //    "Юнігрейн-Агро ТОВ, Семенівка".Replace(" ", ""),
                    //    "Вовчанський ОЕЗ ПРАТ".Replace(" ", "")
                    //};

                    List<string> passengerCellValue = new List<string>();
                    List<int> rowsToProc = new List<int>();


                    //Поиск первой строки с данными
                    for (int rCnt = 0; rCnt < thesheetdata.ChildElements.Count(); rCnt++)
                    {
                        for (int rCnt1 = 0; rCnt1 < thesheetdata.ElementAt(rCnt).ChildElements.Count(); rCnt1++)
                        {
                            Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(rCnt1);

                            int id;
                            if (Int32.TryParse(thecurrentcell.InnerText, out id))
                            {
                                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                if (item.Text != null)
                                {
                                    if (Array.IndexOf(rowHeaders, item.Text.Text) > -1)
                                    {
                                        startRowIndex = rCnt;
                                        ozColumnIndex = rCnt1;
                                        //rowsToProc.Add(rCnt);
                                        //passengerCellValue = thecurrentcell.InnerText;
                                        break;
                                    }
                                }
                            }
                        }
                        if (startRowIndex != null)
                        {
                            break;
                        }
                    }




                    //rowHeaders = new string[] { "Кластер", "Назва підприємства", "Назва основного засобу", "Класифікатор ОЗ", "Дата випуску", "Державний номер", "Стан ОЗ", "Модель" };

                    rowHeaders = new string[] { "Власник/Орендодавець" };
                    

                    SharedStringTablePart stringTablePart = workbookPart.SharedStringTablePart;
                    List<int> columns = new List<int>();

                    //Поиск номеров колонок для получения данных
                    int idx = 0;
                    foreach (Cell c in rows.ElementAt((int)startRowIndex).Elements<Cell>())
                    {

                        if (c.CellValue != null)
                        {
                            if (c.DataType != null && c.DataType.Value == CellValues.SharedString)
                            {
                                if (Array.IndexOf(rowHeaders, stringTablePart.SharedStringTable.ChildElements[Int32.Parse(c.CellValue.InnerXml)].InnerText) > -1)
                                {
                                    columns.Add(idx);// regex.Match(c.CellReference).Value);
                                }
                            }
                        }
                        idx++;
                    }



                    for (int rCnt = 0; rCnt < thesheetdata.ChildElements.Count(); rCnt++)
                    {
                        try
                        {
                            Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(ozColumnIndex);

                            int id;
                            if (Int32.TryParse(thecurrentcell.InnerText, out id))
                            {
                                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                //if (Array.IndexOf(vechicleType.ToArray(), item.Text.Text.Replace("\"", "").Replace(" ", "")) > -1 && item.Text.Text.IndexOf("Дружба")>-1)
                                //{
                                //    int fkjgk = 0;
                                //    //rowsToProc.Add(rCnt);
                                //    //passengerCellValue.Add(thecurrentcell.InnerText);
                                //}

                                //if (Array.IndexOf(vechicleType.ToArray(), item.Text.Text.Replace("\"","").Replace(" ", "")) < 0)
                                //{
                                    rowsToProc.Add(rCnt);
                                    passengerCellValue.Add(thecurrentcell.InnerText);
                                //}
                            }
                        }
                        catch (Exception)
                        {
                            //throw;
                        }

                    }


                    for (int rCnt = 0; rCnt < rowsToProc.Count(); rCnt++)
                    {
                        List<string> rowList = new List<string>();

                        for (int c = 0; c < columns.Count(); c++)
                        {
                            Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rowsToProc[rCnt]).ChildElements.ElementAt(columns[c]);
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
                                            if (rCnt == 0)
                                            {
                                                dtTable.Columns.Add(item.Text.Text);
                                            }
                                            else
                                            {
                                                rowList.Add(item.Text.Text);
                                            }
                                        }
                                        else
                                        {
                                            rowList.Add("");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (rCnt != 0)//reserved for column values
                                {
                                    rowList.Add(thecurrentcell.InnerText);
                                }
                            }
                        }
                        if (rCnt != 0)//reserved for column values
                        {
                            dtTable.Rows.Add(rowList.ToArray());
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return dtTable;
        }
    }
}