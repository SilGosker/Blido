using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class IndexAtMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        if (expression.Object is not null)
        {
            processNext(expression.Object);
        }

        builder.Append(".charAt(");
        processNext(expression.Arguments[0]);
        builder.Append(')');
    };

    #nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod("get_Chars", new []{ typeof(int)}),
    };
    #nullable restore
}