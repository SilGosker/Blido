using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation;

public delegate void TranslateBinary(StringBuilder stringBuilder, BinaryExpression binaryExpression, ProcessExpression processExpression);