using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using IInfrastructure.IRepositories.Purchase;
using IInfrastructure;
using IInfrastructure.Models;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Purchase
{
    public class PoRepository : Infrastructure.BaseRepository<DAO.EF.Po>, IPoRepository
    {
        public PoRepository(IDbContext dbContext):base(dbContext)
        {
             Mapper.Initialize(p =>
             {
                 p.CreateMap<DAO.EF.Po, IInfrastructure.Models.Purchase.Po>();
             });
            LoggerService.LoggerService.ApplicationLogger.Debug("一个参数构造函数初始化PoRepository");
        }
        
        public IList<IInfrastructure.Models.Purchase.Po> GetList(Expression<Func<IInfrastructure.Models.Purchase.Po, bool>> predicate)
        {
            return _entities
                .ProjectTo<IInfrastructure.Models.Purchase.Po>()
                .Where(predicate)
                .ToList()
                ;
        }

        public IList<IInfrastructure.Models.Purchase.Po> GetList(Expression<Func<IInfrastructure.Models.Purchase.Po, bool>> predicate, string order, string sort)
        {
            return _entities
                .ProjectTo<IInfrastructure.Models.Purchase.Po>()
                .Where(predicate)
                .ToList()
                ;
        }

        public IList<IInfrastructure.Models.Purchase.Po> GetList(Expression<Func<IInfrastructure.Models.Purchase.Po, bool>> predicate, GridPager gridPager)
        {
            return _entities
                .ProjectTo<IInfrastructure.Models.Purchase.Po>()
                .Where(predicate)
                .ToList()
                ;
        }

        public IInfrastructure.Models.Purchase.Po GetModel(params object[] Keys)
        {
            if (Keys.Count() <= 0) throw new Exception("请传入单号！");
            return Mapper.Map<IInfrastructure.Models.Purchase.Po>(_entities.FirstOrDefault(p => p.PONUM.Equals(Keys[0])));
        }        
    }
}
