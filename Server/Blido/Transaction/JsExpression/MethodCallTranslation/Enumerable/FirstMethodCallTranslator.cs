﻿using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class FirstMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        sb.Append('(');
        processNext(expression.Arguments[0]);

        if (expression.Arguments.Count == 2)
        {
            sb.Append(".find(");
            processNext(expression.Arguments[1]);
            sb.Append(')');
        }
        else
        {
            sb.Append("[0]");
        }

        sb.Append("??throw new Error(\"Sequence contains no (matching) elements\"))");
    };

    public static MethodInfo[] SupportedMethods => typeof(System.Linq.Enumerable).GetMethods()
        .Where(e => e.Name == nameof(System.Linq.Enumerable.First))
        .ToArray();
}