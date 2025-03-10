using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation;

public interface IBinaryTranslator
{
    public static abstract TranslateBinary TranslateBinary { get; }
    public static abstract BinaryExpression[] SupportedBinaries { get; }
}