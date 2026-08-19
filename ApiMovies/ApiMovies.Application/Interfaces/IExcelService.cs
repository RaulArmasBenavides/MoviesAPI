namespace ApiMovies.Application.Interfaces
{
    public interface IExcelService
    {
        byte[] GenerateTemplate<T>(string[] columnNames) where T : class;
        List<T> ImportFromExcel<T>(Stream fileStream, string fileName) where T : class;
    }
}
