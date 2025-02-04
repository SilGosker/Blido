using System.Reflection;
using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class TrimMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, _) =>
    {
        builder.Append(".trim()");
        if (expression.Arguments.Count > 0)
        {
            ThrowHelper.ThrowUnsupportedException(expression.Method);
        }
    };

#nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.Trim), Array.Empty<Type>())
    };
#nullable restore
}