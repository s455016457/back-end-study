using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Commmon
{
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
            result = Visit(expression);
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
