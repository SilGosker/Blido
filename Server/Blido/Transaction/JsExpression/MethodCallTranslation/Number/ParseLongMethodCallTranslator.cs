using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Number;

public class ParseLongMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        builder.Append("BigInt(");
        processNext(expression.Arguments[0]);
        builder.Append(')');
    };
    public static MethodInfo[] SupportedMethods => new MethodInfo[]
    {
        typeof(long).GetMethod(nameof(long.Parse), new Type[] { typeof(string) })!,
        typeof(ulong).GetMethod(nameof(ulong.Parse), new Type[] { typeof(string) })!,
    };
}