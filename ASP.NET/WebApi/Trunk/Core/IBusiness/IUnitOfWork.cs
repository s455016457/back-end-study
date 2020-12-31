using System;

using System.Threading.Tasks;

namespace IInfrastructure
{
    public interface IUnitOfWork:IDisposable
    {
        void BeginTransaction();

        Task<bool> RegisterNew<TEntity>(TEntity entity)
            where TEntity : class;

        Task<bool> RegisterDirty<TEntity>(TEntity entity)
            where TEntity : class;

        Task<bool> RegisterClean<TEntity>(TEntity entity)
            where TEntity : class;

        Task<bool> RegisterDeleted<TEntity>(TEntity entity)
            where TEntity : class;

        Task<bool> Commit();

        void Rollback();
    }
}
