using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRExercises.DDD.Repositories
{
    public interface IBaseRepository: IDisposable, IObjectContextAdapter
    {
        /// <summary>
        /// 提供对上下文的配置选项的访问。
        /// </summary>
        DbContextConfiguration Configuration { get; }
        /// <summary>
        /// 为此上下文创建一个数据库实例，该上下文允许您对基础数据库执行创建/删除/存在性检查操作。
        /// </summary>
        Database Database { get; }
        /// <summary>
        /// 提供对用于处理实体的更改跟踪的上下文功能的访问。
        /// </summary>
        DbChangeTracker ChangeTracker { get; }
        /// <summary>
        ///  获取给定实体的 System.Data.Entity.Infrastructure.DbEntityEntry 对象，以便提供对与该实体有关的信息的访问以及对实体执行操作的功能。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityEntry Entry(object entity);
        /// <summary>
        ///  获取给定实体的 System.Data.Entity.Infrastructure.DbEntityEntry`1 对象，以便提供对与该实体有关的信息的访问以及对实体执行操作的功能。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// 验证跟踪实体，并返回包含验证结果的 System.Data.Entity.Validation.DbEntityValidationResult 的集合。
        /// </summary>
        /// <returns></returns>
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
        /// <summary>
        /// 扩展点允许用户重写仅验证添加的实体和修改的实体的默认行为。
        /// </summary>
        /// <param name="entityEntry"></param>
        /// <returns></returns>
        bool ShouldValidateEntity(DbEntityEntry entityEntry);
        /// <summary>
        ///  扩展点允许用户自定义实体的验证或筛选出验证结果。由 System.Data.Entity.DbContext.GetValidationErrors 调用。
        /// </summary>
        /// <param name="entityEntry"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items);
        /// <summary>
        /// 针对对上下文和基础存储中给定类型的实体的访问返回一个非泛型 System.Data.Entity.DbSet 实例。
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        DbSet Set(Type entityType);
        /// <summary>
        ///  针对对上下文和基础存储中给定类型的实体的访问返回一个 System.Data.Entity.DbSet`1 实例。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();
    }
}
