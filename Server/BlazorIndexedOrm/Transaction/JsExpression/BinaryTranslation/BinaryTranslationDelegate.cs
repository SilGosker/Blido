using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation;

public delegate void TranslateBinary(StringBuilder stringBuilder, BinaryExpression binaryExpression, ProcessExpression processExpression);
public delegate bool TryMatchBinary(BinaryExpression binaryExpression, out TranslateBinaryHash hash);