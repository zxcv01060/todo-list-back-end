using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace TodoList.Utilities;

public static class ExcelWorkbookUtils
{
    public static void AddHeaderRow(SheetData sheet, params string[] headers)
    {
        var header = new Row();
        header.Append(headers.ToCellArray());
        sheet.AppendChild(header);
    }

    private static OpenXmlElement[] ToCellArray(this IReadOnlyList<string> values)
    {
        var result = new OpenXmlElement[values.Count];

        for (var i = 0; i < values.Count; i++)
        {
            result[i] = CreateCell(values[i]);
        }

        return result;
    }

    public static Cell CreateCell(string value)
    {
        return new Cell
        {
            CellValue = new CellValue(value),
            DataType = new EnumValue<CellValues>(CellValues.String)
        };
    }

    public static Cell CreateCell(int value)
    {
        return new Cell
        {
            CellValue = new CellValue(value.ToString()),
            DataType = new EnumValue<CellValues>(CellValues.Number)
        };
    }

    public static Cell CreateCell(DateTime value)
    {
        return new Cell
        {
            CellValue = new CellValue(value),
            DataType = new EnumValue<CellValues>(CellValues.Date)
        };
    }
}
