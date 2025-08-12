# Variables
$OldNamespace = "BookStoreAppBlazor.Server.UI"
$NewNamespace = "BookStoreApp.Blazor.Server.UI"
$ProjectFolder = "BookStoreApp.Blazor.Server.UI"
$CsprojPath = "$ProjectFolder\$NewNamespace.csproj"

# 1. Update .csproj file
if (Test-Path $CsprojPath) {
    (Get-Content $CsprojPath) -replace $OldNamespace, $NewNamespace | Set-Content $CsprojPath
    Write-Output "✅ Updated RootNamespace in .csproj"
} else {
    Write-Output "❌ .csproj file not found at: $CsprojPath"
}

# 2. File extensions to update
$fileExtensions = @("*.cs", "*.razor", "*.razor.cs")

# 3. Replace namespaces in all relevant files
foreach ($ext in $fileExtensions) {
    $files = Get-ChildItem -Path $ProjectFolder -Recurse -Include $ext
    foreach ($file in $files) {
        (Get-Content $file.FullName) -replace $OldNamespace, $NewNamespace | Set-Content $file.FullName
    }
    Write-Output "✅ Updated $ext files"
}

# 4. Update .sln file
$slnFile = Get-ChildItem -Path "." -Filter *.sln | Select-Object -First 1
if ($slnFile) {
    (Get-Content $slnFile.FullName) -replace $OldNamespace, $NewNamespace | Set-Content $slnFile.FullName
    Write-Output "✅ Updated .sln file: $($slnFile.Name)"
} else {
    Write-Output "⚠️ No .sln file found. Skipping solution update."
}

Write-Output "`n🎉 Namespace replacement complete across .cs, .razor, .razor.cs, .csproj, and .sln files!"
