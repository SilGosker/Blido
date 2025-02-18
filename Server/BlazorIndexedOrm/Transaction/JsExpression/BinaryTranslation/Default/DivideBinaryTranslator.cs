using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class DivideBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append(")/(");
        processNext(expression.Right);
        builder.Append(')');
    };

    public static TryMatchBinary TryMatchBinary => (BinaryExpression binaryExpression, out TranslateBinaryHash hash) =>
    {
        hash = default;
        if (binaryExpression.NodeType == ExpressionType.Divide)
        {
            hash = new TranslateBinaryHash((int)CoreBinaryTranslators.DivideBinaryTranslator);
            return true;
        }
        return false;
    };

    public static TranslateBinaryHash[] SupportedHashes => new[]
    {
        new TranslateBinaryHash((int)CoreBinaryTranslators.DivideBinaryTranslator)
    };
}