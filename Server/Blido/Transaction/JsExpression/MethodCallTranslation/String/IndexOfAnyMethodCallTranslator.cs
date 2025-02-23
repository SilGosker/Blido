using System.Reflection;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class IndexOfAnyMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        processNext(expression.Object!);

        builder.Append(".split('').findIndex(_x=>");
        processNext(expression.Arguments[0]);
        builder.Append(".contains(_x))");
    };

    #nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.IndexOfAny), new[] { typeof(char[]) })
    };
    #nullable restore
}