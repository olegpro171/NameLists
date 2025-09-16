using System;
using System.Globalization;

namespace Helpers;

public static class DoubleUtils
{
    public static int CountDigits(double value)
    {
        return value.ToString(CultureInfo.InvariantCulture).Replace(".", "").Replace("-", "").Length;
    }
}