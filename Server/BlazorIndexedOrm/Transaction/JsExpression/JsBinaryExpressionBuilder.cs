using System.Linq.Expressions;
using System.Text;
using BlazorIndexedOrm.Core.Helpers;
using BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsBinaryExpressionBuilder
{
    public static void AppendBinary(StringBuilder sb, IBinaryTranslatorFactory binaryTranslatorFactory, BinaryExpression binary, ProcessExpression processExpression)
    {
        if (binaryTranslatorFactory.TryGetValue(binary, out TranslateBinary translateBinary))
        {
            translateBinary(sb, binary, processExpression);
            return;
        }

        ThrowHelper.ThrowUnsupportedException(binary);
    }
}