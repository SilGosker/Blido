using System.Linq.Expressions;
using System.Text;
using Blido.Core.Helpers;
using Blido.Core.Transaction.JsExpression.BinaryTranslation;

namespace Blido.Core.Transaction.JsExpression;

public class JsBinaryExpressionBuilder
{
    public static void AppendBinary(StringBuilder sb, IBinaryTranslatorFactory binaryTranslatorFactory, BinaryExpression binary, ProcessExpression processExpression)
    {
        if (binaryTranslatorFactory.TryGetValue(binary, out TranslateBinary? translateBinary))
        {
            translateBinary(sb, binary, processExpression);
            return;
        }

        ThrowHelper.ThrowUnsupportedException(binary);
    }
}