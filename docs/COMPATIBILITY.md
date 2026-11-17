# Client compatibility matrix

| Client | Display preferences ID | Client identifier | Policy model | Status |
|---|---|---|---|---|
| Jellyfin Web | `usersettings` | `emby` | Shared profile | Confirmed from live server request |
| Jellyfin Android TV | `usersettings` | `emby` | Shared with Jellyfin Web; not independently targetable | Confirmed in current Android TV source; runtime test still required |
| Emby | Configurable | Configurable | Independent profile | Identifier must be verified on the app |
| Jellyfin Android | To verify | To verify | Not available yet | Not implemented |
| Jellyfin iOS / iPadOS | To verify | To verify | Not available yet | Not implemented |
| Other clients | Varies | Varies | Not available yet | Not implemented |

## Android TV details

The current Jellyfin Android TV source configures its user home preferences with:

- Display preferences ID: `usersettings`
- API client identifier: `emby`
- Home section values stored in the display-preferences custom preferences.

This is the same preference namespace already observed for Jellyfin Web on the Raidtech Media Server. Therefore, the plugin must expose one shared policy for **Jellyfin Web + Android TV**, not two independent policies. Creating a second Android TV policy with the same user, ID and client identifier would cause the policies to overwrite one another during synchronization.

The Android TV app reads up to ten ordered home-section preferences. Its current implementation does not render the `ResumeBook` section (it is explicitly a no-op), and Live TV sections depend on the user's Live TV access. A server-side preference update cannot make unsupported UI sections appear.

Source references:
- [Jellyfin Android TV: UserSettingPreferences.kt](https://github.com/jellyfin/jellyfin-androidtv/blob/master/app/src/main/java/org/jellyfin/androidtv/preference/UserSettingPreferences.kt)
- [Jellyfin Android TV: HomeSectionType.kt](https://github.com/jellyfin/jellyfin-androidtv/blob/master/app/src/main/java/org/jellyfin/androidtv/constant/HomeSectionType.kt)
- [Jellyfin Android TV: HomeRowsFragment.kt](https://github.com/jellyfin/jellyfin-androidtv/blob/master/app/src/main/java/org/jellyfin/androidtv/ui/home/HomeRowsFragment.kt)

## Validation boundary

Source-code inspection confirms the Android TV preference namespace and supported section enum. It does not replace a runtime test against the exact Android TV app version installed on the user's device. Keep enforcement limited to a test user until that check is complete.
