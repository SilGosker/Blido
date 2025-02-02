using System.Reflection;
using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class IndexOfAnyMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        if (expression.Arguments.Count != 1)
        {
            ThrowHelper.ThrowUnsupportedException(expression.Method);
        }

        builder.Append(".split().findIndex(z=>");
        processNext(expression.Arguments[0]);
        builder.Append(".contains(z))");
    };
    #nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.IndexOfAny), new[] { typeof(char[]) })
    };
    #nullable restore
}