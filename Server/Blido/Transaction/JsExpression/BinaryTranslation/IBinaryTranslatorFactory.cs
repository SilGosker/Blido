using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation;

public interface IBinaryTranslatorFactory : IReadOnlyDictionary<BinaryExpression, TranslateBinary>
{
    public void AddCustomBinaryTranslator(BinaryExpression tryMatchBinary, TranslateBinary translator);
    public void AddCustomBinaryTranslator<TTranslator>() where TTranslator : IBinaryTranslator;
    internal void Confirm();
}