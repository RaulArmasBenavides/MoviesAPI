namespace ApiMovies.Application.Dtos
{
    public class CategoryFilterDto
    {
        public string SearchTerm { get; set; } = "";
        public string OrderBy { get; set; } = "name"; // "name" o "date"
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 10;
    }
}
