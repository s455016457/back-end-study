using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test.lambda
{
    public class ParsingExpressionTreesTest : BaseTest
    {
        public override void Test()
        {
            Test1();
            Test2();
        }

        private void Test1()
        {
            var ParsingExpressionTrees = new ParsingExpressionTrees();
            var ParasingExpressionTreeByVisitor = new ParsingExpressionTreeByVisitor();
            {
                ParameterExpression valueExp = Expression.Parameter(typeof(int), "Value");      //创建参数表达式 Value 
                ParameterExpression resultExp = Expression.Parameter(typeof(int), "Result");    //创建参数表达式 Result 
                LabelTarget lableExp = Expression.Label(typeof(int), "Break");                  //创建一个标签以从循环跳转到，接收一个int类型的值
                BlockExpression blockExp = Expression.Block(                                    //创建代码块
                    new[] { resultExp }                                                         //定义局部变量
                    , Expression.Assign(resultExp, Expression.Constant(1))                      //给变量赋值
                    , Expression.Loop(                                                          //定义循环体
                        Expression.IfThenElse(                                                  //创建条件表达式 if else
                            Expression.GreaterThanOrEqual(valueExp, Expression.Constant(1))     //valueExp>=1
                            , Expression.MultiplyAssign(resultExp, Expression.PostDecrementAssign(valueExp))// if true resultExp*=valueExp--
                            , Expression.Break(lableExp, resultExp)                             // if else 跳转到lableExp，并将resultExp传入到跳转到lableExp
                        )
                     , lableExp                                                                 // 跳转到lableExp
                     )
                );
                var lambda = Expression.Lambda<Func<int, int>>(blockExp, valueExp);
                int value = lambda.Compile().Invoke(6);
                WriteLine("{0}:{1}", lambda.Body, value);

                WriteLine("lambda:{0}", ParsingExpressionTrees.ParsingExpression(lambda));

                WriteLine("==============================================================================");
                WriteLine("ParasingExpressionTreeByVisitor:");
                WriteLine("{0}",ParasingExpressionTreeByVisitor.Parsing(lambda));
                WriteLine("==============================================================================");
            }

            {
                Expression<Func<int, int>> lambda = num => num + 100;
                WriteLine("lambda :{0} ", ParsingExpressionTrees.ParsingExpression(lambda));
            }
        }

        private void Test2()
        {
            var ParsingExpressionTrees = new ParsingExpressionTreeByVisitor();


            ParameterExpression pExp = Expression.Parameter(typeof(String), "p");      //创建参数表达式 p 
            ConstantExpression aExp = Expression.Constant("A", typeof(String));
            ConstantExpression bExp = Expression.Constant("B", typeof(String));
            ConstantExpression cExp = Expression.Constant("C", typeof(String));
            ConstantExpression dExp = Expression.Constant("D", typeof(String));

            SwitchExpression switchExp = Expression.Switch(pExp
                , Expression.Constant("未知")
                , Expression.SwitchCase(Expression.Constant("aaaa", typeof(String)), aExp)
                , Expression.SwitchCase(Expression.Constant("bbbb", typeof(String)), bExp)
                , Expression.SwitchCase(Expression.Constant("cccc", typeof(String)), cExp)
                , Expression.SwitchCase(Expression.Constant("dddd", typeof(String)), dExp)
            );

            LambdaExpression lambdaExp = Expression.Lambda(switchExp, pExp);
            String strLambda = ParsingExpressionTrees.Parsing(switchExp);
            WriteLine("{0}",strLambda);
        }
    }

}
