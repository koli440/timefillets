using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using TimeFillets.Model;
using DocumentFormat.OpenXml;
using System.Globalization;


namespace TimeFillets.Connectors
{
  /// <summary>
  /// Class for export to excel sheet
  /// </summary>
  public class ExcelSheetConnector : IExportConnector
  {
    /// <summary>
    /// Path to excel sheet template
    /// </summary>
    public string ExcelSheetTemplatePath { get; set; }

    /// <summary>
    /// Name of this connector
    /// </summary>
    public string ConnectorName
    {
      get
      {
        return "ExcelExportConnector";
      }
    }
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="excelSheetTemplatePath">Path to excel sheet template</param>
    public ExcelSheetConnector(string excelSheetTemplatePath)
    {
      ExcelSheetTemplatePath = excelSheetTemplatePath;
    }

    #region IExportConnector Members

    /// <summary>
    /// Exports data to specified file
    /// </summary>
    /// <param name="calendarItems">Items to export</param>
    /// <param name="path">path of file to export</param>
    public void Export(IEnumerable<CalendarItem> calendarItems, string path)
    {
      using (SpreadsheetDocument template = SpreadsheetDocument.Open(ExcelSheetTemplatePath, false))
      {
        using (SpreadsheetDocument output = SpreadsheetDocument.Create(path, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
        {
          output.AddPart<WorkbookPart>(template.WorkbookPart);
          uint rowIndex = 2;
          foreach (var calendarItem in calendarItems)
          {
            InsertText(output, calendarItem.StartDate.ToString("dd.MM. yyyy"), rowIndex, "A");
            InsertText(output, calendarItem.StartDate.ToString("HH:mm"), rowIndex, "B");
            InsertText(output, calendarItem.EndDate.ToString("HH:mm"), rowIndex, "C");
            InsertText(output, calendarItem.CustomerItem.Name, rowIndex, "D");
            InsertText(output, calendarItem.ProjectItem.Name, rowIndex, "E");
            InsertText(output, calendarItem.TaskItem.Name, rowIndex, "F");
            InsertText(output, calendarItem.CleanDescription, rowIndex, "G");
            InsertNumber(output, calendarItem.Duration.TotalHours, rowIndex, "H");
            rowIndex++;
          }
        }
      }
    }

    #endregion

    private static void InsertText(SpreadsheetDocument spreadSheet, string text, uint rowIndex, string colIndex)
    {
      SharedStringTablePart shareStringPart;
      if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
      {
        shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
      }
      else
      {
        shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();
      }

      int index = InsertSharedStringItem(text, shareStringPart);
      WorksheetPart worksheetPart = GetWorksheetPartByName(spreadSheet, "Sheet1");
      Cell cell = InsertCellInWorksheet(colIndex, rowIndex, worksheetPart);
      cell.CellValue = new CellValue(index.ToString());
      cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

      worksheetPart.Worksheet.Save();
    }

    private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
    {
      if (shareStringPart.SharedStringTable == null)
      {
        shareStringPart.SharedStringTable = new SharedStringTable();
      }

      int i = 0;
      foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
      {
        if (item.InnerText == text)
        {
          return i;
        }

        i++;
      }

      shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
      shareStringPart.SharedStringTable.Save();

      return i;
    }

    private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
    {
      Worksheet worksheet = worksheetPart.Worksheet;
      SheetData sheetData = worksheet.GetFirstChild<SheetData>();
      string cellReference = columnName + rowIndex;

      Row row;
      if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
      {
        row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
      }
      else
      {
        row = new Row() { RowIndex = rowIndex };
        sheetData.Append(row);
      }

      if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
      {
        return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
      }
      else
      {
        Cell refCell = null;
        foreach (Cell cell in row.Elements<Cell>())
        {
          if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
          {
            refCell = cell;
            break;
          }
        }

        Cell newCell = new Cell() { CellReference = cellReference };
        row.InsertBefore(newCell, refCell);

        worksheet.Save();
        return newCell;
      }
    }

    private static WorksheetPart GetWorksheetPartByName(SpreadsheetDocument document, string sheetName)
    {
      IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().Where(s => s.Name == sheetName);
      if (sheets.Count() == 0)
      {
        return null;
      }

      string relationshipId = sheets.First().Id.Value;
      WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
      return worksheetPart;
    }

    private static void InsertNumber(SpreadsheetDocument spreadSheet, double number, uint rowIndex, string colIndex)
    {
      WorksheetPart worksheetPart = GetWorksheetPartByName(spreadSheet, "Sheet1");
      Cell cell = InsertCellInWorksheet(colIndex, rowIndex, worksheetPart);
      IFormatProvider formatProvider = CultureInfo.GetCultureInfo("en-us").NumberFormat;
      CellValue value = new CellValue(number.ToString("#.#", formatProvider));
      cell.CellValue = value;
      cell.DataType = CellValues.Number;

      worksheetPart.Worksheet.Save();
    }

    private static void InsertDate(SpreadsheetDocument spreadSheet, DateTime date, uint rowIndex, string colIndex)
    {
      WorksheetPart worksheetPart = GetWorksheetPartByName(spreadSheet, "Sheet1");
      Cell cell = InsertCellInWorksheet(colIndex, rowIndex, worksheetPart);
      cell.CellValue = new CellValue(date.ToString("MM-dd-yyyy"));
      cell.DataType = CellValues.Date;

      worksheetPart.Worksheet.Save();
    }

    private static void InsertTime(SpreadsheetDocument spreadSheet, DateTime date, uint rowIndex, string colIndex)
    {
      WorksheetPart worksheetPart = GetWorksheetPartByName(spreadSheet, "Sheet1");
      Cell cell = InsertCellInWorksheet(colIndex, rowIndex, worksheetPart);
      cell.CellValue = new CellValue(date.ToString("HH:mm"));
      cell.DataType = new EnumValue<CellValues>(CellValues.Date);

      worksheetPart.Worksheet.Save();
    }

    
  }
}
