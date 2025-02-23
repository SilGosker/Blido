using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class LessThanBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append(")<(");
        processNext(expression.Right);
        builder.Append(')');
    };

    public static TryMatchBinary TryMatchBinary => (BinaryExpression expression, out TranslateBinaryHash hash) =>
    {
        hash = default;
        if (expression.NodeType == ExpressionType.LessThan)
        {
            hash = new TranslateBinaryHash((int)CoreBinaryTranslators.LessThanBinaryTranslator);
            return true;
        }

        return false;
    };

    public static TranslateBinaryHash[] SupportedHashes => new[]
    {
        new TranslateBinaryHash((int)CoreBinaryTranslators.LessThanBinaryTranslator)
    };
}