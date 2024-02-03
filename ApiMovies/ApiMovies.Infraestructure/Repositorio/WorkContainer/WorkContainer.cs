using ApiMovies.Infraestructure.Data;
using ApiMovies.Infraestructure.Repositorio.IRepositorio;
using ApiMovies.Repositorio;

namespace ApiMovies.Infraestructure.Repositorio.WorkContainer
{
    public class WorkContainer : IWorkContainer
    {

        private readonly ApplicationDbContext _db;
        public WorkContainer( ApplicationDbContext db) {
            _db = db;
            Categorias = new CategoriaRepositorio(_db);
            Peliculas = new PeliculaRepositorio(_db);
            Usuarios = new UsuarioRepositorio(_db,null);
        }
        public ICategoriaRepositorio Categorias { get; private set; }
        public IPeliculaRepositorio Peliculas { get; private set; }
        public IUsuarioRepositorio Usuarios { get; private set; }
        public void Dispose()
        {
            _db.Dispose();
        }
        public void Save()
        {
            _db.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
