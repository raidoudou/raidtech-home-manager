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
    /// Enforcement defaults to disabled until client behavior is verified.
    /// </summary>
    public List<ClientHomePolicy> Policies { get; set; }
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
    /// Gets or sets whether enforcement is enabled for this client family.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the ordered section identifiers configured by the administrator.
    /// Values are not applied until verified for the target client.
    /// </summary>
    public List<string> Sections { get; set; } = [];
}
