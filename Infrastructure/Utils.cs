using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KernelCars.Infrastructure
{
    public static class Utils
    {
        public static string CheckNumber(this String str)
        {
            Dictionary<string, string> latToRus = new Dictionary<string, string>()
            {
                { "A","А"},
                { "B","В"},
                { "C","С"},
                { "E","Е"},
                { "H","Н"},
                { "I","І"},
                { "K","К"},
                { "M","М"},
                { "O","О"},
                { "P","Р"},
                { "T","Т" }
            };


            string inString = str.Replace(" ", "").ToUpper();
            string outString = "";

            for (int i = 0; i < inString.Length; i++)
            {
                string _outStr = String.Empty;
                if (Char.IsLetter(inString[i]))
                {
                    if (latToRus.TryGetValue(inString[i].ToString(), out _outStr))
                    {
                    }

                    if (!String.IsNullOrEmpty(_outStr))
                    {
                        outString += _outStr;
                    }
                    else
                    {
                        outString += inString[i].ToString();
                    }
                }
                else
                {
                    outString += inString[i].ToString();
                }
            }
            return outString;
        }

        public static int? GetSellIndexFromColumnLetter(this Row row,string cellChar)
        {
            int? index = null;
            Regex regex = new Regex(@"\D+");

            for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
            {
                Cell cell = row.Descendants<Cell>().ElementAt(i);
                if (regex.Match(cell.CellReference).Value==cellChar)
                {
                    index = i;
                    break;
                }
            }
            
            return index;
        }

        public static void CreateSpreadsheetWorkbook(this DataTable dataTable, string filepath)
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

    }
}
