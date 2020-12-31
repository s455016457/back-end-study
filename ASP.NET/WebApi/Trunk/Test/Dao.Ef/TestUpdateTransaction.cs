using DAO.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Dao.Ef
{
    public class TestUpdateTransaction : BaseTest
    {
        //private static swerpEntities _dbContext = new swerpEntities("data source=.;initial catalog=swerp;user id=swerp;password=swerp2008");
        //private DbContextTransaction transaction = _dbContext.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);

        public TestUpdateTransaction()
        {
            using (swerpEntities _dbContext = new swerpEntities("data source=.;initial catalog=swerp;user id=swerp;password=swerp2008"))
            {
                var itemType = _dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals("110103"));
                itemType.DESCR = "测试";
                _dbContext.SaveChanges();
            }
        }

        public override void Test()
        {
            WriteLine("测试更新事务");
            test();
        }

        private void test()
        {
            using (swerpEntities _dbContext = new swerpEntities("data source=.;initial catalog=swerp;user id=swerp;password=swerp2008"))
            {

                var itemType = _dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals("110103"));
                WriteLine("修改前次类编号【{0}】，名称【{1}】", itemType.CODE, itemType.DESCR);
            }
            Task.Factory.StartNew(() =>
            {
                using (swerpEntities _dbContext = new swerpEntities("data source=.;initial catalog=swerp;user id=swerp;password=swerp2008"))
                {

                    Thread.Sleep(50);
                    var transaction = _dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    var itemType1 = _dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals("110103"));
                    Thread.Sleep(200);
                    WriteLine("第一次修改 获取到数据次类编号【{0}】，名称【{1}】", itemType1.CODE, itemType1.DESCR);
                    itemType1.DESCR += "第一次修改";
                    WriteLine("第一次修改 次类编号【{0}】，名称【{1}】", itemType1.CODE, itemType1.DESCR);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
            });
            Task.Factory.StartNew(() =>
            {
                using (swerpEntities _dbContext = new swerpEntities("data source=.;initial catalog=swerp;user id=swerp;password=swerp2008"))
                {

                    Thread.Sleep(100);
                    var transaction = _dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    var itemType2 = _dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals("110103"));
                    WriteLine("第二次修改 获取到数据次类编号【{0}】，名称【{1}】", itemType2.CODE, itemType2.DESCR);
                    itemType2.DESCR += "第二次修改";
                    WriteLine("第二次修改 次类编号【{0}】，名称【{1}】", itemType2.CODE, itemType2.DESCR);
                    _dbContext.SaveChanges();
                    //_dbContext.Set<ItemType>().
                    transaction.Commit();
                }
            });

            Thread.Sleep(500);
            using (swerpEntities _dbContext = new swerpEntities("data source=.;initial catalog=swerp;user id=swerp;password=swerp2008"))
            {
                var itemType = _dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals("110103"));
                WriteLine("修改后次类编号【{0}】，名称【{1}】", itemType.CODE, itemType.DESCR);
            }
        }
    }
}
