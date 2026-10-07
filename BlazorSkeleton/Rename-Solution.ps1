<#
.SYNOPSIS
    Renames the skeleton (projects, namespaces, files, folders) to your application's name.

.EXAMPLE
    .\Rename-Solution.ps1 -NewName PUComp2 -DisplayName "Price Comparison 2"

.NOTES
    Run from the solution root, before the first build (or delete bin/obj first).
    NewName must be a valid C# namespace segment (letters, digits, underscores, dots).
#>
param(
    [Parameter(Mandatory = $true)]
    [ValidatePattern('^[A-Za-z_][A-Za-z0-9_]*(\.[A-Za-z_][A-Za-z0-9_]*)*$')]
    [string] $NewName,

    # Shown in the page title/header (Constants.ApplicationName)
    [string] $DisplayName,

    [string] $OldName = 'BlazorSkeleton',
    [string] $OldDisplayName = 'Blazor Skeleton'
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$skip = '\\(bin|obj|\.git|\.vs)(\\|$)'
$textExtensions = @('.cs', '.razor', '.cshtml', '.csproj', '.sln', '.slnx', '.json', '.props', '.targets', '.yml', '.yaml', '.md', '.css', '.config')
$utf8NoBom = New-Object System.Text.UTF8Encoding $false

# 1. Replace text inside files (this script is excluded because .ps1 isn't in the list)
@(Get-ChildItem -Path $root -Recurse -File) |
    Where-Object { $_.FullName -notmatch $skip -and $textExtensions -contains $_.Extension.ToLowerInvariant() } |
    ForEach-Object {
        $text = [System.IO.File]::ReadAllText($_.FullName)
        $updated = $text.Replace($OldName, $NewName)
        if ($DisplayName) { $updated = $updated.Replace($OldDisplayName, $DisplayName) }
        if ($updated -ne $text) {
            [System.IO.File]::WriteAllText($_.FullName, $updated, $utf8NoBom)
            Write-Host "Updated  $($_.FullName.Substring($root.Length + 1))"
        }
    }

# 2. Rename files, then folders deepest-first so parent paths stay valid
@(Get-ChildItem -Path $root -Recurse -File) |
    Where-Object { $_.FullName -notmatch $skip -and $_.Name -like "*$OldName*" } |
    ForEach-Object { Rename-Item -LiteralPath $_.FullName -NewName $_.Name.Replace($OldName, $NewName) }

Get-ChildItem -Path $root -Recurse -Directory |
    Where-Object { $_.FullName -notmatch $skip -and $_.Name -like "*$OldName*" } |
    Sort-Object { $_.FullName.Length } -Descending |
    ForEach-Object { Rename-Item -LiteralPath $_.FullName -NewName $_.Name.Replace($OldName, $NewName) }

Write-Host ""
Write-Host "Done. Renamed '$OldName' to '$NewName'. You can also rename the root folder itself."
