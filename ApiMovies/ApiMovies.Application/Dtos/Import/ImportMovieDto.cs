namespace ApiMovies.Application.Dtos.Import
{
    public class ImportMovieDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string Clasificacion { get; set; }
        public int categoriaId { get; set; }
    }
}
