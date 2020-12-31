using Domain.Repository.Interface;
using System.Data.Entity;

namespace DAO.EF.Repository.Imp
{
    public class BaseRepository: IBaseRepository
    {
        protected DbContext _dbContext;
        protected DbContextTransaction transaction;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void BeginTransaction()
        {
            transaction = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
            if (transaction != null)
            {
                transaction.Commit();
            }
        }
    }
}
