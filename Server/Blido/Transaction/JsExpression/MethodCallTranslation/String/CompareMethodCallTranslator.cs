using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class CompareMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        builder.Append(".localeCompare(");
        processNext(expression.Arguments[1]);

        if (expression.Arguments.Count > 2)
        {

            bool ignoreCase = CaseInsensitivityHelper.IsCaseInsensitive(expression.Arguments[2]);

            if (!ignoreCase && expression.Arguments[2] is ConstantExpression { Value: bool ignoreCasingArgument })
            {
                ignoreCase = ignoreCasingArgument;
            }

            if (expression.Arguments.Count == 4)
            {
                builder.Append(',');
                processNext(expression.Arguments[3]);
            }
            else if (ignoreCase)
            {
                builder.Append(",undefined");
            }

            if (ignoreCase)
            {
                builder.Append(",{sensitivity:'accent'}");
            }
        }

        builder.Append(')');
    };

    #nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.Compare), new[] { typeof(string), typeof(string) }),
        typeof(string).GetMethod(nameof(string.Compare), new[] { typeof(string), typeof(string), typeof(bool) }),
        typeof(string).GetMethod(nameof(string.Compare), new[] { typeof(string), typeof(string), typeof(StringComparison) }),
        typeof(string).GetMethod(nameof(string.Compare), new[] { typeof(string), typeof(string), typeof(bool), typeof(CultureInfo) }),
        typeof(string).GetMethod(nameof(string.Compare), new[] { typeof(string), typeof(string), typeof(CultureInfo), typeof(CompareOptions) }),
    };
    #nullable restore
}