using System;

namespace LearningPlatform.Extensions
{
    public static class Extensions
    {
        public static bool IsUrl(this string source) => Uri.TryCreate(source, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeHttps;
    }
}
