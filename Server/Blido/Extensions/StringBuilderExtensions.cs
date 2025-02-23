using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Extensions;

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

    internal static StringBuilder AppendEscaped(this StringBuilder sb, object? obj)
    {
        switch (obj)
        {
            case Index index:
                sb.Append(index.Value);
                break;
            case int i:
                sb.Append(i);
                break;
            case double b:
                sb.Append(b);
                break;
            case float f:
                sb.Append(f);
                break;
            case uint ui:
                sb.Append(ui);
                break;
            case long l:
                sb.Append(l);
                break;
            case ulong ul:
                sb.Append(ul);
                break;
            case short s:
                sb.Append(s);
                break;
            case ushort us:
                sb.Append(us);
                break;
            case byte b:
                sb.Append(b);
                break;
            case sbyte @sbyte:
                sb.Append(@sbyte);
                break;
            case char c:
                sb.Append('\'');
                if (c == '\'')
                {
                    sb.Append("\\\'");
                }
                else
                {
                    sb.Append(c);
                }

                sb.Append('\'');
                break;
            case string s:
                sb.Append('"');
                sb.AppendEscaped(s);
                sb.Append('"');
                break;
            case CultureInfo ci:
                sb.Append('\"');
                sb.Append(ci.TwoLetterISOLanguageName);
                sb.Append('\"');
                break;
            case bool b:
                sb.Append(b ? "true" : "false");
                break;
            case null:
                sb.Append("null");
                break;
            case IEnumerable enumerable:
                sb.Append('[');
                var first = true;
                foreach (var item in enumerable)
                {
                    if (!first)
                    {
                        sb.Append(',');
                    }
                    sb.AppendEscaped(item);
                    first = false;
                }
                sb.Append(']');
                break;
            default:
                throw new NotImplementedException();
        }

        return sb;
    }
}