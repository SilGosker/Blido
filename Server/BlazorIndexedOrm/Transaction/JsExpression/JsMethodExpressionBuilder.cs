﻿using System.Linq.Expressions;
using System.Text;
using BlazorIndexedOrm.Core.Helpers;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsMethodExpressionBuilder
{
    public static void AppendMethod(StringBuilder builder, IJsMethodCallTranslatorFactory translatorFactory, MethodCallExpression methodCall, ProcessExpression processExpression)
    {
        if (!translatorFactory.TryGetValue(methodCall.Method, out var translateMethod))
        {
            ThrowHelper.ThrowUnsupportedException(methodCall.Method);
        }

        translateMethod(builder, methodCall, processExpression);
    }
}