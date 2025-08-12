# Variables
$OldNamespace = "BookStoreAppBlazor.Server.UI"
$NewNamespace = "BookStoreApp.Blazor.Server.UI"
$ProjectFolder = "BookStoreApp.Blazor.Server.UI"
$CsprojPath = "$ProjectFolder\$NewNamespace.csproj"

# 1. Update .csproj contents
if (Test-Path $CsprojPath) {
    (Get-Content $CsprojPath) -replace $OldNamespace, $NewNamespace | Set-Content $CsprojPath
    Write-Output "✅ Updated RootNamespace in .csproj"
} else {
    Write-Output "❌ .csproj file not found at: $CsprojPath"
}

# 2. Replace namespaces in all .cs files
$csFiles = Get-ChildItem -Path $ProjectFolder -Recurse -Include *.cs
foreach ($file in $csFiles) {
    (Get-Content $file.FullName) -replace $OldNamespace, $NewNamespace | Set-Content $file.FullName
}
Write-Output "✅ Updated namespaces in .cs files"

# 3. Update .sln file (if any)
$slnFile = Get-ChildItem -Path "." -Filter *.sln | Select-Object -First 1
if ($slnFile) {
    (Get-Content $slnFile.FullName) -replace $OldNamespace, $NewNamespace | Set-Content $slnFile.FullName
    Write-Output "✅ Updated .sln file: $($slnFile.Name)"
} else {
    Write-Output "⚠️ No .sln file found. Skipping solution update."
}

Write-Output "`n🎉 Namespace and solution update complete!"
