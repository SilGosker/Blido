using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation;

public delegate void TranslateUnary(StringBuilder builder, UnaryExpression expression, ProcessExpression processNext);