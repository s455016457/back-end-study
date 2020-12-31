using System;
using System.Linq.Expressions;
using System.Text;

namespace Commmon
{
    public class ParsingExpressionTrees
    {
        /// <summary>
        /// 解析树状结构
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public String ParsingExpression(Object exp)
        {
            String str = String.Empty;

            if (exp is BlockExpression) //exp.GetType().IsSubclassOf(typeof(BlockExpression)) 效果相同
                str = ParsingBlockExpression((BlockExpression)exp);
            else if (exp is MemberExpression)
                str = ParsingMemberExpression((MemberExpression)exp);
            else if (exp is ConditionalExpression)
                str = ParsingConditionalExpression((ConditionalExpression)exp);
            else if (exp is LoopExpression)
                str = ParsingLoopExpression((LoopExpression)exp);
            else if (exp is BinaryExpression)
                str = ParsingBinaryExpression((BinaryExpression)exp);
            else if (exp is ParameterExpression)
                str = ParsingParameterExpression((ParameterExpression)exp);
            else if (exp is ConstantExpression)
                str = ParsingConstantExpression((ConstantExpression)exp);
            else if (exp is UnaryExpression)
                str = ParsingUnaryExpression((UnaryExpression)exp);
            else if (exp is GotoExpression)
                str = ParsingGotoExpression((GotoExpression)exp);
            else if (exp is LambdaExpression)
                str = ParsingLambdaExpression((LambdaExpression)exp);
            else if (exp is MethodCallExpression)
                str = ParsingMethodCallExpression((MethodCallExpression)exp);
            else if (exp is LabelTarget)
                str = ParsingLabelTarget((LabelTarget)exp);
            else if (exp is ExpressionType)
                str = ParsingExpressionType((ExpressionType)exp);

            return str;
        }

        private String ParsingMethodCallExpression(MethodCallExpression methodCall)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}.", ParsingExpression(methodCall.Object));
            sb.Append(methodCall.Method.Name);
            sb.Append("(");
            var isFirst = true;
            foreach (var item in methodCall.Arguments)
            {
                sb.AppendFormat("{0}", ParsingExpression(item));
                if (isFirst) { sb.Append(","); isFirst = false; }
            }
            sb.Append(")");
            return sb.ToString();
        }

        private String ParsingMemberExpression(MemberExpression member)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}.{1}", ParsingExpression(member.Expression), member.Member.Name);
            return sb.ToString();
        }
        /// <summary>
        /// 解析Lambda表达式
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        private String ParsingLambdaExpression(LambdaExpression lambda)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            Boolean isFirst = true;
            foreach (var item in lambda.Parameters)
            {
                if (!isFirst) sb.Append(",");
                sb.AppendFormat("{0}", item.Name);
                isFirst = false;
            }
            sb.Append(") => ");
            sb.Append(ParsingExpression(lambda.Body));
            return sb.ToString();
        }

        private String ParsingLabelTarget(LabelTarget label)
        {
            return label.Name;
        }
        /// <summary>
        /// 解析代码块
        /// </summary>
        /// <param name="bolck"></param>
        /// <returns></returns>
        private String ParsingBlockExpression(BlockExpression bolck)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");

            foreach (var item in bolck.Variables)
            {
                sb.AppendLine(ParsingExpression(item));
            }

            foreach (var item in bolck.Expressions)
            {
                sb.AppendLine(ParsingExpression(item));
            }
            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// 解析循环语句
        /// </summary>
        /// <param name="loop"></param>
        /// <returns></returns>
        private String ParsingLoopExpression(LoopExpression loop)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}", ParsingExpression(loop.NodeType));
            sb.AppendLine("{");
            sb.AppendLine(ParsingExpression(loop.Body));
            sb.AppendLine(ParsingExpression(loop.BreakLabel));
            sb.Append("}");
            return sb.ToString();
        }
        /// <summary>
        /// 解析条件表达式
        /// </summary>
        /// <param name="conditional"></param>
        /// <returns></returns>
        private String ParsingConditionalExpression(ConditionalExpression conditional)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("if ({0}) ", ParsingExpression(conditional.Test));
            sb.AppendLine("{ ");
            sb.AppendLine(ParsingExpression(conditional.IfTrue));
            sb.AppendLine("}");
            sb.AppendLine("else {");
            sb.AppendLine(ParsingExpression(conditional.IfFalse));
            sb.Append("}");
            return sb.ToString();
        }
        /// <summary>
        /// 解析Goto
        /// </summary>
        /// <param name="Goto"></param>
        /// <returns></returns>
        private String ParsingGotoExpression(GotoExpression Goto)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1} {2}", ParsingExpression(Goto.NodeType), ParsingExpression(Goto.Target), ParsingExpression(Goto.Value));
            return sb.ToString();
        }
        /// <summary>
        /// 解析二元计算
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        private String ParsingBinaryExpression(BinaryExpression body)
        {
            StringBuilder sb = new StringBuilder();
            var left = body.Left;
            var right = body.Right;

            sb.AppendFormat("{0} {1} {2}", ParsingExpression(left), ParsingExpression(body.NodeType), ParsingExpression(right));
            return sb.ToString();
        }
        /// <summary>
        /// 解析一元计算
        /// </summary>
        /// <param name="unary"></param>
        /// <returns></returns>
        private String ParsingUnaryExpression(UnaryExpression unary)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} {1}", ParsingExpression(unary.Operand), ParsingExpression(unary.NodeType));


            return sb.ToString();
        }

        private String ParsingExpressionType(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.And:
                    return "&";
                case ExpressionType.Or:
                    return "|";
                case ExpressionType.AndAlso:
                    return "&&";
                case ExpressionType.OrElse:
                    return "||";
                case ExpressionType.Equal:
                    return "==";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.PostDecrementAssign:
                    return "--";
                case ExpressionType.PostIncrementAssign:
                    return "++";
                case ExpressionType.Add:
                    return "+";
                case ExpressionType.AddAssign:
                    return "+=";
                case ExpressionType.Subtract:
                    return "-";
                case ExpressionType.SubtractAssign:
                    return "-=";
                case ExpressionType.Multiply:
                    return "*";
                case ExpressionType.MultiplyAssign:
                    return "*=";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.DivideAssign:
                    return "/=";
                case ExpressionType.Assign:
                    return "=";
                case ExpressionType.RightShift:
                    return ">>";
                case ExpressionType.Goto:
                    return "GOTO";
                case ExpressionType.Loop:
                    return "Loop";
                case ExpressionType.Quote:
                    return String.Empty;
                default:
                    return "未知类型" + type.ToString();
            }
        }

        private String ParsingParameterExpression(ParameterExpression parameter)
        {
            return parameter.Name.ToString();
        }

        private String ParsingConstantExpression(ConstantExpression parameter)
        {
            if (parameter.Type.Equals(typeof(String))) return String.Format("\"{0}\"", parameter.Value.ToString());
            return parameter.Value.ToString();
        }

    }

    public class ParsingExpressionTreeByVisitor:ExpressionVisitor
    {
        StringBuilder sb = new StringBuilder();

        public String Parsing(Expression exprossion)
        {
            sb.Length = 0;
            Visit(exprossion);
            return sb.ToString();
        }
        /// <summary>
        /// 代码块
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBlock(BlockExpression node)
        {
            sb.AppendLine("{");

            foreach (var item in node.Variables)
            {
                Visit(item);
                sb.AppendLine();
            }

            foreach (var item in node.Expressions)
            {
                Visit(item);
                sb.AppendLine();
            }
            sb.Append("}");

            return node;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            sb.Append("(");
            Boolean isFirst = true;
            foreach (var item in node.Parameters)
            {
                if (!isFirst) sb.Append(",");
                Visit(item);
                isFirst = false;
            }
            sb.Append(") => ");
            Visit(node.Body);

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Visit(node.Object);
            sb.AppendFormat(".");
            sb.Append(node.Method.Name);
            sb.Append("(");
            var isFirst = true;
            foreach (var item in node.Arguments)
            {
                Visit(item);
                if (isFirst) { sb.Append(","); isFirst = false; }
            }
            sb.Append(")");

            return node;
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            ParsingExpressionType(node.NodeType);
            sb.AppendLine("{");
            Visit(node.Body);
            sb.AppendLine();
            VisitLabelTarget(node.BreakLabel);
            sb.Append("}");
            return node;
        }

        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            if (node != null) sb.Append(node.Name);
            return node;
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            sb.Append("if (");
            Visit(node.Test);
            sb.AppendLine("){");
            Visit(node.IfTrue);
            sb.AppendLine();
            sb.AppendLine("}");
            sb.AppendLine("else {");
            Visit(node.IfFalse);
            sb.AppendLine();
            sb.Append("}");
            return node;
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            ParsingExpressionType(node.NodeType);
            sb.Append(" ");
            VisitLabelTarget(node.Target);
            sb.Append(" ");
            Visit(node.Value);
            return node;
        }

        /// <summary>
        /// Switch
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitSwitch(SwitchExpression node)
        {
            ParsingExpressionType(node.NodeType);
            sb.Append("(");
            Visit(node.SwitchValue);
            sb.Append(") {");
            sb.AppendLine();
            foreach (var item in node.Cases)
            {
                VisitSwitchCase(item);
                sb.AppendLine();
            }
            Visit(node.DefaultBody);
            sb.AppendLine();
            sb.Append("}");

            return node;
        }

        /// <summary>
        /// case
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            sb.Append("case ");
            Visit(node.TestValues);
            sb.Append(":");
            Visit(node.Body);
            return node;
        }

        /// <summary>
        /// 二元运算
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            sb.Append(" ");
            ParsingExpressionType(node.NodeType);
            sb.Append(" ");
            Visit(node.Right);

            return node;
        }
        protected override Expression VisitMember(MemberExpression node)
        {
            Visit(node.Expression);
            sb.AppendFormat(".{0}", node.Member.Name);

            return node;
        }

        /// <summary>
        /// 一元运算
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            Visit(node.Operand);
            ParsingExpressionType(node.NodeType);
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.IsByRef) sb.AppendFormat("ref {0}",node.Name);
            else sb.Append(node.Name);
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type == typeof(String)) sb.AppendFormat("\"{0}\"", node.Value);
            else sb.Append(node.Value);
            return node;
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            return base.VisitTypeBinary(node);
        }

        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            return base.VisitCatchBlock(node);
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            return base.VisitListInit(node);
        }

        private ExpressionType ParsingExpressionType(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.And:
                    sb.Append("&");
                    return type;
                case ExpressionType.Or:
                    sb.Append("|");
                    return type;
                case ExpressionType.AndAlso:
                    sb.Append("&&");
                    return type;
                case ExpressionType.OrElse:
                    sb.Append("||");
                    return type;
                case ExpressionType.Equal:
                    sb.Append("==");
                    return type;
                case ExpressionType.NotEqual:
                    sb.Append("<>");
                    return type;
                case ExpressionType.LessThan:
                    sb.Append("<");
                    return type;
                case ExpressionType.LessThanOrEqual:
                    sb.Append("<=");
                    return type;
                case ExpressionType.GreaterThan:
                    sb.Append(">");
                    return type;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(">=");
                    return type;
                case ExpressionType.PostDecrementAssign:
                    sb.Append("--");
                    return type;
                case ExpressionType.PostIncrementAssign:
                    sb.Append("++");
                    return type;
                case ExpressionType.Add:
                    sb.Append("+");
                    return type;
                case ExpressionType.AddAssign:
                    sb.Append("+=");
                    return type;
                case ExpressionType.Subtract:
                    sb.Append("-");
                    return type;
                case ExpressionType.SubtractAssign:
                    sb.Append("-=");
                    return type;
                case ExpressionType.Multiply:
                    sb.Append("*");
                    return type;
                case ExpressionType.MultiplyAssign:
                    sb.Append("*=");
                    return type;
                case ExpressionType.Divide:
                    sb.Append("/");
                    return type;
                case ExpressionType.DivideAssign:
                    sb.Append("/=");
                    return type;
                case ExpressionType.Assign:
                    sb.Append("=");
                    return type;
                case ExpressionType.RightShift:
                    sb.Append(">>");
                    return type;
                case ExpressionType.Goto:
                    sb.Append("GOTO");
                    return type;
                case ExpressionType.Loop:
                    sb.Append("Loop");
                    return type;
                case ExpressionType.Quote:
                    sb.Append(String.Empty);
                    return type;
                default:
                    sb.Append("未知类型" + type.ToString());
                    return type;
            }
        }
    }
}
