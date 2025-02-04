using System.Linq.Expressions;
using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class SplitMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processExpression) =>
    {
        processExpression(expression.Object!);
        sb.Append(".split(");
        processExpression(expression.Arguments[0]);
        if (expression.Arguments.Count >= 1 && expression.Arguments[1].Type == typeof(int))
        {
            sb.Append(',');
            processExpression(expression.Arguments[1]);
        }

        sb.Append(')');

        StringSplitOptions splitOptions = default;

        if (expression.Arguments is { Count: > 1 }
            && expression.Arguments[1] is ConstantExpression { Value: StringSplitOptions constantValue })
        {
            splitOptions = constantValue;
        }
        else if (expression.Arguments is { Count: > 2 }
                 && expression.Arguments[2] is ConstantExpression { Value: StringSplitOptions } constantExpression)
        {
            splitOptions = (StringSplitOptions)constantExpression.Value;
        }

        if (splitOptions.HasFlag(StringSplitOptions.TrimEntries))
        {
            sb.Append(".map(_x => _x.trim())");
        }

        if (splitOptions.HasFlag(StringSplitOptions.RemoveEmptyEntries))
        {
            sb.Append(".filter(_x => !!_x)");
        }
    };

#nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.Split), new[] { typeof(char), typeof(int), typeof(StringSplitOptions) }),
        typeof(string).GetMethod(nameof(string.Split), new[] { typeof(char), typeof(StringSplitOptions) }),
        typeof(string).GetMethod(nameof(string.Split), new[] { typeof(string), typeof(int), typeof(StringSplitOptions) }),
        typeof(string).GetMethod(nameof(string.Split), new[] { typeof(string), typeof(StringSplitOptions) }),
    };
#nullable restore
}