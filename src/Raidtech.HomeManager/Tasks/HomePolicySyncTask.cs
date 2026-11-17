using System.Security.Cryptography;
using System.Text;
using Jellyfin.Database.Implementations.Entities;
using Jellyfin.Database.Implementations.Enums;
using MediaBrowser.Common.Extensions;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;
using Raidtech.HomeManager.Configuration;

namespace Raidtech.HomeManager.Tasks;

/// <summary>
/// Reconciles configured home sections for existing Jellyfin users.
/// </summary>
public sealed class HomePolicySyncTask : IScheduledTask
{
    private readonly IUserManager _userManager;
    private readonly IDisplayPreferencesManager _displayPreferencesManager;
    private readonly ILogger<HomePolicySyncTask> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomePolicySyncTask"/> class.
    /// </summary>
    public HomePolicySyncTask(
        IUserManager userManager,
        IDisplayPreferencesManager displayPreferencesManager,
        ILogger<HomePolicySyncTask> logger)
    {
        _userManager = userManager;
        _displayPreferencesManager = displayPreferencesManager;
        _logger = logger;
    }

    /// <inheritdoc />
    public string Name => "Raidtech Home Manager: Synchronize home sections";

    /// <inheritdoc />
    public string Key => "RaidtechHomeManagerSync";

    /// <inheritdoc />
    public string Description => "Applies enabled Raidtech home-section policies to existing users and configured clients.";

    /// <inheritdoc />
    public string Category => "Raidtech Home Manager";

    /// <inheritdoc />
    public Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
    {
        var configuration = Plugin.Instance?.Configuration;
        if (configuration is null || !configuration.EnforcementEnabled)
        {
            _logger.LogInformation("Raidtech Home Manager enforcement is disabled. No user preferences were changed.");
            progress.Report(100);
            return Task.CompletedTask;
        }

        if (!Guid.TryParse(configuration.DisplayPreferencesId, out var preferencesId))
        {
            preferencesId = configuration.DisplayPreferencesId.GetMD5();
        }

        var policies = configuration.Policies
            .Where(policy => policy.Enabled && !string.IsNullOrWhiteSpace(policy.ClientIdentifier))
            .ToList();

        if (policies.Count == 0)
        {
            _logger.LogInformation("No enabled client policies were configured.");
            progress.Report(100);
            return Task.CompletedTask;
        }

        var users = _userManager.GetUsers().ToList();
        var total = Math.Max(1, users.Count * policies.Count);
        var completed = 0;
        var updated = 0;
        var unchanged = 0;
        var skipped = 0;
        var failed = 0;

        foreach (var policy in policies)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (policy.Sections.Count > 10)
            {
                _logger.LogWarning("Policy {ClientFamily} contains more than 10 sections and was skipped.", policy.ClientFamily);
                skipped += users.Count;
                completed += users.Count;
                progress.Report(100d * completed / total);
                continue;
            }

            var desiredSections = new List<HomeSectionType>();
            var invalid = false;
            foreach (var section in policy.Sections)
            {
                if (!Enum.TryParse(section, true, out HomeSectionType parsed))
                {
                    _logger.LogWarning("Unknown home section {Section} in policy {ClientFamily}; policy skipped.", section, policy.ClientFamily);
                    invalid = true;
                    break;
                }

                desiredSections.Add(parsed);
            }

            if (invalid)
            {
                skipped += users.Count;
                completed += users.Count;
                progress.Report(100d * completed / total);
                continue;
            }

            foreach (var user in users)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    var preferences = _displayPreferencesManager.GetDisplayPreferences(
                        user.Id,
                        preferencesId,
                        policy.ClientIdentifier);

                    var current = preferences.HomeSections
                        .OrderBy(section => section.Order)
                        .Select(section => section.Type)
                        .ToList();

                    if (current.SequenceEqual(desiredSections))
                    {
                        unchanged++;
                    }
                    else
                    {
                        preferences.HomeSections.Clear();
                        for (var index = 0; index < desiredSections.Count; index++)
                        {
                            preferences.HomeSections.Add(new HomeSection
                            {
                                Order = index,
                                Type = desiredSections[index]
                            });
                        }

                        _displayPreferencesManager.UpdateDisplayPreferences(preferences);
                        updated++;
                    }
                }
                catch (Exception exception)
                {
                    failed++;
                    _logger.LogError(
                        exception,
                        "Failed to synchronize home sections for user {UserId}, client {ClientIdentifier}.",
                        user.Id,
                        policy.ClientIdentifier);
                }

                completed++;
                progress.Report(100d * completed / total);
            }
        }

        _logger.LogInformation(
            "Raidtech Home Manager sync finished. Updated: {Updated}; unchanged: {Unchanged}; skipped: {Skipped}; failed: {Failed}.",
            updated,
            unchanged,
            skipped,
            failed);

        progress.Report(100);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
    {
        yield return new TaskTriggerInfo
        {
            Type = TaskTriggerInfoType.IntervalTrigger,
            IntervalTicks = TimeSpan.FromMinutes(15).Ticks
        };
    }
}
