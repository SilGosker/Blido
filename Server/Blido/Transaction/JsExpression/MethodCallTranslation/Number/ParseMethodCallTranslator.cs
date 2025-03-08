using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Number;

public class ParseMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        builder.Append("parseInt(");
        processNext(expression.Arguments[0]);
        builder.Append(')');
    };

    public static MethodInfo[] SupportedMethods => new MethodInfo[]
    {
        typeof(sbyte).GetMethod(nameof(sbyte.Parse), new Type[] { typeof(string) })!,
        typeof(byte).GetMethod(nameof(byte.Parse), new Type[] { typeof(string) })!,
        typeof(short).GetMethod(nameof(short.Parse), new Type[] { typeof(string) })!,
        typeof(ushort).GetMethod(nameof(ushort.Parse), new Type[] { typeof(string) })!,
        typeof(int).GetMethod(nameof(int.Parse), new Type[] { typeof(string) })!,
        typeof(uint).GetMethod(nameof(uint.Parse), new Type[] { typeof(string) })!,
    };
}