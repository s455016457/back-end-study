using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Test.lambda
{
    public class TestQueryExtensions : BaseTest
    {
        private List<People> peoples = new List<People>();
        private List<Teacher> teachers = new List<Teacher>();

        public TestQueryExtensions() : base()
        {
            initData();
        }

        public override void Test()
        {
            WriteLine(peoples.AsQueryable().Where(p => p.Amount >= 400 || p.Name == ""));
            WriteLine(teachers.AsQueryable().Where(p => p.Amount >= 4500 && p.Name == ""));
        }

        #region TestData  

        private void initData() {
            peoples.Add(new People() { Name = "张三", Amount = 300 });
            peoples.Add(new People() { Name = "李四", Amount = 400 });
            peoples.Add(new People() { Name = "王五", Amount = 500 });

            teachers.Add(new Teacher() { Name = "Teacher1", Amount = 4000 });
            teachers.Add(new Teacher() { Name = "Teacher2", Amount = 4500 });
            teachers.Add(new Teacher() { Name = "Teacher3", Amount = 5000 });
        }

        public class People
        {
            public String Name { get; set; }

            public Decimal Amount { get; set; }
        }

        public class Teacher
        {
            public String Name { get; set; }

            public Decimal Amount { get; set; }
        }
        #endregion
    }


    /// <summary>
    /// 自己的Query扩展
    /// </summary>
    public static class QueryExtensions
    {
        public static string Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            var expression = Expression.Call(
                /**
                 * MethodBase 提供有关方法和构造函数的信息
                 * GetCurrentMethod 返回表示当前正在执行的方法的 MethodBase 对象
                 * MakeGenericMethod 用类型数组的元素替代当前泛型方法定义的类型参数，并返回表示结果构造方法的 System.Reflection.MethodInfo 对象。
                 * */
                ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) })
                , source.Expression//获取与 System.Linq.IQueryable 的实例关联的表达式目录树。
                , Expression.Quote(predicate) //创建一个表示具有类型 System.Linq.Expressions.Expression 的常量值的表达式的 一元表达式 System.Linq.Expressions.UnaryExpression。
            );

            //Console.WriteLine(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }));
            var translator = new QueryTranslator();
            return translator.Translate(expression);
        }
    }

    public class QueryTranslator : ExpressionVisitor
    {
        protected StringBuilder sb;
        internal string Translate(Expression expression)
        {
            sb = new StringBuilder();
            Visit(expression);
            return sb.ToString();
        }

        /// <summary>
        /// 方法调用重写
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name.ToUpper().Equals("WHERE"))
            {
                sb.AppendFormat(
                    "SELECT * FROM ( {0} ) AS T WHERE "
                    ,node.Arguments[0].ToString().ToUpper()
                    );
                LambdaExpression lambda = Expression.Lambda(node.Arguments[1]);
                Visit(lambda.Body);
                return node;
            }
            throw new NotSupportedException(string.Format("方法{0}不支持", node.Method.Name));
        }

        /// <summary>
        /// 二元计算重写
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            sb.Append("(");
            this.Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.And:
                    sb.Append(" AND ");
                    break;
                case ExpressionType.Or:
                    sb.Append(" OR");
                    break;
                case ExpressionType.AndAlso:
                    sb.Append(" AND");
                    break;
                case ExpressionType.OrElse:
                    sb.Append(" OR");
                    break;
                case ExpressionType.Equal:
                    sb.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    sb.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("二元运算符{0}不支持", b.NodeType));
            }
            Visit(b.Right);
            sb.Append(")");
            return b;
        }

        /// <summary>
        /// 常量重写
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;
            if (q != null)
            {
                // 我们假设我们那个Queryable就是对应的表
                sb.Append("SELECT * FROM ");
                sb.Append(q.ElementType.Name);
            }
            else if (c.Value == null)
            {
                sb.Append("NULL");
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)c.Value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("常量'{0}'不受支持", c.Value));
                    default:
                        sb.Append(c.Value);
                        break;
                }
            }
            return c;
        }

        /// <summary>
        /// 成员重写
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(m.Member.Name);
                return m;
            }
            throw new NotSupportedException(string.Format("成员 '{0}'不受支持", m.Member.Name));
        }
    }
}
