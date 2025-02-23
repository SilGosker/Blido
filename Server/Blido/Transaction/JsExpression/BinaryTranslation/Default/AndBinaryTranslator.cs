using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class AndBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append(")&(");
        processNext(expression.Right);
        builder.Append(')');
    };

    public static TryMatchBinary TryMatchBinary => (BinaryExpression expression, out TranslateBinaryHash hash) =>
    {
        hash = default;
        if (expression.NodeType is ExpressionType.And)
        {
            hash = new TranslateBinaryHash((int)CoreBinaryTranslators.AndBinaryTranslator);
            return true;
        }

        return false;
    };

    public static TranslateBinaryHash[] SupportedHashes => new[]
    {
        new TranslateBinaryHash((int)CoreBinaryTranslators.AndBinaryTranslator),
    };
}