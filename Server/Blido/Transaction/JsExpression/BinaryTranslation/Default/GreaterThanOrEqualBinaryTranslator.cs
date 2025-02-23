using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class GreaterThanOrEqualBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append(")>=(");
        processNext(expression.Right);
        builder.Append(')');
    };

    public static TryMatchBinary TryMatchBinary => (BinaryExpression binaryExpression, out TranslateBinaryHash hash) =>
    {
        hash = default;
        if (binaryExpression.NodeType == ExpressionType.GreaterThanOrEqual)
        {
            hash = new TranslateBinaryHash((int)CoreBinaryTranslators.GreaterThanOrEqualBinaryTranslator);
            return true;
        }
        return false;
    };

    public static TranslateBinaryHash[] SupportedHashes => new[]
    {
        new TranslateBinaryHash((int)CoreBinaryTranslators.GreaterThanOrEqualBinaryTranslator)
    };
}