﻿using System.Linq.Expressions;
using System.Reflection;
using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class ConcatMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        if (expression.Arguments.Any(e => e.Type == typeof(object) || e.Type == typeof(ReadOnlySpan<char>)))
        {
            ThrowHelper.ThrowUnsupportedException(expression.Method);
        }

        builder.Append('(');
        bool first = true;
        foreach (Expression expressionArgument in expression.Arguments)
        {
            if (!first)
            {
                builder.Append(" + ");
            }

            processNext(expressionArgument);
            first = false;
        }

        builder.Append(')');
    };
    #nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(string), typeof(string), typeof(string), typeof(string) }),
        typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(string), typeof(string), typeof(string) }),
        typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(string), typeof(string) }),
        typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(IEnumerable<string>) }),
        typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(string[]) }),
    };
    #nullable restore
}