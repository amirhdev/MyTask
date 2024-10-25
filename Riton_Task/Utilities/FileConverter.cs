using ClosedXML.Excel;
using System.Data;

namespace Riton_Task.Utilities;

public static class FileConverter
{
    public static async Task<DataSet> ExcelToDataSetAsync(Stream stream)
    {
        var dataSet = new DataSet();
        using var workBook = new XLWorkbook(stream);
        foreach (var sheet in workBook.Worksheets)
        {
            var table = new DataTable(sheet.Name);

            GenerateColumns(sheet, table);

            GenerateRows(sheet, table);

            dataSet.Tables.Add(table);
        }

        return dataSet;
    }

    private static void GenerateColumns(IXLWorksheet sheet, DataTable table)
    {
        sheet
            .FirstRowUsed()
            .CellsUsed()
            .ToList()
            .ForEach(cell => table.Columns.Add(cell.Value.ToString()));
    }

    private static void GenerateRows(IXLWorksheet? sheet, DataTable table)
    {
        foreach (var row in sheet.RowsUsed().Skip(1))
        {
            var dr = table.NewRow();

            GenerateCells(table.Columns.Count, row, dr);

            table.Rows.Add(dr);
        }
    }

    private static void GenerateCells(int columnCount, IXLRow row, DataRow dr)
    {
        for (var i = 0; i < columnCount; i++)
            dr[i] = row.Cell(i + 1).Value.ToString();
    }
}