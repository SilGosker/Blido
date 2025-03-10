﻿using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class LessThanOrEqualBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append(")<=(");
        processNext(expression.Right);
        builder.Append(')');
    };

    public static BinaryExpression[] SupportedBinaries => new[]
    {
        Expression.LessThanOrEqual(Expression.Constant(1), Expression.Constant(2))
    };
}