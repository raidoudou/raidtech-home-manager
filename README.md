# Raidtech Home Manager

Administrator-managed home-section policies for Jellyfin 12.4.

> **Status: early development / test only.** The plugin now includes a configuration page and a scheduled server-side synchronization task. It has not yet been validated against every client. Do not enable enforcement until the exact client identifier and display-preferences behavior have been verified on a test account.

## Current features

- A shared policy for Jellyfin Web and Jellyfin Android TV, plus an independent Emby policy.
- Editable exact Jellyfin display-preferences client identifier.
- Configurable display-preferences ID (default: `usersettings`).
- Global enforcement safety switch, disabled by default.
- Scheduled synchronization task, default interval 15 minutes.
- Targets explicitly selected Jellyfin users per client policy, with a separate opt-in switch for all users. Empty user selections affect nobody by default.
- Logs updated, unchanged, skipped and failed user/client pairs.

## Important compatibility boundary

Jellyfin stores display preferences per user and client identifier. The plugin uses Jellyfin's `IDisplayPreferencesManager` and the server's `DisplayPreferences.HomeSections` model. This is a server-side preference mechanism; it does not force a client to render a section that its UI does not implement, and it does not prove that every native app uses the same preference ID or client identifier.

The Jellyfin Web identifier `emby` was observed in a live Jellyfin Web request. The current Jellyfin Android TV source also uses display-preferences ID `usersettings` and client identifier `emby` for its home sections. These clients therefore share one server-side policy and cannot be configured independently by this plugin. Android TV runtime behavior still needs testing on the installed app version. The separate Emby app identifier is not yet verified and must be confirmed before enabling its policy.

## Configuration

1. Build and install the plugin in a non-production Jellyfin 12.4 test server.
2. Open **Dashboard → Plugins → Raidtech Home Manager**.
3. Keep **Enable enforcement globally** disabled initially.
4. Configure the shared Jellyfin Web + Android TV policy (`usersettings` / `emby`). Configure Emby independently only after verifying its identifiers.
5. Enable one client policy, select the intended users (or explicitly choose all users), and configure its ordered section names.
6. Run **Dashboard → Scheduled Tasks → Raidtech Home Manager: Synchronize home sections** manually.
7. Verify the result with a test account and the intended client before enabling periodic enforcement.

Allowed section names:

- `None`
- `SmallLibraryTiles`
- `LibraryButtons`
- `ActiveRecordings`
- `Resume`
- `ResumeAudio`
- `LatestMedia`
- `NextUp`
- `LiveTv`
- `ResumeBook`

## Build

- Target framework: .NET 10
- Jellyfin package references: 12.4.0

```bash
dotnet restore Raidtech.HomeManager.sln
dotnet build Raidtech.HomeManager.sln --configuration Release
```

## Safety

The sync task changes only `HomeSections` for the selected display-preferences ID, client identifier and explicitly targeted users. A policy with no selected users and `Apply to all users` disabled changes nobody. It does not change user permissions, libraries, passwords, or unrelated display-preference fields. The global enforcement switch defaults to off.

Back up Jellyfin before testing. This project is not a production release yet.

## License

GPL-3.0. See `LICENSE`.
