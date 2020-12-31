using IInfrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IInfrastructure.IRepositories
{
    public interface IBaseRepository<T>
    {
        T GetModel(params object[] Keys);
        IList<T> GetList(Expression<Func<T, bool>> predicate);
        IList<T> GetList(Expression<Func<T, bool>> predicate, string order, string sort);
        IList<T> GetList(Expression<Func<T, bool>> predicate, GridPager gridPager);
    }
}
