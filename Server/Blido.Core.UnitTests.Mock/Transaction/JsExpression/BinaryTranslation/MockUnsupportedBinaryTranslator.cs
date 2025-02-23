using System.Linq.Expressions;
using Blido.Core.Transaction.JsExpression.BinaryTranslation;

namespace Blido.Core.UnitTests.Mock.Transaction.JsExpression.BinaryTranslation;

public class MockUnsupportedBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext)
        => throw new NotImplementedException();

    public static TryMatchBinary TryMatchBinary => (BinaryExpression binary, out TranslateBinaryHash hash) =>
    {
        hash = default;
        return false;
    };

    public static TranslateBinaryHash[] SupportedHashes => new[]
    {
        new TranslateBinaryHash(int.MaxValue),
        new TranslateBinaryHash(int.MaxValue - 1)
    };
}