namespace Khdamatk.Server.Helper;

public static class StringExtensions
{
    public static string ClampLength(this string value, int min = 0, int max = int.MaxValue)
    {
        if (string.IsNullOrEmpty(value)) return value;

        // إذا كان النص أصغر من الحد الأدنى، نكمله بفراغات (اختياري)
        if (value.Length < min)
        {
            return value.PadRight(min);
        }

        // إذا كان النص أكبر من الحد الأقصى، نقوم بقصه
        if (value.Length > max)
        {
            return value.Substring(0, max);
        }

        return value;
    }

    public static string LimitLength(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }
}