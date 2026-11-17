# Raidtech Home Manager

Centralized home-screen preference management for Jellyfin 12.4.

> **Status: early development.** The repository now contains the initial Jellyfin plugin entry point and a persistent policy configuration model. Preference enforcement and the administrator UI are not implemented yet.

## Goals

- Define administrator-managed home-screen policies.
- Apply verified preferences to existing and newly created users.
- Reconcile supported settings when they drift.
- Keep Web, Android TV, Android, iOS and other client capabilities separate.
- Report unsupported settings rather than claiming universal control.

## Compatibility boundary

A server plugin can manage server-side user configuration and supported API behavior. It cannot force a native client to render UI sections that the client does not implement, nor can it necessarily override client-local settings. Each client must be tested independently.

## Development

- Jellyfin packages: `12.4.0`
- Target framework: `.NET 10`
- Build: `dotnet build Raidtech.HomeManager.sln --configuration Release`

The package references are pinned to Jellyfin 12.4.0. Runtime assets are excluded as recommended by the official plugin template.

## Safety

This is not yet a production-ready release. Do not install it on a production Jellyfin server. Back up the server configuration and test with a non-admin account first.

## License

GPL-3.0. See `LICENSE`.
