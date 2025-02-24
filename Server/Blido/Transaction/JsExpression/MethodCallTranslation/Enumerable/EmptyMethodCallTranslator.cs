using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class EmptyMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, _, _) =>
    {
        builder.Append("[]");
    };

    public static MethodInfo[] SupportedMethods => new MethodInfo[]
    {
        typeof(System.Linq.Enumerable).GetMethod(nameof(System.Linq.Enumerable.Empty))!
    };
}