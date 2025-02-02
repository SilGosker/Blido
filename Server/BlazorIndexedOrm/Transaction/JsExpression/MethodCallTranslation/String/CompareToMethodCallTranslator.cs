using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class CompareToMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        if (expression.Object is not null)
        {
            processNext(expression.Object);
        }

        builder.Append(".localCompare(");
        processNext(expression.Arguments[0]);
        builder.Append(')');
    };

    #nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.CompareTo), new[] { typeof(string) }),
        typeof(string).GetMethod(nameof(string.CompareTo), new[] { typeof(object) })
    };
    #nullable restore
}