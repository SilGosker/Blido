using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class LastMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Arguments[0]);
        if (expression.Arguments.Count == 2)
        {
            builder.Append(".findLast(");
            processNext(expression.Arguments[1]);
            builder.Append(')');
        }
        else
        {
            builder.Append(".at(-1)");
        }

        builder.Append("??throw new Error(\"Sequence contains no matching elements\"))");
    };

    public static MethodInfo[] SupportedMethods =>
        typeof(Enumerable).GetMethods().Where(x => x.Name == nameof(Enumerable.Last)).ToArray();
}