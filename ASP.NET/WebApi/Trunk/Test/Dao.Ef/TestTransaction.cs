using DAO.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Dao.Ef
{
    public class TestTransaction : BaseTest
    {
        public TestTransaction()
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
            WriteLine("测试无聚合根事务");
            Test1();
        }

        private void Test1()
        {
            using (swerpEntities _dbContext = new swerpEntities("data source=.;initial catalog=swerp;user id=swerp;password=swerp2008"))
            {
                var itemType = _dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals("110103"));
                WriteLine("修改前次类编号【{0}】，名称【{1}】", itemType.CODE, itemType.DESCR);
            }
            Task.Factory.StartNew(() =>
            {
                using (swerpEntities dbContext = new swerpEntities("data source=.;initial catalog=swerp;user id=swerp;password=swerp2008"))
                {
                    var transaction = dbContext.Database.BeginTransaction();
                    var itemType1 = dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals("110103"));
                    Thread.Sleep(50);
                    WriteLine("第一次修改 获取到数据次类编号【{0}】，名称【{1}】", itemType1.CODE, itemType1.DESCR);
                    itemType1.DESCR += "第一次修改";
                    WriteLine("第一次修改 次类编号【{0}】，名称【{1}】", itemType1.CODE, itemType1.DESCR);
                    Thread.Sleep(200);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
            });
            Task.Factory.StartNew(() =>
            {
                using (swerpEntities dbContext = new swerpEntities("data source=.;initial catalog=swerp;user id=swerp;password=swerp2008"))
                {
                    var transaction = dbContext.Database.BeginTransaction();
                    var itemType2 = dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals("110103"));
                    Thread.Sleep(100);
                    WriteLine("第二次修改 获取到数据次类编号【{0}】，名称【{1}】", itemType2.CODE, itemType2.DESCR);
                    itemType2.DESCR += "第二次修改";
                    WriteLine("第二次修改 次类编号【{0}】，名称【{1}】", itemType2.CODE, itemType2.DESCR);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
            });

            Thread.Sleep(300);
            using (swerpEntities _dbContext = new swerpEntities("data source=.;initial catalog=swerp;user id=swerp;password=swerp2008"))
            {

                var itemType = _dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals("110103"));
                WriteLine("修改后次类编号【{0}】，名称【{1}】", itemType.CODE, itemType.DESCR);
            }
        }
    }
}
