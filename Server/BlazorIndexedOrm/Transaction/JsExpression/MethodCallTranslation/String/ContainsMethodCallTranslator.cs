using System.Reflection;
using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class ContainsMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        bool ignoreCase = false;
        if (expression.Object is not null)
        {
            processNext(expression.Object);
        }

        if (expression.Arguments.Count == 2)
        {
            if (CaseInsensitivityHelper.IsCaseInsensitive(expression.Arguments[1]))
            {
                builder.Append(".toUpperCase()");
                ignoreCase = true;
            }
        }

        builder.Append(".includes(");
        processNext(expression.Arguments[0]);
        if (ignoreCase)
        {
            builder.Append(".toUpperCase()");
        }

        builder.Append(')');
    };

    #nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char) }),
        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) }),
        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char), typeof(StringComparison) }),
        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string), typeof(StringComparison) }),
    };
    #nullable restore
}