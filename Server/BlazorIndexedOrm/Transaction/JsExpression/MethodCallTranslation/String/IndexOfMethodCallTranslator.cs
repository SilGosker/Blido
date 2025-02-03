using System.Reflection;
using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class IndexOfMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        // IndexOf(string, int, int, StringComparison)
        if (expression.Arguments.Count == 4)
        {
            ThrowHelper.ThrowUnsupportedException(expression.Method);
            return;
        }

        // IndexOf(string, int, int)
        // IndexOf(char, int, int)
        if (expression.Arguments.Count == 3 && expression.Arguments[2].Type == typeof(int))
        {
            ThrowHelper.ThrowUnsupportedException(expression.Method);
            return;
        }

        processNext(expression.Object!);

        bool ignoreCase = false;
        if (expression.Arguments.Count > 1)
        {
            if (CaseInsensitivityHelper.IsCaseInsensitive(expression.Arguments[1]))
            {
                ignoreCase = true;
            }
            else if (expression.Arguments.Count > 2
                     && CaseInsensitivityHelper.IsCaseInsensitive(expression.Arguments[2]))
            {
                ignoreCase = true;
            }
        }

        if (ignoreCase)
        {
            sb.Append(".toUpperCase()");
        }

        sb.Append(".indexOf(");

        processNext(expression.Arguments[0]);
        if (ignoreCase)
        {
            sb.Append(".toUpperCase()");
        }

        if (expression.Arguments.Count > 1 && expression.Arguments[1].Type == typeof(int))
        {
            sb.Append(',');
            processNext(expression.Arguments[1]);
        }

        sb.Append(')');
    };

    #nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(string), typeof(int), typeof(StringComparison) }),
        typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(string), typeof(StringComparison) }),
        typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(char), typeof(StringComparison) }),
        typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(char), typeof(int) }),
        typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(string) }),
        typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(char) }),
        typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(string), typeof(int) }),
    };
    #nullable restore
}
