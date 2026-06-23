namespace ApiMovies.Application.Dtos
{
    public class MovieFilterDto
    {
        public string SearchTerm { get; set; } = "";
        public string Clasificacion { get; set; } = "";
        public string Estado { get; set; } = "";
        public int? CategoriaId { get; set; }
        public string OrderBy { get; set; } = "name"; // "name" o "date"
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 10;
    }
}
