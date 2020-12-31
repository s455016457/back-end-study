using IInfrastructure;
using System.Linq;

namespace Infrastructure
{
    public class BaseRepository<T> where T:class
    {
        public readonly IQueryable<T> _entities;
        public BaseRepository(IDbContext dbContext)
        {
            _entities = dbContext.Set<T>();
        }
    }
}
