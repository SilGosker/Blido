using System.Globalization;
using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class ToLowerMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        processNext(expression.Object!);
        if (expression.Arguments.Count == 0)
        {
            builder.Append(".toLowerCase()");
            return;
        }

        builder.Append(".toLocalLowerCase(");
        processNext(expression.Arguments[0]);
        builder.Append(')');
    };

#nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.ToLower), Array.Empty<Type>()),
        typeof(string).GetMethod(nameof(string.ToLower), new Type[] { typeof(CultureInfo) })
    };

#nullable restore
}