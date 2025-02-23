using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class IsNullOrEmptyMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, methodCall, processNext) =>
    {
        builder.Append("(!!");
        processNext(methodCall.Arguments[0]);
        builder.Append(')');
    };

#nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.IsNullOrEmpty), new[] { typeof(string) })
    };
#nullable restore
}