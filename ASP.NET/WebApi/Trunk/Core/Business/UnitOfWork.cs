using IInfrastructure;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork: IUnitOfWork,IDisposable
    {
        private IDbContext _dbContext;
        private DbContextTransaction _dbTransaction;
        /// <summary>
        /// 工作单元构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public UnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 开启事物
        /// </summary>
        public void BeginTransaction()
        {
            _dbTransaction = _dbContext.Database.BeginTransaction();
        }

        /// <summary>
        /// 注册新对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RegisterNew<TEntity>(TEntity entity)
            where TEntity : class
        {
            _dbContext.Set<TEntity>().Add(entity);
            if (_dbTransaction != null)
                return await _dbContext.SaveChangesAsync() > 0;
            return true;
        }
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RegisterDirty<TEntity>(TEntity entity)
            where TEntity : class
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            if (_dbTransaction != null)
                return await _dbContext.SaveChangesAsync() > 0;
            return true;
        }
        /// <summary>
        /// 取消更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RegisterClean<TEntity>(TEntity entity)
            where TEntity : class
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Unchanged;
            if (_dbTransaction != null)
                return await _dbContext.SaveChangesAsync() > 0;
            return true;
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RegisterDeleted<TEntity>(TEntity entity)
            where TEntity : class
        {
            _dbContext.Set<TEntity>().Remove(entity);
            if (_dbTransaction != null)
                return await _dbContext.SaveChangesAsync() > 0;
            return true;
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Commit()
        {
            if (_dbTransaction == null)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                _dbTransaction.Commit();
            return true;
        }
        /// <summary>
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
            if (_dbTransaction != null)
                _dbTransaction.Rollback();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
