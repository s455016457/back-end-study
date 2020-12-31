using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Test.DDD.Store;

namespace Test.DDD.Test
{
    /// <summary>
    /// 测试乐观锁
    /// </summary>
    public class TestOptimisticLock : BaseTest
    {
        public override void Test()
        {
            var watch = Stopwatch.StartNew();
            watch.Start();
            for (var i = 1; i <= 40; i++)
            {
                WriteLine("============================开始第【{0}】次测试============================", i);
                test();
                WriteLine("============================结束第【{0}】次测试============================", i);
                WriteLine("");
            }
            watch.Stop();
            WriteLine("共用时【{0}】毫秒", watch.ElapsedMilliseconds);
        }

        private void test()
        {
            List<Task> tasks = new List<Task>();
            using (DDDTestContext _dbContext = new DDDTestContext())
            {
                var po = _dbContext.Set<Po>().FirstOrDefault(p => p.PoNum.Equals("PONUM1"));
                if (po != null)
                {
                    _dbContext.Set<Po>().Remove(po);
                    _dbContext.SaveChanges();
                }

                po = new Po()
                {
                    PoNum = "PONUM1",
                    VenId = "VENID1",
                    TotalAmount = 100
                };
                _dbContext.Set<Po>().Add(po);
                _dbContext.SaveChanges();
                WriteLine("新增采购订单，编号【{0}】，供应商【{1}】，合计金额【{2}】", po.PoNum, po.VenId, po.TotalAmount);
            }
            tasks.Add(Task.Factory.StartNew(() =>
            {
                using (DDDTestContext _dbContext = new DDDTestContext())
                {
                    using (DbContextTransaction transaction = _dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                    {
                        try
                        {
                            var po = _dbContext.Set<Po>().FirstOrDefault(p => p.PoNum.Equals("PONUM1"));
                            WriteLine("第一次修改 获取到数据采购订单，编号【{0}】，供应商【{1}】，合计金额【{2}】", po.PoNum, po.VenId, po.TotalAmount);
                            po.VenId += "第一次修改";
                            WriteLine("第一次修改 采购订单，编号【{0}】，供应商【{1}】，合计金额【{2}】", po.PoNum, po.VenId, po.TotalAmount);
                            _dbContext.SaveChanges();
                            transaction.Commit();
                        }
                        catch (DbUpdateException ex)
                        {
                            WriteLine("事务死锁:{0}", ex.Message);
                            WriteLine("事务死锁:{0}", ex.InnerException.InnerException.Message);
                        }
                        catch (Exception ex)
                        {
                            WriteLine("第一次更新出错：错误信息【{0}】", ex.Message); ;
                            WriteLine(ex);
                            //transaction.Rollback();
                        }
                    }
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                using (DDDTestContext _dbContext = new DDDTestContext())
                {
                    using (DbContextTransaction transaction = _dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                    {
                        try
                        {
                            var po = _dbContext.Set<Po>().FirstOrDefault(p => p.PoNum.Equals("PONUM1"));
                            WriteLine("第二次修改 获取到数据采购订单，编号【{0}】，供应商【{1}】，合计金额【{2}】", po.PoNum, po.VenId, po.TotalAmount);
                            po.VenId += "第二次修改";
                            WriteLine("第二次修改 采购订单，编号【{0}】，供应商【{1}】，合计金额【{2}】", po.PoNum, po.VenId, po.TotalAmount);
                            _dbContext.SaveChanges();
                            transaction.Commit();
                        }
                        catch (DbUpdateException ex)
                        {
                            WriteLine("事务死锁:{0}", ex.Message);
                            WriteLine("事务死锁:{0}", ex.InnerException.InnerException.Message);
                        }
                        catch (Exception ex)
                        {
                            WriteLine("第二次更新出错：错误信息【{0}】", ex.Message); ;
                            WriteLine(ex);
                            //transaction.Rollback();
                        }
                    }
                }
            }));

            Task.WaitAll(tasks.ToArray());
            using (DDDTestContext _dbContext = new DDDTestContext())
            {
                var po = _dbContext.Set<Po>().FirstOrDefault(p => p.PoNum.Equals("PONUM1"));
                WriteLine("修改后采购订单，编号【{0}】，供应商【{1}】，合计金额【{2}】", po.PoNum, po.VenId, po.TotalAmount);
                //_dbContext.Set<Po>().Remove(po);
                //_dbContext.SaveChanges();
            }


            /**
            {
                WriteLine("======================================================================");

                Thread.Sleep(500);
                using (DDDTestContext _dbContext = new DDDTestContext())
                {
                    var po = _dbContext.Set<Po>().FirstOrDefault(p => p.PoNum.Equals("PONUM1"));
                    if (po != null)
                    {
                        _dbContext.Set<Po>().Remove(po);
                        _dbContext.SaveChanges();
                    }

                    po = new Po()
                    {
                        PoNum = "PONUM1",
                        VenId = "VENID1",
                        TotalAmount = 100
                    };
                    _dbContext.Set<Po>().Add(po);
                    _dbContext.SaveChanges();
                    WriteLine("新增采购订单，编号【{0}】，供应商【{1}】，合计金额【{2}】", po.PoNum, po.VenId, po.TotalAmount);
                }

                using (DDDTestContext _dbContext = new DDDTestContext())
                {
                    using (DbContextTransaction transaction = _dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                    {
                        try
                        {
                            var po = _dbContext.Set<Po>().FirstOrDefault(p => p.PoNum.Equals("PONUM1"));
                            WriteLine("第一次修改 获取到数据采购订单，编号【{0}】，供应商【{1}】，合计金额【{2}】", po.PoNum, po.VenId, po.TotalAmount);
                            po.VenId += "第一次修改";
                            WriteLine("第一次修改 采购订单，编号【{0}】，供应商【{1}】，合计金额【{2}】", po.PoNum, po.VenId, po.TotalAmount);
                            _dbContext.SaveChanges();
                            transaction.Commit();
                        }
                        catch (DbUpdateException ex)
                        {
                            WriteLine("事务死锁:{0}", ex.Message);
                        }
                        catch (Exception ex)
                        {
                            WriteLine("第一次更新出错：错误信息【{0}】", ex.Message); ;
                            WriteLine(ex);
                            //transaction.Rollback();
                        }
                    }
                }
                using (DDDTestContext _dbContext = new DDDTestContext())
                {
                    using (DbContextTransaction transaction = _dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                    {
                        try
                        {
                            var po = _dbContext.Set<Po>().FirstOrDefault(p => p.PoNum.Equals("PONUM1"));
                            WriteLine("第二次修改 获取到数据采购订单，编号【{0}】，供应商【{1}】，合计金额【{2}】", po.PoNum, po.VenId, po.TotalAmount);
                            po.VenId += "第二次修改";
                            WriteLine("第二次修改 采购订单，编号【{0}】，供应商【{1}】，合计金额【{2}】", po.PoNum, po.VenId, po.TotalAmount);
                            _dbContext.SaveChanges();
                            transaction.Commit();
                        }
                        catch (DbUpdateException ex)
                        {
                            WriteLine("事务死锁:{0}", ex.Message);
                        }
                        catch (Exception ex)
                        {
                            WriteLine("第二次更新出错：错误信息【{0}】", ex.Message);
                            WriteLine(ex);
                            //transaction.Rollback();
                        }
                    }
                }

                using (DDDTestContext _dbContext = new DDDTestContext())
                {
                    var po = _dbContext.Set<Po>().FirstOrDefault(p => p.PoNum.Equals("PONUM1"));
                    WriteLine("修改后采购订单，编号【{0}】，供应商【{1}】，合计金额【{2}】", po.PoNum, po.VenId, po.TotalAmount);
                    //_dbContext.Set<Po>().Remove(po);
                    //_dbContext.SaveChanges();
                }
            }

            **/
        }
    }
}
