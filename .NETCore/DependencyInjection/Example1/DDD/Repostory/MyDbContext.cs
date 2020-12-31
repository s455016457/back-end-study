using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Example1.DDD.Entities;
using System.Threading.Tasks;

namespace Example1.DDD.Repostory
{
    public class MyDbContext:DbContext
    {
        public DbSet<Po> PoSet { get; set; }

        private ILogger<MyDbContext> _logger;
        public MyDbContext(ILogger<MyDbContext> logger,DbContextOptions options):base(options){
            _logger=logger;
            _logger.LogDebug("初始化资源库！");
        }

        public override void Dispose()
        {
            _logger.LogDebug("Dispose:销毁资源库");
            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            _logger.LogDebug("DisposeAsync:销毁资源库");
            return base.DisposeAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /// <summary>
            /// 添加EF日志
            /// </summary>
            /// <value></value>
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder=>{
                builder
                    .ClearProviders()
                    .AddFilter((category,level)=>{
                        return category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information;
                    })
                    .AddConsole()
                    .AddLog4Net();
            }));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Mapping.PoMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}