namespace Bootler;

public static class Is
{
    public static bool IsNullEmptyOrWhiteSpace(this string str) =>
        string.IsNullOrWhiteSpace(str.Trim()) || string.IsNullOrEmpty(str.Trim());

    public static bool AnyNullEmptyOrWhitepace(params string[] sources) =>
        sources.Any(source => source.IsNullEmptyOrWhiteSpace());
}