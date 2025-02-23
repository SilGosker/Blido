﻿using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class TakeMethodCallTranslation : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        builder.Append(".slice(0,");
        processNext(expression.Arguments[1]);
        builder.Append(')');
    };

    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(System.Linq.Enumerable).GetMethods().First(x => x.GetParameters().Length == 2 && x.GetParameters()[1].ParameterType == typeof(int)),
    };
}