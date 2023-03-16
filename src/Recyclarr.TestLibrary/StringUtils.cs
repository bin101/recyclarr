using Recyclarr.Common;

namespace Recyclarr.TestLibrary;

public static class StringUtils
{
    public static string TrimmedString(string value)
    {
        return value.TrimNewlines();
    }
}
