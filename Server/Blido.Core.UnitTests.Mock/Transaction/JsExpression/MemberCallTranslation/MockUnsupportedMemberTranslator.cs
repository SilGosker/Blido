﻿using System.Reflection;
using Blido.Core.Transaction.JsExpression.MemberTranslation;

namespace Blido.Core.Transaction.JsExpression.MemberCallTranslation;

public class MockUnsupportedMemberTranslator : IMemberTranslator
{
    public int UnsupportedMember { get; } = 0;

    public static TranslateMember TranslateMember => (builder, expression, processNext) =>
    {
        builder.Append("UnsupportedMember");
    };

    public static MemberInfo[] SupportedMembers => typeof(MockUnsupportedMemberTranslator).GetMember(nameof(UnsupportedMember));
}