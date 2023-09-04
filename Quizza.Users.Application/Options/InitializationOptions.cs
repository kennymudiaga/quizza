
using Quizza.Common.Web.Configuration;

namespace Quizza.Users.Application.Options;

[ConfigSectionName(ConfigSection)]
public class InitializationOptions : OptionsBase
{
    public const string ConfigSection = "Initialization";

    /// <summary>
    /// Gets or sets a flag to control app user initialization.
    /// If <see langword="true"/>, initialization logic will be run.
    /// </summary>
    public virtual bool Enabled { get; init; }

    /// <summary>
    /// List of predefined admin-user emails: users on this list will be configured as 'admin' when they signup.
    /// </summary>
    public virtual IReadOnlyList<string>? AdminUsers { get; init; }
}
