using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test.lambda
{
    public class LambdaTest1 : BaseTest
    {
        public override void Test()
        {
            test1();
        }

        public void test1()
        {
            Expression<Func<People, Decimal>> calc = p => p.Amount * 10;
            WriteLine("calc.ToString():" + calc.ToString());
            WriteLine("calc.Body:{0}", calc.Body);
            WriteLine("calc.Body.Type:{0}", calc.Body.Type);
            WriteLine("calc.Compile():{0}", calc.Compile());
            WriteLine("calc.Name:{0}", calc.Name);
            WriteLine("calc.NodeType:{0}", calc.NodeType);
            WriteLine("calc.Parameters:{0}", calc.Parameters);
            int i = 0;
            foreach (var item in calc.Parameters)
            {
                ++i;
                WriteLine("calc.Parameter{0}.Name:{1}", i, item.Name);
                WriteLine("calc.Parameter{0}.NodeType:{1}", i, item.NodeType);
                WriteLine("calc.Parameter{0}.Type:{1}", i, item.Type);
            }
            WriteLine("===================================================================");

            Expression<Func<People, bool>> peopleWhere = p => p.Amount >= 100 && p.Amount <= 200;

            WriteLine("peopleWhere:{0}", peopleWhere);
            var lambda = peopleWhere.Body as LambdaExpression;
            WriteLine(lambda);

            Teacher teacher = new Teacher()
            {
                Name = "TeacherTest",
                Amount = 1000
            };

            var paramter = Expression.Parameter(typeof(Teacher),"p");

            MemberExpression memberExpression = Expression.Property(paramter, "Amount");
            var a1= Expression.GreaterThanOrEqual(memberExpression, Expression.Constant(100M,typeof(decimal)));
            var a2= Expression.LessThanOrEqual(memberExpression, Expression.Constant(2000M,typeof(decimal)));
            var a = Expression.AndAlso(a1, a2);
            WriteLine("===================================================================");
            var b = Expression.Lambda(a, paramter);
            WriteLine("b:" + b);
            WriteLine("===================================================================");
            var c = Expression.Lambda<Func<Teacher, Boolean>>(a, paramter);
            WriteLine("c:" + c);


            WhereExpression where = new WhereExpression(peopleWhere);

            var tWhere = where.Replace(typeof(Teacher));
            WriteLine("tWhere:" + tWhere);
            WriteLine("tWhere.Compile().DynamicInvoke(teacher):" + tWhere.Compile().DynamicInvoke(teacher));
        }

        public class WhereExpression : ExpressionVisitor
        {
            protected LambdaExpression SourceExpression { get; set; }
            private ParameterExpression Parameter;
            public WhereExpression(LambdaExpression expression)
            {
                SourceExpression = expression;
            }
            public LambdaExpression Replace(Type targetType)
            {
                if (SourceExpression == null)
                    return null;

                Parameter = Expression.Parameter(targetType, SourceExpression.Parameters[0].Name);

                Expression body = Visit(SourceExpression.Body);
                SourceExpression = Expression.Lambda(body, Parameter);
                return SourceExpression;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                return Expression.Parameter(Parameter.Type, node.Member.Name);
            }
            
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
    }
}
