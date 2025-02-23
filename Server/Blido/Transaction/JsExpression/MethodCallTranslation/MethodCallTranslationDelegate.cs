using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation;

public delegate void TranslateMethodCall(StringBuilder stringBuilder, MethodCallExpression expression, ProcessExpression processNext);