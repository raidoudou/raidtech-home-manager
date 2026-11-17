using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Serialization;
using Raidtech.HomeManager.Configuration;

namespace Raidtech.HomeManager;

/// <summary>
/// Main entry point for Raidtech Home Manager.
/// </summary>
public sealed class Plugin : BasePlugin<PluginConfiguration>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Plugin"/> class.
    /// </summary>
    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    /// <inheritdoc />
    public override string Name => "Raidtech Home Manager";

    /// <inheritdoc />
    public override Guid Id => Guid.Parse("9b0b46ce-3f5c-4c9a-92f2-2a86c62a4f71");

    /// <summary>
    /// Gets the active plugin instance.
    /// </summary>
    public static Plugin? Instance { get; private set; }
}
