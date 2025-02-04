using System.Globalization;
using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class ToUpperMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        processNext(expression.Object!);
        if (expression.Arguments.Count == 0)
        {
            builder.Append(".toUpperCase()");
            return;
        }

        builder.Append(".toLocaleUpperCase(");
        processNext(expression.Arguments[0]);
        builder.Append(')');
    };

#nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.ToUpper), Array.Empty<Type>()),
        typeof(string).GetMethod(nameof(string.ToUpper), new Type[] { typeof(CultureInfo) })
    };
#nullable restore
}