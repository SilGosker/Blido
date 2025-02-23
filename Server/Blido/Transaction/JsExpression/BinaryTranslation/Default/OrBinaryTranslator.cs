using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class OrBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append(")|(");
        processNext(expression.Right);
        builder.Append(')');
    };

    public static TryMatchBinary TryMatchBinary => (BinaryExpression expression, out TranslateBinaryHash hash) =>
    {
        if (expression.NodeType == ExpressionType.Or)
        {
            hash = new TranslateBinaryHash((int)CoreBinaryTranslators.OrBinaryTranslator);
            return true;
        }
        hash = default;
        return false;
    };

    public static TranslateBinaryHash[] SupportedHashes => new[]
    {
        new TranslateBinaryHash((int)CoreBinaryTranslators.OrBinaryTranslator)
    };
}