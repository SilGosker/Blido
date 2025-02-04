using System.Reflection;
using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class EndsWithMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processExpression) =>
    {
        processExpression(expression.Object!);

        bool ignoreCase = false;
        if (expression.Arguments.Count == 2)
        {
            if (CaseInsensitivityHelper.IsCaseInsensitive(expression.Arguments[1]))
            {
                builder.Append(".toUpperCase()");
                ignoreCase = true;
            }
        }
        else if (expression.Arguments.Count == 3)
        {
            ThrowHelper.ThrowUnsupportedException(expression.Method);
        }

        builder.Append(".endsWith(");
        processExpression(expression.Arguments[0]);
        if (ignoreCase)
        {
            builder.Append(".toUpperCase()");
        }

        builder.Append(')');
    };

#nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(char) }),
        typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) }),
        typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string), typeof(StringComparison) }),
    };
#nullable restore
}