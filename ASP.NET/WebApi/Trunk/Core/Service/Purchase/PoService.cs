using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using System.Data.Entity;
using IInfrastructure;

namespace Service.Purchase
{
    public class PoService : IService.Interface.Purchase.IPOService
    {
        private IUnitOfWork _unitOfWork;
        public PoService()
        {
            //Mapper.Initialize(p =>
            //{
            //    p.CreateMap<PoSearchModel, IBusiness.Models.Po>();
            //    p.CreateMap<IBusiness.Models.Po, PoSearchModel>();
            //});
            LoggerService.LoggerService.ApplicationLogger.Debug("PoService无参数构造函数");
        }
        public PoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            //Mapper.Initialize(p =>
            //{
            //    //p.CreateMap<PoSearchModel, IBusiness.Models.Po>();
            //    p.CreateMap<IBusiness.Models.Po, PoSearchModel>();
            //});
            LoggerService.LoggerService.ApplicationLogger.Debug("PoServicey一个参数构造函数");
        }
    }
}
