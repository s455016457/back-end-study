using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test.lambda
{
    public class BuildDynamicQueriesTest2 : BaseTest
    {
        private List<People> peoples = new List<People>();
        private List<Teacher> teachers = new List<Teacher>();

        public BuildDynamicQueriesTest2()
        {
            initData();
        }

        public override void Test()
        {
            Test2();
        }

        private void Test1()
        {
            Expression<Func<People, Boolean>> where = p => (p.Amount > 4000 || p.Amount < 4500) && p.Name.Length > 2;
            ExpressionReplaceType<People, Teacher> replace = new ExpressionReplaceType<People, Teacher>();
            var newWhere = replace.ReplaceBodyType(where) as Expression<Func<Teacher, Boolean>>;
            WriteLine(new ParsingExpressionTrees().ParsingExpression(newWhere));
            foreach (Teacher item in teachers.Where(newWhere.Compile()))
            {
                WriteLine("Teacher：Name【{0}】，Amount【{1}】", item.Name, item.Amount);
            }
        }

        private void Test2()
        {
            var Parsing = new ParsingExpressionTrees();

            Expression<Func<People,Student, Boolean>> where = (p,s) => (p.Amount > 4000 ||s.Amount<400)&&p.Name.Length>3&&s.Name.Length>2;
            ExpressionReplaceType<Student, Teacher> replace1 = new ExpressionReplaceType<Student, Teacher>();
            var newWhere1 = replace1.ReplaceBodyType(where) as Expression<Func<People, Teacher, Boolean>>;
            var lambda1 = newWhere1.Compile();
            WriteLine(Parsing.ParsingExpression(newWhere1));
            ExpressionReplaceType<People, Teacher> replace2 = new ExpressionReplaceType<People, Teacher>();
            var newWhere2 = replace2.ReplaceBodyType(newWhere1) as Expression<Func<Teacher, Teacher, Boolean>>;
            var lambda2 =  newWhere2.Compile();
            WriteLine(Parsing.ParsingExpression(newWhere2));
        }

        #region TestData  

        private void initData()
        {
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

        public class Student
        {
            public String Name { get; set; }

            public Decimal Amount { get; set; }
        }
        #endregion
    }

    /// <summary>
    /// 替换表达式中指定类型
    /// </summary>
    /// <typeparam name="TSource">原类型</typeparam>
    /// <typeparam name="TResult">新类型</typeparam>
    public class ExpressionReplaceType<TSource, TResult> : ExpressionVisitor
    {
        private Dictionary<String, ParameterExpression> parameters = null;
        Expression result;
        public Expression ReplaceBodyType(Expression expression)
        {
            parameters = new Dictionary<string, ParameterExpression>();
            result = Visit(expression) ;
            return result;
        }

        /// <summary>
        /// 成员解析
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            return Expression.PropertyOrField(Visit(node.Expression), node.Member.Name);
        }

        /// <summary>
        /// Lambda解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            ////如果Lambda传入参数没有原类型，则不对Lambda进行转换
            //if (!node.Type.GetGenericArguments().Any(p => p == typeof(TSource))) return node;
            var body = Visit(node.Body);
            IList<ParameterExpression> parameters = new List<ParameterExpression>();
            foreach (var item in node.Parameters)
            {
                var newParamter = Visit(item) as ParameterExpression;
                if (newParamter == null) continue;
                parameters.Add(newParamter);
            }
            
            return Expression.Lambda(body, node.Name, parameters);
        }

        /// <summary>
        /// 变量解析
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            //如果变量不是源类型，则不对变量进行转换
            if (node.Type != typeof(TSource)) return node;
            String key = typeof(TResult).AssemblyQualifiedName + "**" + node.Name;
            ParameterExpression newParameterExpression = null;
            if (!parameters.TryGetValue(key, out newParameterExpression))
            {
                newParameterExpression = Expression.Parameter(typeof(TResult), node.Name);
                parameters.Add(key, newParameterExpression);
            }

            return newParameterExpression;
        }
    }
}
