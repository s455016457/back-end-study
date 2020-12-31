namespace Domain.Entity
{
    public enum EntityState
    {
        /// <summary>
        /// 该实体未由上下文跟踪。刚使用新运算符或某个 System.Data.Entity.DbSet Create 方法创建实体后，实体就处于此状态。
        /// </summary>
        Detached = 1,
        /// <summary>
        /// 实体将由上下文跟踪并存在于数据库中，其属性值与数据库中的值相同。
        /// </summary>
        Unchanged = 2,
        /// <summary>
        /// 实体将由上下文跟踪，但是在数据库中还不存在。
        /// </summary>
        Added = 4,
        /// <summary>
        /// 实体将由上下文跟踪并存在于数据库中，但是已被标记为在下次调用 SaveChanges 时从数据库中删除。
        /// </summary>
        Deleted = 8,
        /// <summary>
        /// 实体将由上下文跟踪并存在于数据库中，已修改其中的一些或所有属性值。
        /// </summary>
        Modified = 16
    }
}