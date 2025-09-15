using System;

namespace ReadTrack.API.Extensions;

public static class StringExtensions
{
    private static string Pad(this string text)
    {
        var padding = 3 - ((text.Length + 3) % 4);
        if (padding == 0)
        {
            return text;
        }
        return text + new string('=', padding);
    }
    
    public static byte[] Decode(this string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return Convert.FromBase64String(text.Replace('-', '+').Replace('_', '/').Pad());
    }
}