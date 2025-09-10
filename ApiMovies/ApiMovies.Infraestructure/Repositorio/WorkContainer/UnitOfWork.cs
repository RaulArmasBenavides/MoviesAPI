using ApiMovies.Core.IRepositorio;
using ApiMovies.Infrastructure.Data;
using ApiMovies.Repositorio;

namespace ApiMovies.Infrastructure.Repositorio.WorkContainer
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext db;
        public UnitOfWork( ApplicationDbContext db) {
            db = db;
            Categories = new CategoryRepository(db);
            Movies = new MovieRepository(db);
            Users = new UserRepository(db, null);
        }
        public ICategoryRepository Categories { get; private set; }
        public IMovieRepository Movies { get; private set; }
        public IUserRepository Users { get; private set; }
        public void Dispose()
        {
            db.Dispose();
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }
    }
}
