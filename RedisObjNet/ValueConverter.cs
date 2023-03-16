using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RedisObjNet;

internal static class ValueConverter
{
    internal static string TRUE = "1";
    internal static string FALSE = "0";

    private static NumberFormatInfo nfi = BuildNfi();
    private static NumberFormatInfo BuildNfi()
    {
        var culture = new CultureInfo("en");
        var nfi = (NumberFormatInfo)culture.NumberFormat.Clone();
        nfi.NumberDecimalSeparator = ".";
        return nfi;
    }

    internal static int? IntFromRedisString(string raw)
    {
        if (!int.TryParse(raw, out int result))
        {
            return default;
        }

        return result;
    }

    internal static bool? BoolFromRedisString(string raw)
    {
        return raw == TRUE ? true : raw == FALSE ? false : null;
    }

    internal static double? DoubleFromRedisString(string raw)
    {
        if (!double.TryParse(raw, nfi, out double result))
        {
            return default;
        }

        return result;
    }

    internal static string? ToRedisString(object? value)
    {
        if (value == null)
        {
            return string.Empty;
        }
        else if(value is string stringValue)
        {
            return stringValue;
        }
        else if(value is int intValue)
        {
            return intValue.ToString();
        }
        else if (value is bool boolValue)
        {
            return boolValue ? TRUE : FALSE;
        }
        else if (value is TimeSpan timeSpanValue)
        {
            return timeSpanValue.Ticks.ToString();
        }
        else if (value is float floatValue)
        {
            return floatValue.ToString(nfi);
        }
        else if (value is double doubleValue)
        {
            return doubleValue.ToString(nfi);
        }
        else if (value is List<string> listValue)
        {
            return string.Join(",", listValue.Select(v => v.ToString()));
        }

        //TODO
        throw new Exception();
    }
}
