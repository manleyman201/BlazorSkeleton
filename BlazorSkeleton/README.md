# BlazorSkeleton

A bare-bones .NET 10 Blazor Server solution that follows the same architecture as `Blazor.PUComp`, with all of the business logic removed. One end-to-end **Example** feature shows each layer so you can copy the pattern for real features.

## Getting started

1. Install the .NET 10 SDK and Visual Studio 2026 (or VS Code with C# Dev Kit).
2. Rename the skeleton to your app (optional but recommended):
   ```powershell
   .\Rename-Solution.ps1 -NewName MyApp -DisplayName "My App"
   ```
3. Fill in the placeholders in `BlazorSkeleton.Server/appsettings.{Environment}.json` (SQL instance and database). With `"ImpersonateUser": true` the app connects with its Windows identity; otherwise supply `UserName`/`UserPassword` through user secrets, environment variables or Key Vault rather than committing them. Credentials are used as-is (they are not decrypted).
4. Run `BlazorSkeleton.Server`.

> Permissions are the signed-in user's role claims, matched against the `Constants.Permission*` names. Locally (the `Development` environment) the end-user and super-user roles grant nothing, by design in `PermissionService`. You need the Admin or an Engineer role to see the Example tab.

## Project structure

| Project | Purpose |
|---|---|
| `BlazorSkeleton.Server` | Host. `Program.cs` (auth, DI, pipeline), `_Host.cshtml` / `_Layout.cshtml`, `appsettings.*.json`, `serilog.*.json` |
| `BlazorSkeleton.Data` | Non-UI library: `Models`, `Providers` (SQL), `Services`, `Utilities`, `Extensions`, `SessionState`, `Constants` |
| `BlazorSkeleton.UI` | Razor class library: `Pages`, `Shared` (layout, nav, login), `Components` (`Buttons`, `DataGrids`, `Dialogs`, `Tabs`), `wwwroot` |
| `BlazorSkeleton.Test` | xUnit + Moq: `UnitTests` (services), `MockTests` (providers vs. mocked `ISqlDataProvider`), `Integration` |

Solution-level files: `Directory.Build.props` (target framework and shared settings), `Directory.Packages.props` (every package version), `global.json` (SDK pin), `nuget.config` (nuget.org only).

## Request flow

```
Component (Components/Tabs/<Feature>)
   └─ injects I<Feature>Service            (Data/Services)
        └─ calls I<Feature>SqlProvider      (Data/Providers)
             └─ SqlHelper → ISqlDataProvider → stored procedure
```

Access is gated per tab in `Pages/Workspace.razor` through `IPermissionService`, which reads permission strings from `ApplicationState.CurrentUser.Permissions` (the user's role claims, loaded in `MainLayout`).

## Adding a feature

1. `Data/Models/<Feature>Item.cs`: property names matching the procedure's result columns.
2. `Data/Providers/Interfaces/I<Feature>SqlProvider.cs` and `Data/Providers/<Feature>SqlProvider.cs`: one method per stored procedure, through `SqlHelper`.
3. `Data/Services/Interfaces/I<Feature>Service.cs` and `Data/Services/<Feature>Service.cs`.
4. Register both in `Server/Program.cs` under Providers and Services.
5. `CanAccess<Feature>` in `IPermissionService` / `PermissionService`, plus any new permission names in `Constants`.
6. `UI/Components/Tabs/<Feature>/<Feature>Tab.razor` (+ `.razor.cs`), and a `@using` in `UI/_Imports.razor`.
7. A `RadzenTabsItem` in `UI/Pages/Workspace.razor`.
8. Tests: `Test/UnitTests/<Feature>Service_Tests.cs` and `Test/MockTests/<Feature>SqlProvider_Tests.cs`.

## Changes from the original (beyond removing business logic)

- **.NET 10**: `net10.0` everywhere; ASP.NET Core packages on 10.0.x; Microsoft.Identity.Web 4.x.
- **Static assets**: .NET 10 only serves `blazor.server.js` through `MapStaticAssets()`, so it replaces `UseStaticFiles()` (marked `AllowAnonymous()` to keep assets public as before).
- **Central package versions** in `Directory.Packages.props`. Redundant packages removed (`System.Text.Json`, `System.Net.Http`, `System.IO.*`, `Microsoft.Extensions.*`, etc. are in-box on .NET 10 and trigger NuGet pruning warnings). xUnit/coverlet removed from the web project.
- **No internal packages**: the private-feed packages were replaced by small local equivalents in `BlazorSkeleton.Data`: `Providers/Sql` (`ISqlDataProvider`, `SqlDataProvider`, `SQLQuery`, `DBConnections`), `Utilities/EntityFactory`, `Utilities/CsvUtilities`, `Extensions/LoggerExtensions`, `ApplicationState` and `Models/AppUser`. Test-user impersonation in the environment banner was removed (it needed the external user-profile API).
- **Kept on current versions**: Radzen 4.28.9 and the Serilog set.
- **Fixes**: environment banner now hides in PROD (the old `||` condition was always true); build date is the real build time (was app start time); `LoadingDialog`/`Error` namespaces fixed (they still pointed at the old template); `Error.cshtml` CSS paths point to the UI library; `DetailedErrors` is off in PROD; `SqlHelper` duplication collapsed and inner exceptions preserved; the non-Development error message now shows in DEV/QA/PROD.
- **Secrets**: none are in this repo. All connection details are placeholders.

## Pipelines

`nuget-project-build.yml` and `nuget-project-build-scd.yml` are updated to .NET 10. The shared templates they call (`Common.Utility` repo) and the build agents must also support the .NET 10 SDK.

## Resources

- [Material Icons](https://fonts.google.com/icons)
