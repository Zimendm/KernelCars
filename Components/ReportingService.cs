using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Data;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace KernelCars.Components
{
    public class ReportingService
    {
        public void GenerateCarBase(IJSRuntime iJSRuntime,DataContext _context)
        {
            byte[] fileContent;
            DataTable dtServices = new System.Data.DataTable();

            dtServices.Columns.Add("Модель");
            dtServices.Columns.Add("Гос.номер");
            dtServices.Columns.Add("Эксплуатирует");
            dtServices.Columns.Add("Подразделение");
            dtServices.Columns.Add("Владелец");
            dtServices.Columns.Add("Закрепление");
            dtServices.Columns.Add("Размещение");
            dtServices.Columns.Add("Статус");

            var cars = _context.Cars
                .Include(c => c.CarModel).ThenInclude(c => c.Manufacturer)
                .Include(c => c.CarOwner)
                .Include(c => c.CarUsers).ThenInclude(c => c.Employee)
                .Include(c => c.CarStatuses).ThenInclude(c => c.Unit).ThenInclude(c => c.Firm).ThenInclude(c => c.Employee)
                .Include(c => c.CarStatuses).ThenInclude(c => c.Unit).ThenInclude(c => c.Department)
                .Include(c => c.CarStatuses).ThenInclude(c => c.Status)
                .Include(c => c.CarStatuses).ThenInclude(c => c.Location)
                .ToList();
            
            foreach (var item in cars)
            {
                DataRow row = dtServices.NewRow();
                row["Модель"] = item.CarModel.Manufacturer.Name + " " + item.CarModel.Model;
                row["Гос.номер"] = item.RegistrationNumber;

                row["Владелец"] = item.CarOwner.FullName;
                row["Закрепление"] = item.CarUserForView;

                if (item.CarStatuses.Count > 0)
                {
                    row["Эксплуатирует"] = item.CarStatuses.LastOrDefault().Unit.Firm.Employee.FullName;
                    row["Подразделение"] = item.CarStatuses.LastOrDefault().Unit.Department.Name;
                    row["Размещение"] = item.CarStatuses.LastOrDefault().Location.LocationName;


                    string _outStat = String.Empty;
                    if (KernelCars.Infrastructure.Utils.carStatus.TryGetValue(item.CarStatuses.LastOrDefault().Status.State, out _outStat))
                    {
                        row["Статус"] = _outStat;
                    }
                }
                dtServices.Rows.Add(row);
            }





            MemoryStream ms = new MemoryStream();

            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.
                   //Create(outpathLocal, SpreadsheetDocumentType.Workbook);
                   Create(ms, SpreadsheetDocumentType.Workbook);

            //Add a WorkbookPart to the document.
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
                Name = "Автомобили"
            };
            sheets.Append(sheet);

            Row firstRow = new Row();

            Cell headerCell = new Cell();

            headerCell.DataType = CellValues.String;
            headerCell.CellValue = new CellValue("Список легковых авто по состоянию на " + DateTime.Now.ToShortDateString());
            firstRow.AppendChild(headerCell);
            sheetData.AppendChild(firstRow);

            Row headerRow = new Row();

            List<String> columns = new List<string>();
            foreach (System.Data.DataColumn column in dtServices.Columns)
            {
                columns.Add(column.ColumnName);

                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(column.ColumnName);
                headerRow.AppendChild(cell);
            }
            sheetData.AppendChild(headerRow);



            foreach (DataRow dsrow in dtServices.Rows)
            {
                Row newRow = new Row();
                foreach (String col in columns)
                {
                    Cell cell = new Cell();
                    //if (col == "Гос.номер" || col == "Модель")
                    //{
                    cell.DataType = CellValues.String;
                    //}
                    //else
                    //{
                    //    cell.DataType = CellValues.Number;
                    //}
                    cell.CellValue = new CellValue(dsrow[col].ToString());//.Replace(",", "."));
                    newRow.AppendChild(cell);

                }

                sheetData.AppendChild(newRow);
            }


            workbookpart.Workbook.Save();
            // Close the document.
            spreadsheetDocument.Close();
            ms.Position = 0;
            

            fileContent = ms.ToArray();
            ms.Close();

            iJSRuntime.InvokeAsync<ReportingService>(
                "saveAsFile",
                "CarBase_"+DateTime.Now.ToString()+".xlsx",
                Convert.ToBase64String(fileContent)
                );
        }
    }
}
