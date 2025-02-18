using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Text;
using BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsExpressionBuilder : IExpressionBuilder
{
    private readonly IMethodCallTranslatorFactory _methodCallTranslatorFactory;
    private readonly IMemberTranslatorFactory _memberTranslatorFactory;
    private readonly IBinaryTranslatorFactory _binaryTranslatorFactory;
    private readonly StringBuilder _builder = new();

    public JsExpressionBuilder(IMethodCallTranslatorFactory methodCallTranslatorFactory, IMemberTranslatorFactory memberTranslatorFactory, IBinaryTranslatorFactory binaryTranslatorFactory)
    {
        ArgumentNullException.ThrowIfNull(methodCallTranslatorFactory);
        ArgumentNullException.ThrowIfNull(memberTranslatorFactory);
        ArgumentNullException.ThrowIfNull(binaryTranslatorFactory);
        _methodCallTranslatorFactory = methodCallTranslatorFactory;
        _memberTranslatorFactory = memberTranslatorFactory;
        _binaryTranslatorFactory = binaryTranslatorFactory;
    }

    public string ProcessExpression(LambdaExpression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        _builder.Clear();
        InternalProcessExpression(expression);
        return _builder.ToString();
    }

    private void InternalProcessExpression(Expression expression)
    {
        switch (expression)
        {
            case BinaryExpression binary:
                JsBinaryExpressionBuilder.AppendBinary(_builder, _binaryTranslatorFactory, binary, InternalProcessExpression);
                break;
            case ConstantExpression constant:
                JsConstantExpressionBuilder.AppendConstant(_builder, constant);
                break;
            case MemberExpression member:
                JsMemberExpressionBuilder.AppendMember(_builder, _memberTranslatorFactory, member, InternalProcessExpression);
                break;
            case UnaryExpression unary:
                JsUnaryExpressionBuilder.AppendUnary(_builder, unary, InternalProcessExpression);
                break;
            case LambdaExpression lambda:
                JsLambdaExpressionBuilder.AppendLambda(_builder, lambda, InternalProcessExpression);
                break;
            case MethodCallExpression methodCall:
                JsMethodExpressionBuilder.AppendMethod(_builder, _methodCallTranslatorFactory, methodCall, InternalProcessExpression);
                break;
            case ParameterExpression parameter:
                JsParameterExpressionBuilder.AppendParameter(_builder, parameter, InternalProcessExpression);
                break;
        }
    }
}