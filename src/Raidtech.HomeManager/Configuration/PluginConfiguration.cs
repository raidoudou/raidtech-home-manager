using MediaBrowser.Model.Plugins;

namespace Raidtech.HomeManager.Configuration;

/// <summary>
/// Persistent administrator configuration for Raidtech Home Manager.
/// </summary>
public sealed class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginConfiguration"/> class.
    /// </summary>
    public PluginConfiguration()
    {
        Policies = [];
    }

    /// <summary>
    /// Gets or sets the client-specific home-screen policies.
    /// </summary>
    public List<ClientHomePolicy> Policies { get; set; }

    /// <summary>
    /// Gets or sets whether the scheduled task is allowed to apply policies.
    /// This global safety switch defaults to false.
    /// </summary>
    public bool EnforcementEnabled { get; set; }

    /// <summary>
    /// Gets or sets the display-preferences identifier used by the client.
    /// Jellyfin Web commonly uses usersettings; verify each native client before enabling it.
    /// </summary>
    public string DisplayPreferencesId { get; set; } = "usersettings";
}

/// <summary>
/// A policy for one Jellyfin client family.
/// </summary>
public sealed class ClientHomePolicy
{
    /// <summary>
    /// Gets or sets the client family identifier (web, androidtv, android, ios, other).
    /// </summary>
    public string ClientFamily { get; set; } = "web";

    /// <summary>
    /// Gets or sets the exact Jellyfin display-preferences client string.
    /// This must match the client value sent to the display-preferences API.
    /// </summary>
    public string ClientIdentifier { get; set; } = "Jellyfin Web";

    /// <summary>
    /// Gets or sets whether enforcement is enabled for this client family.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the ordered section identifiers.
    /// Supported values are Jellyfin HomeSectionType names.
    /// </summary>
    public List<string> Sections { get; set; } = [];
}
