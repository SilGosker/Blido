using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class IsNullOrWhiteSpaceMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, methodCall, processNext) =>
    {
        builder.Append("(!!");
        processNext(methodCall.Arguments[0]);
        builder.Append(".trim()");
        builder.Append(')');
    };

#nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.IsNullOrWhiteSpace), new[] { typeof(string) })
    };
#nullable enable
}