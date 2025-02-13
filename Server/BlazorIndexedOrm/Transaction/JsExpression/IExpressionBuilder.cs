using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public interface IExpressionBuilder
{
    public string ProcessExpression(LambdaExpression expression);
}