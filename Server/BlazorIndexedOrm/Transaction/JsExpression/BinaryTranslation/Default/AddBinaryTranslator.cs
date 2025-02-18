using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class AddBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append(")+(");
        processNext(expression.Right);
        builder.Append(')');
    };

    public static TryMatchBinary TryMatchBinary => (BinaryExpression binaryExpression, out TranslateBinaryHash hash) =>
    {
        hash = default;
        if (binaryExpression.NodeType is ExpressionType.Add
            or ExpressionType.AddChecked)
        {
            hash = new TranslateBinaryHash((int)CoreBinaryTranslators.AddBinaryTranslator);
            return true;
        }

        return false;
    };

    public static TranslateBinaryHash[] SupportedHashes => new[]
    {
        new TranslateBinaryHash((int)CoreBinaryTranslators.AddBinaryTranslator)
    };
}