using System.Linq.Expressions;
using System.Text;
using Blido.Core.Helpers;
using Blido.Core.Transaction.JsExpression.MethodCallTranslation;

namespace Blido.Core.Transaction.JsExpression;

public class JsMethodExpressionBuilder
{
    public static void AppendMethod(StringBuilder builder, IMethodCallTranslatorFactory translatorFactory, MethodCallExpression methodCall, ProcessExpression processExpression)
    {
        if (!translatorFactory.TryGetValue(methodCall.Method, out var translateMethod))
        {
            ThrowHelper.ThrowUnsupportedException(methodCall.Method);
            return;
        }

        translateMethod(builder, methodCall, processExpression);
    }
}