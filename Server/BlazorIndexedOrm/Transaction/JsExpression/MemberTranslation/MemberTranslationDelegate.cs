using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;

public delegate void TranslateMember(StringBuilder stringBuilder, MemberExpression expression, ProcessExpression processNext);