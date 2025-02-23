using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Text;
using Blido.Core.Transaction.JsExpression.BinaryTranslation;
using Blido.Core.Transaction.JsExpression.MemberTranslation;
using Blido.Core.Transaction.JsExpression.MethodCallTranslation;
using Blido.Core.Transaction.JsExpression.UnaryTranslation;

namespace Blido.Core.Transaction.JsExpression;

public class JsExpressionBuilder : IExpressionBuilder
{
    private readonly IMethodCallTranslatorFactory _methodCallTranslatorFactory;
    private readonly IMemberTranslatorFactory _memberTranslatorFactory;
    private readonly IBinaryTranslatorFactory _binaryTranslatorFactory;
    private readonly IUnaryTranslatorFactory _unaryTranslatorFactory;
    private readonly StringBuilder _builder = new();

    public JsExpressionBuilder(IMethodCallTranslatorFactory methodCallTranslatorFactory,
        IMemberTranslatorFactory memberTranslatorFactory,
        IBinaryTranslatorFactory binaryTranslatorFactory,
        IUnaryTranslatorFactory unaryTranslatorFactory)
    {
        ArgumentNullException.ThrowIfNull(methodCallTranslatorFactory);
        ArgumentNullException.ThrowIfNull(memberTranslatorFactory);
        ArgumentNullException.ThrowIfNull(binaryTranslatorFactory);
        ArgumentNullException.ThrowIfNull(unaryTranslatorFactory);
        _methodCallTranslatorFactory = methodCallTranslatorFactory;
        _memberTranslatorFactory = memberTranslatorFactory;
        _binaryTranslatorFactory = binaryTranslatorFactory;
        _unaryTranslatorFactory = unaryTranslatorFactory;
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
                JsUnaryExpressionBuilder.AppendUnary(_builder, _unaryTranslatorFactory, unary, InternalProcessExpression);
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