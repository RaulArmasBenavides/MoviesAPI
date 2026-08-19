using ClosedXML.Excel;
using ApiMovies.Application.Interfaces;

namespace ApiMovies.Application.Services
{
    public class ExcelService : IExcelService
    {
        public byte[] GenerateTemplate<T>(string[] columnNames) where T : class
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Data");

                for (int i = 0; i < columnNames.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = columnNames[i];
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                    worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                }

                worksheet.Columns().AdjustToContents();

                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }

        public List<T> ImportFromExcel<T>(Stream fileStream, string fileName) where T : class
        {
            var result = new List<T>();

            if (fileStream == null || fileStream.Length == 0)
                throw new ArgumentException("El archivo no puede estar vacío");

            if (!Path.GetExtension(fileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Solo se aceptan archivos Excel (.xlsx)");

            using (var workbook = new XLWorkbook(fileStream))
            {
                var worksheet = workbook.Worksheets.First();
                var properties = typeof(T).GetProperties();
                var lastRowCell = worksheet.LastCellUsed();
                var rowCount = lastRowCell == null ? 1 : lastRowCell.Address.RowNumber;

                for (int row = 2; row <= rowCount; row++)
                {
                    var instance = (T)Activator.CreateInstance(typeof(T))!;

                    for (int col = 1; col <= properties.Length; col++)
                    {
                        var cell = worksheet.Cell(row, col);
                        var cellValue = cell.Value;
                        var property = properties[col - 1];

                        if (!cell.IsEmpty())
                        {
                            try
                            {
                                var convertedValue = Convert.ChangeType(cellValue.ToString(), property.PropertyType);
                                property.SetValue(instance, convertedValue);
                            }
                            catch (Exception ex)
                            {
                                throw new ArgumentException($"Error en fila {row}, columna {col}: {ex.Message}");
                            }
                        }
                    }

                    result.Add(instance);
                }
            }

            return result;
        }
    }
}
