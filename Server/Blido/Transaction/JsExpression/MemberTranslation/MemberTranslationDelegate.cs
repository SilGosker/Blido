using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MemberTranslation;

public delegate void TranslateMember(StringBuilder stringBuilder, MemberExpression expression, ProcessExpression processNext);