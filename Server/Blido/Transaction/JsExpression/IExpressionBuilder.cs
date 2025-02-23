using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression;

public interface IExpressionBuilder
{
    public string ProcessExpression(LambdaExpression expression);
}