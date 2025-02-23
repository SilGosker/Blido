using System.Linq.Expressions;
using System.Text;
using Blido.Core.Helpers;
using Blido.Core.Transaction.JsExpression.UnaryTranslation;

namespace Blido.Core.Transaction.JsExpression;

public class JsUnaryExpressionBuilder
{
    public static void AppendUnary(StringBuilder sb, IUnaryTranslatorFactory factory, UnaryExpression expression,
        ProcessExpression processExpression)
    {
#nullable disable
        if (!factory.TryGetValue(expression, out TranslateUnary translator))
        {
            ThrowHelper.ThrowUnsupportedException(expression);
        }
#nullable restore
        translator!(sb, expression, processExpression);
    }
}