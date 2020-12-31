using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test.lambda
{
    public class BuildDynamicQueriesTest1 : BaseTest
    {
        ParsingExpressionTrees parsing = new ParsingExpressionTrees();
        ParsingExpressionTreeByVisitor parsing2 = new ParsingExpressionTreeByVisitor();
        private string[] companies = { "Consolidated Messenger", "Alpine Ski House", "Southridge Video", "City Power & Light",
                               "Coho Winery", "Wide World Importers", "Graphic Design Institute", "Adventure Works",
                               "Humongous Insurance", "Woodgrove Bank", "Margie's Travel", "Northwind Traders",
                               "Blue Yonder Airlines", "Trey Research", "The Phone Company",
                               "Wingtip Toys", "Lucerne Publishing", "Fourth Coffee" };
        public override void Test()
        {
            {
                Expression< Func<String, bool>> where = p => (p.ToLower() == "coho winery" || p.Length > 16);
                Expression<Func<String, int>> order = p => p.Length;

                 var Queryable = companies.Where(where.Compile()).OrderBy(order.Compile()).AsQueryable();
                WriteLine("companies.Where(company => (p.ToLower() == \"coho winery\" || p.Length > 16)).OrderBy(p => p.Length).AsQueryable():");
                foreach (var item in Queryable)
                {
                    WriteLine(item);
                }
                WriteLine("============================ParsingExpressionTrees==========================");
                WriteLine(parsing.ParsingExpression(where));
                WriteLine(parsing.ParsingExpression(order));
                WriteLine("======================================================");
                WriteLine("============================ParsingExpressionTreeByVisitor==========================");
                WriteLine(parsing2.Parsing(where));
                WriteLine(parsing2.Parsing(order));
                WriteLine("======================================================");
            }
            {
                var queryableData = companies.AsQueryable();
                var p1 = Expression.Parameter(typeof(IQueryable<String>), "queryableData");
                var p = Expression.Parameter(typeof(string), "p");
                var toLowerExp = Expression.Call(p, typeof(string).GetMethod("ToLower", new Type[0]));
                var lengthExp = Expression.Property(p, "Length");
                var exp1 = Expression.Equal(toLowerExp, Expression.Constant("coho winery",typeof(string)));
                var exp2 = Expression.GreaterThan(lengthExp, Expression.Constant(16, typeof(int)));

                var predicateBody = Expression.OrElse(exp1, exp2);

                //创建一个表达式表示：'queryableData.Where(p => (p.ToLower() == "coho winery" || p.Length > 16))
                MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { queryableData.ElementType },
                queryableData.Expression,
                Expression.Lambda<Func<string, bool>>(predicateBody, new ParameterExpression[] { p }));
                
                MethodCallExpression orderByCallExpression = Expression.Call(
                    typeof(Queryable),
                    "OrderBy",
                    new Type[] { queryableData.ElementType, typeof(int) },
                    whereCallExpression,
                    Expression.Lambda<Func<String,int>>(lengthExp, new ParameterExpression[] { p }));


                WriteLine("============================ParsingExpressionTrees==========================");
                WriteLine(parsing.ParsingExpression(orderByCallExpression));
                WriteLine("======================================================");
                WriteLine("============================ParsingExpressionTreeByVisitor==========================");
                WriteLine(parsing2.Parsing(orderByCallExpression));
                WriteLine("======================================================");
                var Queryable = queryableData.Provider.CreateQuery<string>(orderByCallExpression);
                foreach (var item in Queryable)
                {
                    WriteLine(item);
                }

            }
        }
    }
}
