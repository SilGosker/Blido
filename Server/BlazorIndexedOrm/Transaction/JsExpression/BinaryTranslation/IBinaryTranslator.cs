namespace BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation;

public interface IBinaryTranslator
{
    public static abstract TranslateBinary TranslateBinary { get; }
    public static abstract TryMatchBinary TryMatchBinary { get; }
    public static abstract TranslateBinaryHash[] SupportedHashes { get; }
}