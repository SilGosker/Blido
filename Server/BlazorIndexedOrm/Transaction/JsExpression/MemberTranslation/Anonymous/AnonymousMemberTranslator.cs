using System.Linq.Expressions;
using System.Reflection;
using BlazorIndexedOrm.Core.Extensions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation.Anonymous;

public class AnonymousMemberTranslator : IMemberTranslator
{
    public static TranslateMember TranslateMember => (builder, expression, _) =>
    {
        if ((expression.Member is not PropertyInfo
            && expression.Member is not FieldInfo)

            || expression.Expression is not ConstantExpression { Value: { } obj })
        {
            return;
        }

        object? value = null;
        if (expression.Member is PropertyInfo propertyInfo)
        {
            value = propertyInfo.GetValue(obj);
        }
        else if (expression.Member is FieldInfo fieldInfo)
        {
            value = fieldInfo.GetValue(obj);
        }

        builder.AppendEscaped(value);
    };

    public static MemberInfo[] SupportedMembers => new[]
    {
        new { x = 1 }.GetType().GetMember("x")[0]
    };
}