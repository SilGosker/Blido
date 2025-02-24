using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class ConcatMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        builder.Append(".concat(");
        processNext(expression.Arguments[1]);
        builder.Append(')');
    };

    public static MethodInfo[] SupportedMethods => new []
    {
        typeof(System.Linq.Enumerable).GetMethod(nameof(System.Linq.Enumerable.Concat))!
    };
}