using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class MultiplyBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append(")*(");
        processNext(expression.Right);
        builder.Append(')');
    };

    public static TryMatchBinary TryMatchBinary => (BinaryExpression binaryExpression, out TranslateBinaryHash hash) =>
    {
        hash = default;
        if (binaryExpression.NodeType is ExpressionType.Multiply or ExpressionType.MultiplyChecked)
        {
            hash = new TranslateBinaryHash((int)CoreBinaryTranslators.MultiplyBinaryTranslator);
            return true;
        }
        return false;
    };

    public static TranslateBinaryHash[] SupportedHashes => new[]
    {
        new TranslateBinaryHash((int)CoreBinaryTranslators.MultiplyBinaryTranslator)
    };
}