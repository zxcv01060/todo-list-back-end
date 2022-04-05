using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace TodoList.Utilities;

public static class ExcelWorkbookUtils
{
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
