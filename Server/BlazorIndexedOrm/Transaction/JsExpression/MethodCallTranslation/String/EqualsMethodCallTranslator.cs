using System.Reflection;
using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class EqualsMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processExpression) =>
    {
        if (expression.Arguments.Count == 2 && CaseInsensitivityHelper.IsCaseInsensitive(expression.Arguments[1]))
        {
            builder.Append('(');
            builder.Append(".localeCompare(");
            processExpression(expression.Arguments[0]);
            builder.Append(",undefined,{sensitivity:'accent'})===0");
            builder.Append(')');
            return;
        }

        builder.Append(" == ");
        processExpression(expression.Arguments[0]);
        builder.Append(')');
    };
    #nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(object) }),
        typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string) }),
        typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string), typeof(string) }),
        typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string), typeof(StringComparison) }),
        typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string), typeof(string), typeof(StringComparison)}),
    };
    #nullable restore
}