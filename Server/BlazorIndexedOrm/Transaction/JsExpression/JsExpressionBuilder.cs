using System.Linq.Expressions;
using System.Text;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsExpressionBuilder
{
    private readonly IJsMethodCallTranslatorFactory _methodCallTranslatorFactory;
    private readonly StringBuilder _builder = new();

    public JsExpressionBuilder(IJsMethodCallTranslatorFactory methodCallTranslatorFactory)
    {
        ArgumentNullException.ThrowIfNull(methodCallTranslatorFactory);
        _methodCallTranslatorFactory = methodCallTranslatorFactory;
    }

    public string Result => _builder.ToString();
    internal void ProcessExpression(Expression expression)
    {
        switch (expression)
        {
            case BinaryExpression binary:
                JsBinaryExpressionBuilder.AppendEquality(_builder, binary, ProcessExpression);
                break;
            case ConstantExpression constant:
                JsConstantExpressionBuilder.AppendConstant(_builder, constant);
                break;
            case MemberExpression member:
                JsMemberExpressionBuilder.AppendMember(_builder, member, ProcessExpression);
                break;
            case UnaryExpression unary:
                JsUnaryExpressionBuilder.AppendUnary(_builder, unary, ProcessExpression);
                break;
            case LambdaExpression lambda:
                JsLambdaExpressionBuilder.AppendLambda(_builder, lambda, ProcessExpression);
                break;
            case MethodCallExpression methodCall:
                JsMethodExpressionBuilder.AppendMethod(_builder, _methodCallTranslatorFactory, methodCall, ProcessExpression);
                break;
            case ParameterExpression parameter:
                JsParameterExpressionBuilder.AppendParameter(_builder, parameter, ProcessExpression);
                break;
        }
    }
}