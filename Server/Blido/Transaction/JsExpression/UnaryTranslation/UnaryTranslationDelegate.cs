using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation;

public delegate void TranslateUnary(StringBuilder builder, UnaryExpression expression, ProcessExpression processNext);