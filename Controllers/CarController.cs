﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KernelCars.Models;
using KernelCars.Data;
using Microsoft.EntityFrameworkCore;
using KernelCars.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;

namespace KernelCars.Controllers
{
   
    public class CarController : Controller
    {
        private readonly DataContext _context;

        public int PageSize = 100;

        public CarController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int carPage = 1)
        {
            return View(
                new CarsListViewModel {
                    Cars =_context.Cars
                    .Include(c => c.CarModel)
                    .ThenInclude(c => c.Manufacturer)
                    .Include(c=>c.CarOwner)
                    .Skip((carPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList(),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage=carPage,
                        ItemsPerPage=PageSize,
                        TotalItems=_context.Cars.Count()
                    }
                }

                //_context.Cars
                //.Include(c=>c.CarModel)
                //.ThenInclude(c=>c.Manufacturer)
                //.Skip((productPage-1)*PageSize)
                //.Take(PageSize)
                //.ToList())


                );
        }

        public IActionResult Create()
        {
            var manufacturerQuery = from m in _context.Manufacturers
                                    orderby m.Name
                                    select m;
            ViewBag.ManufacturerID = new SelectList(manufacturerQuery.AsNoTracking(), "ManufacturerId", "Name",null);

            return View();
        }

        //[Authorize]
        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = _context.Cars.Include(c => c.CarModel).ThenInclude(c => c.Manufacturer).Include(c=>c.CarOwner)
                .AsNoTracking()
                .FirstOrDefault(m => m.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            ViewBag.Manufacturers = _context.Manufacturers;
            //ViewBag.CarTypes = context.CarTypes.Include(c=>c.Manufacturer);
            //return View("Editor", repository.GetProduct(id));
            var manufacturersQuery = from m in _context.Manufacturers
                                     orderby m.Name
                                     select m;
            ViewBag.ManufacturerId = new SelectList(manufacturersQuery.AsNoTracking(), "ManufacturerId", "Name");

            var carModelsQuery = from m in _context.CarModels
                                     orderby m.Model
                                     select m;
            ViewBag.CarModelId = new SelectList(carModelsQuery.AsNoTracking(), "CarModelId", "Model");


            return View(car);
        }
        //[Authorize]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(Car car, string owners)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            var str = owners;

            Car c = _context.Cars.Find(car.Id);
            c.CarModelId = car.CarModelId;
            //c.CarTypeId = car.CarTypeId;

            var owner = (from o in _context.Employees
                        where o.FirstName == owners
                        select o).First();

            if (owner != null)
            {
                c.CarOwnerId = owner.Id;
            }


            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
            //return View();
            //var courseToUpdate = await _context.Courses
            //    .FirstOrDefaultAsync(c => c.CourseID == id);

            //if (await TryUpdateModelAsync<Course>(courseToUpdate,
            //    "",
            //    c => c.Credits, c => c.DepartmentID, c => c.Title))
            //{
            //    try
            //    {
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateException /* ex */)
            //    {
            //        //Log the error (uncomment ex variable name and write a log.)
            //        ModelState.AddModelError("", "Unable to save changes. " +
            //            "Try again, and if the problem persists, " +
            //            "see your system administrator.");
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID);
            //return View(courseToUpdate);
        }


        private void PopulateOwnersDropDownList(object selectedOwner = null)
        {
            var ownersQuery = from o in _context.Employees
                              orderby o.FirstName
                              select o;
            ViewBag.OwnerID = new SelectList(ownersQuery.AsNoTracking(), "Id", "FirstName", selectedOwner);
        }

        [HttpGet]
        public JsonResult GetCarTypeList(int ManufacturerId)
        {
            var carTypelist = new SelectList(_context.CarModels.Where(c => c.ManufacturerId == ManufacturerId), "CarModelId", "Model");
            return Json(carTypelist);
        }

        [HttpGet]
        public JsonResult GetCarOwnerList(int ManufacturerId)
        {
            var carTypelist = new SelectList(_context.Employees, "Id", "FirstName");
            return Json(carTypelist);
        }

        //[HttpGet]
        public FileResult DownloadFile()
        {
            int z = 0;

            var testFile = (from f in _context.Cars
                            where f.ImagePage1 != null
                            select f).First();

            //if (testFile ==null)
            //{
            //    return NotFound();
            //}
            return File(testFile.ImagePage1, "image/jpg", "testfile.jpg");
        }

        public IActionResult WorkWithData()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file==null||file.Length==0)
            {
                return NotFound();
            }

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),@"wwwroot/InFiles",
                file.FileName);

            using (var stream = new FileStream(path,FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            DataTable dataTable = ReadExcelasJSON(path);
            
            UpdateData(dataTable);

            return RedirectToAction("Index");

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
            //    if (models[i].Key !="")
            //    {
            //        Manufacturer man = new Manufacturer
            //        {
            //            Name = models[i].Key
            //        };

            //        var manEx = (from m in _context.Manufacturers
            //                     where m.Name.ToUpper() == models[i].Key.ToUpper()
            //                     select m).FirstOrDefault();

            //        if (manEx==null)
            //        {
            //            manufacturers.Add(man);
            //        }
            //    }
            //}

            //_context.Manufacturers.AddRange(manufacturers);
            //_context.SaveChanges();

            ////for (int i = 0; i < length; i++)
            ////{

            ////}
            //var cm = (from m in dataTable.AsEnumerable()
            //         orderby m.Field<string>("Модель")
            //         select new
            //         {
            //             model = m.Field<string>("Модель").ToUpper()
            //         }).GroupBy(m => m.model).ToArray();

            //List<CarModel> carModels = new List<CarModel>();

            //foreach (var item in cm)
            //{
            //    var man = (from m in _context.Manufacturers
            //               where item.Key.ToUpper().IndexOf(m.Name.ToUpper()) == 0
            //               select m).FirstOrDefault();

            //    int z = 0;

            //    if (man != null)
            //    {
            //        if (true)
            //        {
            //            var cme = (from cm_ in _context.CarModels
            //                      where cm_.Model.IndexOf(item.Key.Replace(man.Name, "").Trim()) > -1
            //                      select cm_).FirstOrDefault();
            //            if (cme == null)
            //            {
            //                CarModel cn = new CarModel
            //                {
            //                    ManufacturerId = man.ManufacturerId,
            //                    Model = item.Key.Replace(man.Name, "").Trim()
            //                };
            //                _context.CarModels.Add(cn);
            //            }
            //        }
            //    }

            //    //}

            //    _context.SaveChanges();
            //    //for (int i = 0; i < length; i++)
            //    //{

            //    //}
            //    ////foreach (var item in _context.Manufacturers)
            //    //{
            //    //    if ()
            //    //    {

            //    //    }


            //}


            ////_context.Manufacturers.AddRange(models);




            //for (int i = 0; i < dataTable.Rows.Count; i++)
            //{
            //    try
            //    {

            //        DateTime regDate;
            //        try
            //        {
            //            regDate = Convert.ToDateTime(dataTable.Rows[i]["Дата випуску"]);
            //        }
            //        catch (Exception)
            //        {
            //            regDate=new DateTime( 1917,11,7);
            //        }

            //        var manuf = from m in _context.Manufacturers
            //                    where ((string)dataTable.Rows[i]["Модель"]).IndexOf(m.Name) > -1
            //                    select m;

            //        var model  = from m in _context.CarModels
            //                    where ((string)dataTable.Rows[i]["Модель"]).IndexOf(m.Model) > -1
            //                    select m;

            //        int cmId = 2195;
            //        if (manuf!=null && model!=null)
            //        {
            //            if (model.Count()==1)
            //            {
            //               cmId = model.First().CarModelId;
            //            }
                       
            //        }


            //        Car car = new Car
            //        {
            //            RegistrationNumber = (string)dataTable.Rows[i]["Державний номер"],
            //            FirstRegistrationYear = regDate.Year,
            //            CarModelId = cmId,
            //            CarOwnerId = 1,
            //            VinNumber = (string)dataTable.Rows[i]["Кластер"]
            //        };

            //        _context.Cars.Add(car);
            //    }
            //    catch (Exception)
            //    {

            //        //throw;
            //    }
                
            //}

            //_context.SaveChanges();

            //var outpath = Path.Combine(
            //    Directory.GetCurrentDirectory(), @"wwwroot/OutFiles",
            //    file.FileName);


            //CreateSpreadsheetWorkbook(outpath,dataTable);



            //return RedirectToAction("WorkWithData");
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










                    rowHeaders = new string[] { "Кластер", "Назва підприємства", "Назва основного засобу", "Класифікатор ОЗ", "Дата випуску", "Державний номер", "Стан ОЗ", "Модель", "М.В.О.", "Власник/Орендодавець" };
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
                                if (Array.IndexOf(vechicleType.ToArray(), item.Text.Text) > -1)
                                {
                                    rowsToProc.Add(rCnt);
                                    //passengerCellValue.Add(thecurrentcell.InnerText);
                                }
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
                        List<string> rowList = new List<string>();

                        //int[] columns = new int[] { 1, 2, 4, 6, 9, 12, 13, 15, 20, 21, 31, 32, 33, 34, 36, 41 };

                        //foreach (var item in collection)
                        //{

                        //}
                        for (int c = 0; c < columns.Count(); c++)
                        //for (int rCnt1 = 0; rCnt1 < thesheetdata.ElementAt(rCnt).ChildElements.Count()-10; rCnt1++)
                        {

                            //Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(rCnt1);

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

                                        //else if (item.InnerText != null)
                                        //{
                                        //    currentcellvalue = item.InnerText;
                                        //}
                                        //else if (item.InnerXml != null)
                                        //{
                                        //    currentcellvalue = item.InnerXml;
                                        //}
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
                  //  return JsonConvert.SerializeObject(dtTable);

                    int jhj = 0;
                }



            }
            catch (Exception)
            {

                throw;
            }

            return dtTable;
        }

        public static void CreateSpreadsheetWorkbook(string filepath,DataTable dataTable)
        {
            // Create a spreadsheet document by supplying the filepath.
            // By default, AutoSave = true, Editable = true, and Type = xlsx.
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.
                Create(filepath, SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            var sheetData = new SheetData();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());

            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.
                GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "mySheet"
            };
            sheets.Append(sheet);

           


            Row headerRow = new Row();

            List<String> columns = new List<string>();
            foreach (System.Data.DataColumn column in dataTable.Columns)
            {
                columns.Add(column.ColumnName);

                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(column.ColumnName);
                headerRow.AppendChild(cell);
            }

            sheetData.AppendChild(headerRow);

            foreach (DataRow dsrow in dataTable.Rows)
            {
                Row newRow = new Row();
                foreach (String col in columns)
                {
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(dsrow[col].ToString());
                    newRow.AppendChild(cell);
                }

                sheetData.AppendChild(newRow);
            }



















            workbookpart.Workbook.Save();

            // Close the document.
            spreadsheetDocument.Close();
        }

        void UpdateData(DataTable dataTable)
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
                try
                {

                    DateTime regDate;
                    try
                    {
                        regDate = Convert.ToDateTime(dataTable.Rows[i]["Дата випуску"]);
                    }
                    catch (Exception)
                    {
                        regDate = new DateTime(1917, 11, 7);
                    }

                    if (((string)dataTable.Rows[i]["Модель"]).IndexOf("ЗАЗ DAEWOO T13110") >-1)
                    {
                        int kdsjfk = 0;
                    }


                    var manuf = from m in _context.Manufacturers
                                where ((string)dataTable.Rows[i]["Модель"]).IndexOf(m.Name) > -1
                                select m;

                    string onlyModel="";

                    if (manuf.First()!=null)
                    {
                        if (manuf.Count() > 1)
                        {
                            foreach (var item in manuf)
                            {
                                if (((string)dataTable.Rows[i]["Модель"]).ToUpper().IndexOf(item.Name)==0)
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
                                 where  (m.Model.ToUpper().Trim()).Equals(onlyModel.ToUpper()) 
                                 select m).FirstOrDefault();

                    int cmId = 1;
                    if (manuf != null && model != null)
                    {
                        cmId = model.CarModelId;
                    }

                    int carOwnerId = 1;
                    string ownerStr = ((string)dataTable.Rows[i]["Власник/Орендодавець"]).Replace(" ","").ToUpper();

                    var owner = from o in _context.Employees
                                where (o.LastName + o.FirstName + o.MiddleName).Replace(" ", "").ToUpper() == ownerStr
                                select o;

                    if (owner.FirstOrDefault() != null)
                    {
                        if (owner.Count()==1)
                        {
                            carOwnerId = owner.First().Id;
                        }
                    }



                    Car car = new Car
                    {
                        RegistrationNumber = (string)dataTable.Rows[i]["Державний номер"],
                        FirstRegistrationYear = regDate.Year,
                        CarModelId = cmId,
                        CarOwnerId = carOwnerId,
                        TempModel= (string)dataTable.Rows[i]["Модель"]
                    };

                    _context.Cars.Add(car);
                }
                catch (Exception)
                {
                    //throw;
                }

            }
            _context.SaveChanges();
        }
        //public ActionResult GetImage()
        //{
        //    /MemoryStream ms = new MemoryStream();

        //    return null;
        //}
    }
}