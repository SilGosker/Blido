using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Extensions;

internal static class StringBuilderExtensions
{
    internal static StringBuilder AppendEscaped(this StringBuilder sb, string value)
    {
        ReadOnlySpan<char> s = value.AsSpan();
        ArgumentNullException.ThrowIfNull(sb);
        if (s.IsEmpty)
        {
            return sb;
        }

        foreach (char c in s)
        {
            switch (c)
            {
                case '\'':
                    sb.Append("\\'");
                    break;
                case '\\':
                    sb.Append(@"\\");
                    break;
                case '"':
                    sb.Append("\\\"");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }

        return sb;
    }
}