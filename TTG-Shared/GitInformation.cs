using System.Reflection;

namespace TTG_Shared;

public static class GitInformation {

    private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    private static readonly Type? GitVersionInformationType = Assembly.GetType("GitVersionInformation");

    public static string Version => GitVersionInformationType?.GetField("SemVer")?.GetValue(null) as string ?? string.Empty;
    public static string Sha => GitVersionInformationType?.GetField("Sha")?.GetValue(null) as string ?? string.Empty;
    public static string ShortSha => GitVersionInformationType?.GetField("ShortSha")?.GetValue(null) as string ?? string.Empty;

}
