# Raidtech Home Manager

Prototype foundation for centralized Jellyfin home-screen preference policies.

**Status:** scaffold only, not installable. Jellyfin 12.4 APIs and each native client must be verified before implementing enforcement.

## Goals
- Administrator-managed home-screen policies.
- Apply supported preferences to existing and new users.
- Reconcile supported settings when they drift.
- Keep Web, Android TV, Android, iOS and other client capabilities separate.
- Report unsupported settings rather than claiming universal control.

## Important limitation
A server plugin cannot force a native client to render UI sections it does not implement, or necessarily override client-local settings. Each client must be tested independently.

See [architecture](docs/ARCHITECTURE.md) and [compatibility](docs/COMPATIBILITY.md).

Do not install this scaffold on a production server.