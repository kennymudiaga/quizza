namespace Quizza.Common.Web.Configuration;

[ConfigSectionName(ConfigName)]
public class AppInfoOptions : OptionsBase
{
    public const string ConfigName = "AppInfo";
    /// <summary>
    /// The name of the application
    /// </summary>
    public string? Name { get; set; }
}
