using System.Reflection;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class StartsWithMethodCallTranslator : IMethodCallTranslator
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

        builder.Append(".startsWith(");
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
        typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(char) }),
        typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) }),
        typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string), typeof(StringComparison) }),
    };
#nullable enable
}