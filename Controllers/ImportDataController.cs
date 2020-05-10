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
using KernelCars.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KernelCars.Controllers
{
    public class ImportDataController : Controller
    {
        private readonly DataContext _context;

        public ImportDataController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CarYears()
        {
            return View();
        }

        public IActionResult ReadKT()
        {
            return View();
        }

        [HttpPost]
        public void ReadKT(IFormFile file)
        {
            var path = Path.Combine(
              Directory.GetCurrentDirectory(), @"wwwroot/InFiles",
              file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo (stream);
            }

            DataTable dataTable = ReadExcelKT(path);

            for (int i = 1; i < dataTable.Rows.Count - 1; i++)
            {
                string[] fio = ((string)dataTable.Rows[i]["ФИО Водителя"]).Split(' ');

                // Проверка водителей
                var emplCount = (from e in _context.Employees
                                 where e.LastName.ToLower() == fio[0].ToLower() && e.FirstName.ToLower() == fio[1].ToLower() && e.MiddleName.ToLower() == fio[2].ToLower()
                                 select e).FirstOrDefault();
                if (emplCount == null)
                {
                    Employee emp = new Employee
                    {
                        LastName = fio[0],
                        FirstName = fio[1],
                        MiddleName = fio[2]
                    };
                    _context.Employees.Add(emp);
                }
            }
            _context.SaveChanges();

            for (int i = 1; i < dataTable.Rows.Count - 1; i++)
            {
                string regNumber = ((string)dataTable.Rows[i][0]).CheckNumber();


                var car = _context.Cars.Include(c=>c.CarUsers).ThenInclude(c=>c.Employee).ToList().Where(c=>c.RegistrationNumber.CheckNumber() == regNumber).ToList();

                if (car.Count!=0)
                {
                    string[] fio = ((string)dataTable.Rows[i]["ФИО Водителя"]).Split(' ');
                    
                    if (fio.Length == 3)
                    {
                        var emp = _context.Employees.Where(e => e.LastName.ToLower() == fio[0].ToLower() && e.FirstName.ToLower() == fio[1].ToLower() && e.MiddleName.ToLower() == fio[2].ToLower()).First();
                        if (emp!=null)
                        {
                            if (car[0].CarUsers.Count == 0)
                            {
                                car[0].CarUsers.Add(new CarUser
                                {
                                    StartUsingDate = DateTime.Now,
                                    Employee = emp
                                });
                            }
                            else
                            {
                                var lastDriver = from d in car[0].CarUsers
                                                  where d.EndUsingDate == null
                                                  select d;
                                if (lastDriver != null)
                                {
                                    if (lastDriver.Count() == 1)
                                    {
                                        var curdriver = lastDriver.First();
                                        if (curdriver.Employee.LastName.ToLower() != fio[0].ToLower() || curdriver.Employee.FirstName.ToLower() != fio[1].ToLower() || curdriver.Employee.MiddleName.ToLower() != fio[2].ToLower())
                                        {
                                            curdriver.EndUsingDate = DateTime.Now.AddMinutes(-10);
                                            car[0].CarUsers.Add(new CarUser
                                            {
                                                StartUsingDate = DateTime.Now,
                                                Employee = emp
                                            });
                                        }
                                    }
                                    else if (lastDriver.Count() > 1)
                                    {
                                        foreach (var item in lastDriver)
                                        {
                                            item.EndUsingDate = DateTime.Now.AddMinutes(-10);
                                        }
                                        car[0].CarUsers.Add(new CarUser
                                        {
                                            StartUsingDate = DateTime.Now,
                                            Employee = emp
                                        });
                                    }
                                    else
                                    {
                                        car[0].CarUsers.Add(new CarUser
                                        {
                                            StartUsingDate = DateTime.Now,
                                            Employee = emp
                                        });
                                    }
                                }
                                else
                                {
                                    car[0].CarUsers.Add(new CarUser
                                    {
                                        StartUsingDate = DateTime.Now,
                                        Employee = emp
                                    });
                                }



                                //if (lastDriver!=null)
                                //{


                                //}

                                //car[0].CarUsers[car[0].CarUsers.Count - 1].EndUsingDate = DateTime.Now.AddHours(-1);
                                //car[0].CarUsers.Add(new CarUser
                                //{
                                //    StartUsingDate = DateTime.Now,
                                //    Employee = emp
                                //});
                            }

                            //var driver = car. (from d in _context.CarUsers.Include(c => c.Employee)
                            //              where d.CarId == car.First().Id && d.EndUsingDate == null
                            //              select d).LastOrDefault();
                            //if (driver != null)
                            //{
                            //    int zq = 0;
                            //}
                            //else
                            //{
                           
                            int zq = 1;
                            //}
                        }
                    }

                        

                    int j = 0;
                }
                    //(from c in _context.Cars
                    //       where c.RegistrationNumber.CheckNumber() == regNumber
                    //       select c).FirstOrDefault();
                
                //if (car!=null)
                //{
                //    string[] fio = ((string)dataTable.Rows[i]["ФИО Водителя"]).Split(' ');
                //    if (fio.Length == 3)
                //    {
                //        var driver = (from d in _context.CarUsers.Include(c => c.Employee)
                //                      where d.CarId == car.Id && d.EndUsingDate == null
                //                      select d).LastOrDefault();
                //        if (driver != null)
                //        {
                //            int zq = 0;
                //        }
                //    }

                //}
            }
            _context.SaveChanges();

            //

            //if (car!=null)
            //{

            //    if (fio.Length==3)
            //    {
            //        var driver = (from d in _context.CarUsers.Include(c=>c.Employee)
            //                 where d.CarId == car.Id && d.EndUsingDate == null
            //                 select d).LastOrDefault();

            //        if (driver!=null)
            //        {
            //            if (driver.Employee.LastName.ToLower()!=fio[0].ToLower() || driver.Employee.FirstName.ToLower() != fio[1].ToLower() || driver.Employee.LastName.ToLower() != fio[2].ToLower())
            //            {
            //                driver.EndUsingDate = DateTime.Now.AddMinutes(-10);
            //                CarUser carUser = new CarUser
            //                {
            //                    StartUsingDate = DateTime.Now

            //                };
            //            }
            //        }
            //        else
            //        {


            //        }
            //    }
            //}
            //  }


            int z = 0;

        }

        [HttpPost]
        public async Task<IActionResult> CarYears(IFormFile file)
        {
            var path = Path.Combine(
               Directory.GetCurrentDirectory(), @"wwwroot/InFiles",
               file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            DataTable dataTable = ReadExcelasJSONYears(path);


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string number = dataTable.Rows[i]["Number"].ToString().CheckNumber();


                var car = _context.Cars.Where(c => c.RegistrationNumber == number).FirstOrDefault();
                if (car!=null)
                {
                    if (car.FirstRegistrationYear == 1917)
                    {
                        int y;
                        if (Int32.TryParse(dataTable.Rows[i]["Year"].ToString(), out y))
                        {
                            car.FirstRegistrationYear = y;
                        }
                        if (car.VinNumber=="")
                        {
                            try
                            {
                                car.VinNumber = dataTable.Rows[i]["Vin"].ToString();
                            }
                            catch (Exception)
                            {

                                //throw;
                            }
                            
                        }
                    }
                }

                //string ksjdk = dataTable.Rows[i]["Year"].ToString();


            }

            _context.SaveChanges();
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
            
            UpdateEmployees(_context,dataTable);



            UpdateCars(dataTable, 0);
            //UpdateCars(dataTable);            


            return RedirectToAction("Index");
        }

        private int CheckOwnerID(string empName)
        {
            int id = 1;

            //Замена нескольких пробелов на один
            Regex r = new Regex(@"\s+");
            empName = r.Replace(empName, @" ");


            empName = empName.Trim().ToUpper();
            
            var owner = from o in _context.Employees
                        where o.LastName.Trim().ToUpper() == empName
                        select o;

            if (owner.Count() == 1)
            {
                id = owner.First().Id;
            }
            else
            {
                string[] fio = empName.Split(" ");

                if (fio.Length == 3)
                {
                    var emp = from e in _context.Employees
                              where e.LastName.Trim().ToUpper() == fio[0].Trim().ToUpper()
                              && e.FirstName.Trim().ToUpper() == fio[1].Trim().ToUpper()
                              && e.MiddleName.Trim().ToUpper() == fio[2].Trim().ToUpper()
                              select e;
                    if (emp.Count () == 1)
                    {
                        id = emp.First().Id;
                    }
                }
            }
            return id;
        }

        static DataTable ReadExcelKT(string fileName)
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

                    Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(sheetId)).Worksheet;
                    SheetData thesheetdata = theWorksheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = thesheetdata.Descendants<Row>();

                    //Выделяет все символы из строки
                    Regex regex = new Regex(@"\D+");

                    //Выделяет все цифры из строки
                    Regex regex1 = new Regex(@"\d+");

                    //Поиск колонки с признаком классификатора ОС
                    string[] rowHeaders = new string[] { "Код" };

                    int? startRowIndex = null;
                    int ozColumnIndex = 0;

                    //string[] vechicleType = new string[] { "Легкові", "Автобуси", "Вантажопасажирські" };

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
                                        rowsToProc.Add(rCnt);
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




                    rowHeaders = new string[] { "Гос. номер ТС", "ФИО Водителя" };
                    SharedStringTablePart stringTablePart = workbookPart.SharedStringTablePart;
                    List<string> columns = new List<string>();

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
                                    columns.Add(regex.Match(c.CellReference).Value);// regex.Match(c.CellReference).Value);
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
                                //if (Array.IndexOf(vechicleType.ToArray(), item.Text.Text) > -1)
                                //{
                                rowsToProc.Add(rCnt);
                                //passengerCellValue.Add(thecurrentcell.InnerText);
                                //}
                            }
                        }
                        catch (Exception)
                        {
                            //throw;
                        }

                    }

                    //for (int rCnt = 0; rCnt < thesheetdata.ChildElements.Count(); rCnt++)
                    //{
                    //    try
                    //    {
                    //        Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(ozColumnIndex);
                    //        //if (thecurrentcell.InnerText == passengerCellValue)
                    //        //    {
                    //        //    rowsToProc.Add(rCnt);  
                    //        //    }
                    //    }
                    //    catch (Exception)
                    //    {
                    //        //throw;
                    //    }

                    //}




                    //int kejkej = 0;
                    //if (c.CellValue != null)
                    //{
                    //    if (c.DataType != null && c.DataType.Value == CellValues.SharedString)
                    //    {
                    //        if (Array.IndexOf(rowHeaders, stringTablePart.SharedStringTable.ChildElements[Int32.Parse(c.CellValue.InnerXml)].InnerText) > -1)
                    //        {
                    //            columns.Add(stringTablePart.SharedStringTable.ChildElements[Int32.Parse(c.CellValue.InnerXml)].InnerText, regex.Match(c.CellReference).Value);
                    //        }
                    //    }
                    //}














                    for (int rCnt = 0; rCnt < rowsToProc.Count(); rCnt++)
                    {
                        try
                        {
                            if (rCnt == 1826)
                            {
                                int lskdls = 0;
                            }


                            List<string> rowList = new List<string>();


                            //for (int i = 0; i < thesheetdata.ElementAt(rowsToProc[rCnt]).Descendants<Cell>().Count(); i++)
                            //{
                            //    Cell cell = thesheetdata.ElementAt(rowsToProc[rCnt]).Descendants<Cell>().ElementAt(i);
                            //    int actualCellIndex = CellReferenceToIndex(cell);
                            //    tempRow[actualCellIndex] = GetCellValue(spreadSheetDocument, cell);
                            //}


                            // for (int c = 0; c < columns.Count(); c++)
                            for (int c = 0; c < columns.Count(); c++)
                            {
                                Cell cell = thesheetdata.ElementAt(rowsToProc[0]).Descendants<Cell>().ElementAt(0);//columns[c]);
                                int actualCellIndex = CellReferenceToIndex(cell);

                                int? actualCellIdx = rows.ElementAt(rowsToProc[rCnt]).GetSellIndexFromColumnLetter(columns[c]);



                                //thesheetdata.ElementAt(rowsToProc[rCnt])

                                if (actualCellIdx == null)
                                {
                                    rowList.Add("");
                                    continue;
                                }



                                Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rowsToProc[rCnt]).ChildElements.ElementAt((int)actualCellIdx);
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
                                    else if (thecurrentcell.DataType == CellValues.Number)
                                    {
                                        rowList.Add(thecurrentcell.CellValue.InnerText);
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
                        catch (Exception)
                        {
                            int hhh = 0;
                            //throw;
                        }

                    }
                    //  return JsonConvert.SerializeObject(dtTable);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dtTable;
        }

        static DataTable ReadExcelasJSONYears(string fileName)
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

                    Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(sheetId)).Worksheet;
                    SheetData thesheetdata = theWorksheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = thesheetdata.Descendants<Row>();

                    //Выделяет все символы из строки
                    Regex regex = new Regex(@"\D+");

                    //Выделяет все цифры из строки
                    Regex regex1 = new Regex(@"\d+");

                    //Поиск колонки с признаком классификатора ОС
                    string[] rowHeaders = new string[] { "Vin" };

                    int? startRowIndex = null;
                    int ozColumnIndex = 0;

                    string[] vechicleType = new string[] { "Легкові", "Автобуси", "Вантажопасажирські" };

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
                                        rowsToProc.Add(rCnt);
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

                    //for (int rCnt = (int) startRowIndex; rCnt  < rows.Count()- startRowIndex; rCnt ++)
                    //{
                    //    List<string> rowList = new List<string>();

                    //    for (int cCnt = 0; cCnt < thesheetdata.ElementAt(rCnt).ChildElements.Count(); cCnt++)
                    //    {
                    //        Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(cCnt);
                    //        if (thecurrentcell.DataType!=null)
                    //        {
                    //            if (thecurrentcell.DataType==CellValues.SharedString)
                    //            {
                    //                int id;
                    //                if (Int32.TryParse(thecurrentcell.InnerText,out id))
                    //                {
                    //                    SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                    //                    if (item.Text!=null)
                    //                    {
                    //                        if (rCnt==startRowIndex)
                    //                        {
                    //                            dt.Columns.Add(item.Text.Text);
                    //                        }
                    //                        else
                    //                        {
                    //                            rowList.Add(item.Text.Text);
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        rowList.Add("");
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        if (rCnt!=startRowIndex)
                    //        {
                    //            dt.Rows.Add(rowList.ToArray());
                    //        }
                    //    }


                    //}


                    //return dt;










                    rowHeaders = new string[] { "Number", "Vin", "Year" };
                    SharedStringTablePart stringTablePart = workbookPart.SharedStringTablePart;
                    List<string> columns = new List<string>();

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
                                    columns.Add(regex.Match(c.CellReference).Value);// regex.Match(c.CellReference).Value);
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
                                //if (Array.IndexOf(vechicleType.ToArray(), item.Text.Text) > -1)
                                //{
                                rowsToProc.Add(rCnt);
                                //passengerCellValue.Add(thecurrentcell.InnerText);
                                //}
                            }
                        }
                        catch (Exception)
                        {
                            //throw;
                        }

                    }

                    //for (int rCnt = 0; rCnt < thesheetdata.ChildElements.Count(); rCnt++)
                    //{
                    //    try
                    //    {
                    //        Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(ozColumnIndex);
                    //        //if (thecurrentcell.InnerText == passengerCellValue)
                    //        //    {
                    //        //    rowsToProc.Add(rCnt);  
                    //        //    }
                    //    }
                    //    catch (Exception)
                    //    {
                    //        //throw;
                    //    }

                    //}




                    //int kejkej = 0;
                    //if (c.CellValue != null)
                    //{
                    //    if (c.DataType != null && c.DataType.Value == CellValues.SharedString)
                    //    {
                    //        if (Array.IndexOf(rowHeaders, stringTablePart.SharedStringTable.ChildElements[Int32.Parse(c.CellValue.InnerXml)].InnerText) > -1)
                    //        {
                    //            columns.Add(stringTablePart.SharedStringTable.ChildElements[Int32.Parse(c.CellValue.InnerXml)].InnerText, regex.Match(c.CellReference).Value);
                    //        }
                    //    }
                    //}














                    for (int rCnt = 0; rCnt < rowsToProc.Count(); rCnt++)
                    {
                        try
                        {
                            if (rCnt == 1826)
                            {
                                int lskdls = 0;
                            }


                            List<string> rowList = new List<string>();


                            //for (int i = 0; i < thesheetdata.ElementAt(rowsToProc[rCnt]).Descendants<Cell>().Count(); i++)
                            //{
                            //    Cell cell = thesheetdata.ElementAt(rowsToProc[rCnt]).Descendants<Cell>().ElementAt(i);
                            //    int actualCellIndex = CellReferenceToIndex(cell);
                            //    tempRow[actualCellIndex] = GetCellValue(spreadSheetDocument, cell);
                            //}


                            // for (int c = 0; c < columns.Count(); c++)
                            for (int c = 0; c < columns.Count(); c++)
                            {
                                Cell cell = thesheetdata.ElementAt(rowsToProc[0]).Descendants<Cell>().ElementAt(0);//columns[c]);
                                int actualCellIndex = CellReferenceToIndex(cell);

                                int? actualCellIdx = rows.ElementAt(rowsToProc[rCnt]).GetSellIndexFromColumnLetter(columns[c]);



                                //thesheetdata.ElementAt(rowsToProc[rCnt])

                                if (actualCellIdx == null)
                                {
                                    rowList.Add("");
                                    continue;
                                }



                                Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rowsToProc[rCnt]).ChildElements.ElementAt((int)actualCellIdx);
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
                                    else if (thecurrentcell.DataType == CellValues.Number)
                                    {
                                        rowList.Add(thecurrentcell.CellValue.InnerText);
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
                        catch (Exception)
                        {
                            int hhh = 0;
                            //throw;
                        }

                    }
                    //  return JsonConvert.SerializeObject(dtTable);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dtTable;
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
                    string[] rowHeaders = new string[] { "Класифікатор ОЗ" };

                    int? startRowIndex = null;
                    int ozColumnIndex = 0;

                    string[] vechicleType = new string[] { "Легкові", "Автобуси", "Вантажопасажирські" };

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
                                        rowsToProc.Add(rCnt);
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

                    //for (int rCnt = (int) startRowIndex; rCnt  < rows.Count()- startRowIndex; rCnt ++)
                    //{
                    //    List<string> rowList = new List<string>();

                    //    for (int cCnt = 0; cCnt < thesheetdata.ElementAt(rCnt).ChildElements.Count(); cCnt++)
                    //    {
                    //        Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(cCnt);
                    //        if (thecurrentcell.DataType!=null)
                    //        {
                    //            if (thecurrentcell.DataType==CellValues.SharedString)
                    //            {
                    //                int id;
                    //                if (Int32.TryParse(thecurrentcell.InnerText,out id))
                    //                {
                    //                    SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                    //                    if (item.Text!=null)
                    //                    {
                    //                        if (rCnt==startRowIndex)
                    //                        {
                    //                            dt.Columns.Add(item.Text.Text);
                    //                        }
                    //                        else
                    //                        {
                    //                            rowList.Add(item.Text.Text);
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        rowList.Add("");
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        if (rCnt!=startRowIndex)
                    //        {
                    //            dt.Rows.Add(rowList.ToArray());
                    //        }
                    //    }


                    //}


                    //return dt;










                    rowHeaders = new string[] { "Назва підприємства", "Інв. №", "Заводський номер", "Класифікатор ОЗ", "Дата випуску", "Державний номер", "Модель", "М.В.О.", "Власник/Орендодавець" };
                    SharedStringTablePart stringTablePart = workbookPart.SharedStringTablePart;
                    List<string> columns = new List<string>();

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
                                    columns.Add(regex.Match(c.CellReference).Value);// regex.Match(c.CellReference).Value);
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
                                //if (Array.IndexOf(vechicleType.ToArray(), item.Text.Text) > -1)
                                //{
                                    rowsToProc.Add(rCnt);
                                    //passengerCellValue.Add(thecurrentcell.InnerText);
                                //}
                            }
                        }
                        catch (Exception)
                        {
                            //throw;
                        }

                    }

                    //for (int rCnt = 0; rCnt < thesheetdata.ChildElements.Count(); rCnt++)
                    //{
                    //    try
                    //    {
                    //        Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(ozColumnIndex);
                    //        //if (thecurrentcell.InnerText == passengerCellValue)
                    //        //    {
                    //        //    rowsToProc.Add(rCnt);  
                    //        //    }
                    //    }
                    //    catch (Exception)
                    //    {
                    //        //throw;
                    //    }

                    //}




                    //int kejkej = 0;
                    //if (c.CellValue != null)
                    //{
                    //    if (c.DataType != null && c.DataType.Value == CellValues.SharedString)
                    //    {
                    //        if (Array.IndexOf(rowHeaders, stringTablePart.SharedStringTable.ChildElements[Int32.Parse(c.CellValue.InnerXml)].InnerText) > -1)
                    //        {
                    //            columns.Add(stringTablePart.SharedStringTable.ChildElements[Int32.Parse(c.CellValue.InnerXml)].InnerText, regex.Match(c.CellReference).Value);
                    //        }
                    //    }
                    //}














                    for (int rCnt = 0; rCnt < rowsToProc.Count(); rCnt++)
                    {
                        try
                        {
                            if (rCnt==1826)
                            {
                                int lskdls = 0;
                            }


                            List<string> rowList = new List<string>();


                            //for (int i = 0; i < thesheetdata.ElementAt(rowsToProc[rCnt]).Descendants<Cell>().Count(); i++)
                            //{
                            //    Cell cell = thesheetdata.ElementAt(rowsToProc[rCnt]).Descendants<Cell>().ElementAt(i);
                            //    int actualCellIndex = CellReferenceToIndex(cell);
                            //    tempRow[actualCellIndex] = GetCellValue(spreadSheetDocument, cell);
                            //}


                            // for (int c = 0; c < columns.Count(); c++)
                            for (int c = 0; c < columns.Count(); c++)
                            {
                                Cell cell = thesheetdata.ElementAt(rowsToProc[0]).Descendants<Cell>().ElementAt(0);//columns[c]);
                                int actualCellIndex = CellReferenceToIndex(cell);

                                int? actualCellIdx = rows.ElementAt(rowsToProc[rCnt]).GetSellIndexFromColumnLetter(columns[c]);



                                //thesheetdata.ElementAt(rowsToProc[rCnt])

                                if (actualCellIdx==null)
                                {
                                    rowList.Add("");
                                    continue;
                                }



                                Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rowsToProc[rCnt]).ChildElements.ElementAt((int)actualCellIdx);
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
                                    else if (thecurrentcell.DataType == CellValues.Number)
                                    {
                                        rowList.Add(thecurrentcell.CellValue.InnerText);
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
                        catch (Exception)
                        {
                            int hhh = 0;
                            //throw;
                        }
                        
                    }
                    //  return JsonConvert.SerializeObject(dtTable);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dtTable;
        }


        //int GetColumnIndexByName(GridViewRow row, string columnName)
        //{
        //    int columnIndex = 0;
        //    foreach (DataControlFieldCell cell in row.Cells)
        //    {
        //        if (cell.ContainingField is BoundField)
        //            if (((BoundField)cell.ContainingField).DataField.Equals(columnName))
        //                break;
        //        columnIndex++; // keep adding 1 while we don't have the correct name
        //    }
        //    return columnIndex;
        //}

        private static int CellReferenceToIndex(Cell cell)
        {
            int index = 0;
            string reference = cell.CellReference.ToString().ToUpper();
            foreach (char ch in reference)
            {
                if (Char.IsLetter(ch))
                {
                    int value = (int)ch - (int)'A';
                    index = (index == 0) ? value : ((index + 1) * 26) + value;
                }
                else
                {
                    return index;
                }
            }
            return index;
        }
        static void UpdateEmployees(DataContext _context, DataTable dataTable)
        {
            string[] excludeNames = new string[]
            {
                "ПП",
                "ТОВ",
                "ПСП",
                "СТОВ",
                "ХПП",
                "ПРАТ",
                "Спілка",
                "Орендодавці",
                "Оренда",
                "ФОП"
            };


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                try
                {
                    bool isFirm = false;
                    string checkString = (string)dataTable.Rows[i]["Власник/Орендодавець"];
                    Regex r = new Regex(@"\s+");
                    checkString = r.Replace(checkString, @" ");

                    foreach (var item in excludeNames)
                    {
                        if (checkString.IndexOf(item) > -1)
                        {
                            isFirm = true;
                        }
                    }

                    if (!isFirm)
                    {
                        //Regex r = new Regex(@"\s+");
                        //string input = r.Replace((string)dataTable.Rows[i]["Власник/Орендодавець"], @" ");


                        string[] fio = checkString.Trim().Split(" ");

                        if (fio[0].Trim().ToUpper().IndexOf("Двор".ToUpper())>-1)
                        {
                            int kdfjk = 0;
                        }

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
                                   where e.LastName.Replace(" ", "").ToUpper() == checkString.Replace(" ", "").ToUpper()
                                   select e).Count();
                        if (emp == 0)
                        {
                            Employee em = new Employee
                            {
                                LastName = checkString.ToUpper()
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
        }
        void UpdateCars(DataTable dataTable, int z)
        {
            DataTable dt = new DataTable();
            DataTable dataToadd = new DataTable();
            foreach (System.Data.DataColumn item in dataTable.Columns)
            {
               dt.Columns.Add(new DataColumn { ColumnName = item.ColumnName });
            }

            foreach (System.Data.DataColumn item in dataTable.Columns)
            {
                dataToadd.Columns.Add(new DataColumn { ColumnName = item.ColumnName });
            }
            //List<Car> carsList = new List<Car>();
            List<string> doubleNumbers = new List<string>();


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (i==0)
                {
                    continue;
                }

                //string searchString= "Державний номер=" + ((string)dataTable.Rows[i]["Державний номер"]).CheckNumber();
                //DataRow dr = dt.Select(searchString).FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                //if (dr == null)
                //{
                //    DataRow workRow = dt.NewRow();
                //    workRow["Державний номер"] = ((string)dataTable.Rows[i]["Державний номер"]).CheckNumber();
                //    workRow["Модель"] = ((string)dataTable.Rows[i]["Модель"]);
                //    workRow["Власник/Орендодавець"] = ((string)dataTable.Rows[i]["Власник/Орендодавець"]);
                //    workRow["Дата випуску"] = (string)dataTable.Rows[i]["Дата випуску"];
                //    dt.Rows.Add(workRow);


                //    //dr["Product_name"] = "cde"; //changes the Product_name
                //}
                //else
                //{
                //    doubleNumbers.Add(((string)dataTable.Rows[i]["Державний номер"]).CheckNumber());
                //}



                var checkNumber = (from c in dt.AsEnumerable()
                                   where c.Field<string>("Державний номер") == ((string)dataTable.Rows[i]["Державний номер"]).CheckNumber()
                                   select c).FirstOrDefault();
                if (checkNumber == null)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["Державний номер"] = ((string)dataTable.Rows[i]["Державний номер"]).CheckNumber();
                    workRow["Модель"] = ((string)dataTable.Rows[i]["Модель"]);
                    workRow["Власник/Орендодавець"] = ((string)dataTable.Rows[i]["Власник/Орендодавець"]);
                    workRow["Дата випуску"] = (string)dataTable.Rows[i]["Дата випуску"];
                    workRow["Заводський номер"] = (string)dataTable.Rows[i]["Заводський номер"];
                    workRow["Класифікатор ОЗ"] = (string)dataTable.Rows[i]["Класифікатор ОЗ"];
                    workRow["М.В.О."] = (string)dataTable.Rows[i]["М.В.О."];
                    workRow["Інв. №"] = (string)dataTable.Rows[i]["Інв. №"];
                    workRow["Назва підприємства"] = (string)dataTable.Rows[i]["Назва підприємства"];

                    
                    dt.Rows.Add(workRow);
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if ((string) dr["Державний номер"] == ((string)dataTable.Rows[i]["Державний номер"]).CheckNumber())
                        {
                            if ((string)dr["Власник/Орендодавець"] == (string)dataTable.Rows[i]["Власник/Орендодавець"])
                            {
                                if ((string)dr["Модель"] != (string)dataTable.Rows[i]["Модель"])
                                {
                                    dr["Модель"] += @"|" + ((string)dataTable.Rows[i]["Модель"]);
                                }
                                if ((string)dr["Дата випуску"]=="")
                                {
                                    dr["Дата випуску"] = ((string)dataTable.Rows[i]["Дата випуску"]);
                                }
                                else
                                {
                                    dr["Дата випуску"] += @"|" + ((string)dataTable.Rows[i]["Дата випуску"]);
                                }
                                if ((string)dr["Заводський номер"] == "")
                                {
                                    dr["Заводський номер"] = ((string)dataTable.Rows[i]["Заводський номер"]);
                                }
                                else
                                {
                                    dr["Заводський номер"] += @"|" + ((string)dataTable.Rows[i]["Заводський номер"]);
                                }
                                if (((string)dr["Класифікатор ОЗ"]).Length < ((string)dataTable.Rows[i]["Класифікатор ОЗ"]).Length)
                                {
                                    dr["Класифікатор ОЗ"] = ((string)dataTable.Rows[i]["Класифікатор ОЗ"]);
                                }
                                if (((string)dr["М.В.О."]).Length < ((string)dataTable.Rows[i]["М.В.О."]).Length)
                                {
                                    dr["М.В.О."] = ((string)dataTable.Rows[i]["М.В.О."]);
                                }
                                if (((string)dr["Інв. №"]).Length < ((string)dataTable.Rows[i]["Інв. №"]).Length)
                                {
                                    dr["Інв. №"] = ((string)dataTable.Rows[i]["Інв. №"]);
                                }
                                if (((string)dr["Назва підприємства"]).Length < ((string)dataTable.Rows[i]["Назва підприємства"]).Length)
                                {
                                    dr["Назва підприємства"] = ((string)dataTable.Rows[i]["Назва підприємства"]);
                                }

                            }
                            else if (true)
                            {
                                DataRow workRow = dataToadd.NewRow();
                                workRow["Державний номер"] = ((string)dataTable.Rows[i]["Державний номер"]).CheckNumber();
                                workRow["Модель"] += ((string)dataTable.Rows[i]["Модель"]);
                                workRow["Власник/Орендодавець"] = ((string)dataTable.Rows[i]["Власник/Орендодавець"]);
                                workRow["Дата випуску"] = (string)dataTable.Rows[i]["Дата випуску"];
                                dataToadd.Rows.Add(workRow);
                            }


                            //if ((string)dr["Модель"]!= (string)dataTable.Rows[i]["Модель"])
                            //{
                            //    DataRow workRow = dataToadd.NewRow();
                            //    workRow["Державний номер"] = ((string)dataTable.Rows[i]["Державний номер"]).CheckNumber();
                            //    workRow["Модель"] += ((string)dataTable.Rows[i]["Модель"]);
                            //    workRow["Власник/Орендодавець"] = ((string)dataTable.Rows[i]["Власник/Орендодавець"]);
                            //    workRow["Дата випуску"] = (string)dataTable.Rows[i]["Дата випуску"];
                            //    dataToadd.Rows.Add(workRow);
                            //}
                            //else if ((string)dr["Власник/Орендодавець"] != (string)dataTable.Rows[i]["Власник/Орендодавець"])
                            //{
                            //    DataRow workRow = dataToadd.NewRow();
                            //    workRow["Державний номер"] = ((string)dataTable.Rows[i]["Державний номер"]).CheckNumber();
                            //    workRow["Модель"] += ((string)dataTable.Rows[i]["Модель"]);
                            //    workRow["Власник/Орендодавець"] = ((string)dataTable.Rows[i]["Власник/Орендодавець"]);
                            //    workRow["Дата випуску"] = (string)dataTable.Rows[i]["Дата випуску"];
                            //    dataToadd.Rows.Add(workRow);
                            //}
                            //else
                            //{
                            //    dr["Власник/Орендодавець"]+=@"|"+ ((string)dataTable.Rows[i]["Власник/Орендодавець"]);
                            //    dr["Дата випуску"] += @"|" + ((string)dataTable.Rows[i]["Дата випуску"]);
                            //}
                            doubleNumbers.Add(((string)dataTable.Rows[i]["Державний номер"]).CheckNumber());
                        }
                    }
                }
            }


            var outpath = Path.Combine(
                Directory.GetCurrentDirectory(), @"wwwroot/OutFiles",
                "RecordsForCheck.xlsx");

            dataToadd.CreateSpreadsheetWorkbook(outpath);
           
            UpdateCars(dt);
        }
        void UpdateCars(DataTable dataTable)
        {
            ///*Получение моделей авто*/
            //var models = (from m in dataTable.AsEnumerable()
            //              orderby m.Field<string>("Модель")
            //              select new
            //              {
            //                  model = m.Field<string>("Модель").Split(" ")[0].ToUpper()
            //              }).GroupBy(m => m.model).ToArray();

            //List<Manufacturer> manufacturers = new List<Manufacturer>();

            //for (int i = 0; i < models.Count(); i++)
            //{
            //    if (models[i].Key != "")
            //    {
            //        Manufacturer man = new Manufacturer
            //        {
            //            Name = models[i].Key
            //        };

            //        var manEx = (from m in _context.Manufacturers
            //                     where m.Name.ToUpper() == models[i].Key.ToUpper()
            //                     select m).FirstOrDefault();

            //        if (manEx == null)
            //        {
            //            manufacturers.Add(man);
            //        }
            //    }
            //}

            //_context.Manufacturers.AddRange(manufacturers);
            //_context.SaveChanges();


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (i==2000)
                {
                    int kdjfkdsj = 0;
                }
                try
                {

                    DateTime regDate;
                    try
                    {
                        if (((string)(dataTable.Rows[i]["Дата випуску"])).Length== 4)
                        {
                            regDate = new DateTime(Int32.Parse((string)dataTable.Rows[i]["Дата випуску"]),1,1);
                        }
                        else
                        {
                            regDate = Convert.ToDateTime(dataTable.Rows[i]["Дата випуску"]);
                        }
                    }
                    catch (Exception)
                    {
                        regDate = new DateTime(1917, 11, 7);
                    }


                    var manuf = from m in _context.Manufacturers
                                where ((string)dataTable.Rows[i]["Модель"]).IndexOf(m.Name) > -1
                                select m;

                    string onlyModel = "";

                    if (manuf.FirstOrDefault() != null)
                    {
                        if (manuf.Count() > 1)
                        {
                            foreach (var item in manuf)
                            {
                                if (((string)dataTable.Rows[i]["Модель"]).ToUpper().IndexOf(item.Name) == 0)
                                {
                                    onlyModel = ((string)dataTable.Rows[i]["Модель"]).Replace(item.Name, "").Trim();
                                    break;
                                }
                            }

                        }
                        else
                        {
                            onlyModel = ((string)dataTable.Rows[i]["Модель"]).Replace((string)manuf.First().Name, "").Trim();
                        }
                    }



                    var model = (from m in _context.CarModels
                                 where (m.Model.ToUpper().Trim()).Equals(onlyModel.ToUpper())
                                 select m).FirstOrDefault();

                    int cmId = 1;
                    if (manuf != null && model != null)
                    {
                        cmId = model.CarModelId;
                    }

                    
                    int carOwnerId = CheckOwnerID((string)dataTable.Rows[i]["Власник/Орендодавець"]); 
                    
                    Car car = new Car
                    {
                        RegistrationNumber = ((string)dataTable.Rows[i]["Державний номер"]).CheckNumber(),
                        VinNumber= ((string)dataTable.Rows[i]["Заводський номер"]),
                        FirstRegistrationYear = regDate.Year,
                        CarModelId = cmId,
                        CarOwnerId = carOwnerId,
                        TempModel = (string)dataTable.Rows[i]["Модель"],
                        TempOwner = (string)dataTable.Rows[i]["Власник/Орендодавець"],
                        TempInv = (string)dataTable.Rows[i]["Інв. №"],
                        TempType = (string)dataTable.Rows[i]["Класифікатор ОЗ"],
                        TempUser = (string)dataTable.Rows[i]["М.В.О."],
                        TempFirm = (string)dataTable.Rows[i]["Назва підприємства"]
                    };

                    _context.Cars.Add(car);
                }
                catch (Exception)
                {
                    int z = 0;//throw;
                }

            }
            _context.SaveChanges();
        }

       
    }
}