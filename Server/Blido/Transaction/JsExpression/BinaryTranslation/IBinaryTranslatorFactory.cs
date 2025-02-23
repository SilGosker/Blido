using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation;

public interface IBinaryTranslatorFactory : IReadOnlyDictionary<TranslateBinaryHash, TranslateBinary>
{
    public void AddCustomBinaryTranslator(TryMatchBinary tryMatchBinary, TranslateBinaryHash matchingHash, TranslateBinary translator);
    public void AddCustomBinaryTranslator<TTranslator>() where TTranslator : IBinaryTranslator;
    internal void Confirm();
    public bool TryGetValue(BinaryExpression key, [MaybeNullWhen(false)] out TranslateBinary value);
}