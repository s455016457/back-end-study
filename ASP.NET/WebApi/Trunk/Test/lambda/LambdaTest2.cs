using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Test.lambda
{
    public class LambdaTest2 : BaseTest
    {
        public override void Test()
        {
            Test1();
            Test2();
            TestTypeBinaryExpression();
            TestIndexExpression();
            TestNewExpression();
            TestInvocationExpression();
            TestSwitchExpression();
        }

        /**
         * BlockExpression  代码块，其中包含给定的变量和表达式
         * ParameterExpression 该节点可用于标识表达式树中的参数或变量
         * BinaryExpression 二元运算
         * ConstantExpression 为属性赋值
         * LambdaExpression 描述一个 lambda 表达式。这将捕获与 .NET 方法体类似的代码块
         * */

        private void Test1()
        {
            ParameterExpression number = Expression.Parameter(typeof(int), "number");

            //表示一个包含可在其中定义变量的表达式序列的块
            //用来创建代码块，可以执行多组代码，在它自身的理解里面，是可以包含多个Expression的子类
            BlockExpression myBlock = Expression.Block(
                new[] { number },//定义代码块中的变量
                Expression.Assign(number, Expression.Constant(2)),//给number赋值
                Expression.AddAssign(number, Expression.Constant(6)),//加法算法
                Expression.DivideAssign(number, Expression.Constant(2))//除法算法
            );

            Expression<Func<int>> myAction = Expression.Lambda<Func<int>>(myBlock);//生成表达式树
            WriteLine(myAction.Compile()());
        }

        /**
         * LabelTarget 用来实现GoTo
         * LoopExpression 循环
         * ConditionalExpression 条件表达式
         * MethodCallExpression 表示对静态方法或实例方法的调用
         * UnaryExpression 表示包含一元运算符的表达式 比如：i++,i--,++i,--i
         * GotoExpression  表示无条件跳转。这包括 return 语句、break 和 continue 语句以及其他跳转。
         * */
        private void Test2()
        {
            LabelTarget labelBreak = Expression.Label();
            ParameterExpression Index = Expression.Parameter(typeof(int), "Index");
            BlockExpression myBlock = Expression.Block(
                new[] { Index }//定义代码块中的变量
                , Expression.Assign(Index, Expression.Constant(1))//给Index赋值
                , Expression.Loop(//循环
                    Expression.IfThenElse(//条件表达式
                        Expression.LessThanOrEqual(Index, Expression.Constant(10))//条件
                        , Expression.Block(
                            Expression.Call(//调用方法
                                null
                                , typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) }),
                                Expression.Constant("Hello")
                            )
                            , Expression.PostIncrementAssign(Index)//递增1
                        )
                        , Expression.Break(labelBreak)//     创建一个表示 break 语句的 System.Linq.Expressions.GotoExpression 将跳至的 System.Linq.Expressions.LabelTarget。
                    )
                    , labelBreak
                )
           );
            Expression<Action> lambdaExpression = Expression.Lambda<Action>(myBlock);//生成表达式树
            lambdaExpression.Compile().Invoke();//执行
        }

        /**
         * TypeBinaryExpression 表示表达式和类型之间的操作
         * */
        private void TestTypeBinaryExpression()
        {
            TypeBinaryExpression typeBinaryExpression = Expression.TypeIs(Expression.Constant("spruce"), typeof(int));

            object obj = DBNull.Value;
            ParameterExpression value = Expression.Parameter(typeof(Object), "value");

            var typeBinaryExpression2 = Expression.Block(
                 new[] { value }//定义代码块中的变量
                , Expression.Assign(value, Expression.Constant(obj))//给 value 赋值
                , Expression.TypeIs(value, typeof(DBNull))
            );


            WriteLine(typeBinaryExpression.ToString());// ("spruce" Is Int32)
            WriteLine(typeBinaryExpression2.Result);// (value Is DBNull)

            WriteLine(Expression.Lambda(typeBinaryExpression).Compile().DynamicInvoke());
            WriteLine(Expression.Lambda(typeBinaryExpression2).Compile().DynamicInvoke());
        }

        /**
         * IndexExpression 表示编制属性或数组的索引。
         * */
        private void TestIndexExpression()
        {
            var arrayExp = Expression.Parameter(typeof(String[]), "Array");
            var indexExp = Expression.Parameter(typeof(int), "Index");
            var valueExp = Expression.Parameter(typeof(String), "Value");
            var arrayAccessExpr = Expression.ArrayAccess(arrayExp, indexExp);// 创建一个用于访问数组的 System.Linq.Expressions.IndexExpression。
            Expression<Func<String[], int, String>> funcExp = Expression.Lambda<Func<String[], int, String>>(
                arrayAccessExpr
                , arrayExp
                , indexExp
            );

            Console.WriteLine(arrayAccessExpr.ToString());// Array[Index]

            Console.WriteLine(funcExp.ToString()); // (Array, Index) => (Array[Index]) 

            Console.WriteLine(funcExp.Compile().Invoke(new string[] { "AA", "BB", "CC" }, 1));// BB

            Expression<Func<String[], int, String, String>> funcExp2 = Expression.Lambda<Func<String[], int, String, String>>(
                Expression.Assign(
                    arrayAccessExpr
                    , Expression.Call(
                        typeof(String).GetMethod("Concat", new Type[] { typeof(String), typeof(String) })
                        , arrayAccessExpr
                        , valueExp
                    )
                )
                , arrayExp
                , indexExp
                , valueExp
            );

            WriteLine(arrayAccessExpr.ToString());// Array[Index]

            WriteLine(funcExp2.ToString()); // (Array, Index, Value) => (Array[Index] = Concat(Array[Index], Value))

            WriteLine(funcExp2.Compile().Invoke(new string[] { "AA", "BB", "CC" }, 1, "DD"));// BBDD
        }

        /**
         * NewExpression 表示构造函数调用
         * */
        private void TestNewExpression()
        {
            var newDictionaryExpression = Expression.New(typeof(Dictionary<String, Object>));//创建一个表示调用指定类型的无参数构造函数的 System.Linq.Expressions.NewExpression。
            WriteLine(newDictionaryExpression);// new Dictionary`2() Dictionary<String, Object>)的构造函数
        }

        /**
         * InvocationExpression 表示将委托或 lambda 表达式应用于参数表达式列表的表达式。
         * */
        private void TestInvocationExpression()
        {
            Expression<Func<int, int, bool>> largeSumTest = (num1, num2) => num1 + num2 > 1000;
            InvocationExpression invocationExpression = Expression.Invoke(//创建一个 System.Linq.Expressions.InvocationExpression，它将委托或 lambda 表达式应用于一个参数表达式列表。
                largeSumTest
                , Expression.Constant(450)
                , Expression.Constant(780)
            );
            WriteLine(invocationExpression);//Invoke((num1, num2) => ((num1 + num2) > 1000), 450, 780)
            WriteLine(Expression.Lambda(invocationExpression).Compile().DynamicInvoke());//True
        }

        private void TestSwitchExpression()
        {
            ParameterExpression valueExp = Expression.Parameter(typeof(int), "Value");
            var switchExp = Expression.Switch(
                valueExp
                , Expression.Constant("未知")
                , Expression.SwitchCase(Expression.Constant("男"), Expression.Constant(1))
                , Expression.SwitchCase(Expression.Constant("女"), Expression.Constant(0))
            );
            Expression<Func<int, string>> expr = Expression.Lambda<Func<int, string>>(switchExp, valueExp);
            WriteLine("expr:{0}", expr);//expr:Value => switch (Value) { ... }
            WriteLine("expr.Compile().Invoke(1):{0}", expr.Compile().Invoke(1));//expr.Compile().Invoke(1):男
            WriteLine("expr.Compile().Invoke(0):{0}", expr.Compile().Invoke(0));//expr.Compile().Invoke(0):女
            WriteLine("expr.Compile().Invoke(11):{0}", expr.Compile().Invoke(11));//expr.Compile().Invoke(11):未知

            //提供有关方法和构造函数的信息。
            WriteLine(MethodBase.GetCurrentMethod());//返回表示当前正在执行的方法的 MethodBase 对象。
        }
    }
}
