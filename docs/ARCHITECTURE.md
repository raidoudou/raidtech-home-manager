# Architecture

## Policy model

Policies are client-specific, not one assumed universal set of fields.

Client families:
- Web
- Android TV
- Android
- iOS
- Other / unknown

Each policy should distinguish:
- Desired values
- Keys verified as supported for that client and server version
- Whether enforcement is enabled
- Last-applied audit metadata

## Reconciliation lifecycle

1. Load administrator policy.
2. Detect user creation or relevant preference update.
3. Identify client family when reliable metadata is available.
4. Apply only keys verified for that client.
5. Record applied, skipped, and failed keys.
6. Never overwrite unrelated user settings.

## Native-client limitation

A server plugin cannot guarantee identical UI across independently developed
native clients. Unsupported settings must be reported, not silently treated
as enforced.

## First milestone

Build a compatibility matrix for Jellyfin 12.4 using real API requests and
controlled test accounts before implementing enforcement.
