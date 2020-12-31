using DAO.EF;
using DAO.EF.Repository.Imp.Setting;
using Domain.Repository.Interface.Setting;
using Common.Extended;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Domain.Factory;

namespace Test.Domain.Repository
{
    public class ItemTypeRepositoryTest : BaseTest
    {
        IItemTypeRepository _repository;
        swerpEntities _dbContext;
        public ItemTypeRepositoryTest()
        {
            _dbContext = new swerpEntities("data source=.;initial catalog=swerp;user id=swerp;password=swerp2008");
            _repository = new ItemTypeRepository(_dbContext);
        }
        public override void Test()
        {

            WriteLine("\"ss\".GetType().IsValueType:{0}", "ss".GetType().IsValueType);
            WriteLine("\"ss\".GetType().IsArray:{0}", "ss".GetType().IsArray);
            WriteLine("typeof(IEnumerable).Name:{0}", typeof(IEnumerable).FullName);
            WriteLine(new int[] { 1, 3, 4 }.GetType().GetInterface(typeof(IEnumerable).Name).ToString());

            {
                var itemTypeAggregate = _repository.FindItemTypeAggregate("1101");
                if (itemTypeAggregate != null)
                {
                    WriteLine("itemTypeEntity:【{0}】【{1}】", itemTypeAggregate.Code, itemTypeAggregate.ItemType.Descr);
                    foreach (var item in _repository.GetChildes(itemTypeAggregate.Code))
                    {
                        WriteLine("itemTypeEntity.GetChilds():【{0}】【{1}】", item.Code, item.ItemType.Descr);
                        var parent = _repository.GetParent(item.Code);
                        if (parent != null)
                        {
                            WriteLine("itemTypeEntity.GetChilds().GetParent():【{0}】【{1}】", parent.Code, parent.ItemType.Descr);
                        }
                    }
                }
            }

            {
                String code = "110103";
                    var itemTypeAggregate1 = _repository.FindItemTypeAggregate("1101");
                if (itemTypeAggregate1 != null)
                {
                    var itemTypeAggregate = SettingAggregateFactory.CreateNew(code, "测试", itemTypeAggregate1.Code, itemTypeAggregate1.ItemType.ItemGroup
                        , itemTypeAggregate1.ItemType.AttribType, itemTypeAggregate1.ItemType.PreFix, itemTypeAggregate1.ItemType.InvAcc, itemTypeAggregate1.ItemType.ItemNumMaxLength
                        , itemTypeAggregate1.ItemType.ItemNumMinLength, itemTypeAggregate1.ItemType.MaxScpRate, itemTypeAggregate1.ItemType.MinScpRate, itemTypeAggregate1.ItemType.OverQty
                        , itemTypeAggregate1.ItemType.OverRate, itemTypeAggregate1.ItemType.IsLotManaged, itemTypeAggregate1.ItemType.IsFgRef, itemTypeAggregate1.ItemType.IsVirtual);

                    //_repository.AddItemTypeAggregate(itemTypeAggregate);
                    //_repository.Commit();
                }
            }

            {
                var itemTypeAggregate = _repository.FindItemTypeAggregate("1101");
                if (itemTypeAggregate != null)
                {
                    var childItemType = itemTypeAggregate.AddChild("110104", "测试2", 15, 20, "", 1000, true, false, true, (decimal)0.015, "TEST", (decimal)0.02, (decimal)0.05);
                    //_repository.AddItemTypeAggregate(childItemType);
                    //_repository.Commit();
                }
            }
            {
                var stopWatch = Stopwatch.StartNew();
                stopWatch.Restart();
                var list = _dbContext.PoDetail.ToList();
                stopWatch.Stop();
                WriteLine("查询【{0}】条数据，共需要【{1}】ss", list.Count(), stopWatch.ElapsedMilliseconds);
            }
            {
                var stopWatch = Stopwatch.StartNew();
                stopWatch.Restart();
                int totalRows = 0, totalPage = 0,pageIndex=2;
                var list = _dbContext.GradeData.Where(p => p.LABELID.Contains("5")).OrderBy(p => p.LABELID, true).Paging(pageIndex, 100, ref totalRows, ref totalPage).ToList();
                stopWatch.Stop();
                WriteLine("第一次查询 共【{1}】页【{0}】条数据，当前第【{3}】页，显示【{4}】条数据，共需要【{2}】ss", totalRows, totalPage, stopWatch.ElapsedMilliseconds, pageIndex, list.Count());
            }
            {
                var stopWatch = Stopwatch.StartNew();
                stopWatch.Restart();
                int totalRows = 0, totalPage = 0, pageIndex = 5;
                var list = _dbContext.GradeData.Where(p => p.LABELID.Contains("5")).OrderBy(p => p.LABELID, true).Paging(pageIndex, 100, ref totalRows, ref totalPage).ToList();
                stopWatch.Stop();
                WriteLine("第二次查询 共【{1}】页【{0}】条数据，当前第【{3}】页，显示【{4}】条数据，共需要【{2}】ss", totalRows, totalPage, stopWatch.ElapsedMilliseconds, pageIndex, list.Count());
            }
        }
    }
}
