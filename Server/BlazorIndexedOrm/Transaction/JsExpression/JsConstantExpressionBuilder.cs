using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using BlazorIndexedOrm.Core.Extensions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsConstantExpressionBuilder
{
    public static void AppendConstant(StringBuilder sb, ConstantExpression expression)
    {
        sb.AppendEscaped(expression.Value);
    }
}