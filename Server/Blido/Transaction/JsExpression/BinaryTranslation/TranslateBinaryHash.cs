namespace Blido.Core.Transaction.JsExpression.BinaryTranslation;

public readonly struct TranslateBinaryHash
{
    public TranslateBinaryHash(int hash)
    {
        Hash = hash;
    }
    public int Hash { get; }
    public override int GetHashCode()
    {
        return Hash;
    }
}