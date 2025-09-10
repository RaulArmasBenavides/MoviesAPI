using ApiMovies.Core.IRepositorio;
using ApiMovies.Infrastructure.Data;
using ApiMovies.Repositorio;

namespace ApiMovies.Infrastructure.Repositorio.WorkContainer
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _db;
        public UnitOfWork( ApplicationDbContext db) {
            _db = db;
            Categorias = new CategoryRepository(_db);
            Peliculas = new MovieRepository(_db);
            Usuarios = new UserRepository(_db,null);
        }
        public ICategoryRepository Categorias { get; private set; }
        public IMovieRepository Peliculas { get; private set; }
        public IUserRepository Usuarios { get; private set; }
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
