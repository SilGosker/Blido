using System.Reflection;
using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class LastIndexOfMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
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

        sb.Append(".lastIndexOf(");
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
        typeof(string).GetMethod(nameof(string.LastIndexOf), new Type[] { typeof(char) }),
        typeof(string).GetMethod(nameof(string.LastIndexOf), new Type[] { typeof(string) }),
        typeof(string).GetMethod(nameof(string.LastIndexOf), new Type[] { typeof(char), typeof(int) }),
        typeof(string).GetMethod(nameof(string.LastIndexOf), new Type[] { typeof(string), typeof(int) }),
        typeof(string).GetMethod(nameof(string.LastIndexOf), new Type[] { typeof(string), typeof(StringComparison) }),
        typeof(string).GetMethod(nameof(string.LastIndexOf),
            new Type[] { typeof(string), typeof(int), typeof(StringComparison) }),
    };
#nullable restore
}