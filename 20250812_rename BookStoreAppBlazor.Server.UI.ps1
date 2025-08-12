# Set variables
$OldProjectName = "BookStoreAppBlazor.Server.UI"
$NewProjectName = "BookStoreApp.Blazor.Server.UI"

# Rename folder
Rename-Item -Path $OldProjectName -NewName $NewProjectName -Force
Write-Output "✅ Folder renamed: $OldProjectName → $NewProjectName"

# Rename .csproj file
$OldCsprojPath = "$NewProjectName\$OldProjectName.csproj"
$NewCsprojPath = "$NewProjectName\$NewProjectName.csproj"
Rename-Item -Path $OldCsprojPath -NewName "$NewProjectName.csproj"
Write-Output "✅ .csproj renamed: $OldProjectName.csproj → $NewProjectName.csproj"

# Update RootNamespace in .csproj
(Get-Content $NewCsprojPath) -replace $OldProjectName, $NewProjectName | Set-Content $NewCsprojPath
Write-Output "✅ RootNamespace updated in .csproj"

# Replace namespaces in all .cs files
$csFiles = Get-ChildItem -Path $NewProjectName -Recurse -Include *.cs
foreach ($file in $csFiles) {
    (Get-Content $file.FullName) -replace $OldProjectName, $NewProjectName | Set-Content $file.FullName
}
Write-Output "✅ Namespaces updated in all .cs files"

# Optional: Update .sln file
$slnFile = Get-ChildItem -Path "." -Filter *.sln | Select-Object -First 1
if ($slnFile) {
    (Get-Content $slnFile.FullName) -replace $OldProjectName, $NewProjectName | Set-Content $slnFile.FullName
    Write-Output "✅ Solution file updated: $($slnFile.Name)"
} else {
    Write-Output "⚠️ No .sln file found. Skipping solution update."
}

Write-Output "`n🎉 Project rename complete!"
