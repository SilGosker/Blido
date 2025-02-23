using System.Reflection;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class TrimMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processExpression) =>
    {
        processExpression(expression.Object!);
        builder.Append(".trim()");
    };

#nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.Trim), Array.Empty<Type>())
    };
#nullable restore
}