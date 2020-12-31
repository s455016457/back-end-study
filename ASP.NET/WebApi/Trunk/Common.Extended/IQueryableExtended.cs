using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extended
{
    public static class IQueryableExtended
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="obj"></param>
        /// <param name="pageIndex">从1开始的页码</param>
        /// <param name="rowCount">每页行数</param>
        /// <param name="totaRows">总行数</param>
        /// <param name="totalPage">总页数</param>
        /// <returns></returns>
        public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> obj, int pageIndex, int rowCount, ref int totaRows, ref int totalPage)
        {
            if (pageIndex < 1) throw new Exception("当前页不能小于1！");
            totaRows = obj.Count();
            totalPage = (int)Math.Ceiling((decimal)totaRows / rowCount);

            if (pageIndex == 1)
                return obj.Take(rowCount);
            else if (totalPage == pageIndex)
                return obj.Skip((pageIndex - 1) * rowCount);
            else if (totalPage / 2 > pageIndex)
                return obj.Take(pageIndex * rowCount).Skip((pageIndex - 1) * rowCount);
            else
                return obj.Skip((pageIndex - 1) * rowCount).Take(rowCount);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="obj"></param>
        /// <param name="keySelector">排序字段</param>
        /// <param name="IsDescr">是否倒序</param>
        /// <returns></returns>
        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> obj, Expression<Func<TSource, Object>> keySelector, bool IsDescr)
        {
            if (IsDescr)
            {
                return obj.OrderByDescending(keySelector);
            }
            else
            {
                return obj.OrderBy(keySelector);
            }
        }
    }
}
