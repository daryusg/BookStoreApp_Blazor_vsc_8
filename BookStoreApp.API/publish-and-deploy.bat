@echo on
cls

REM Configuration
set PUBLISH_DIR=publish
set ZIP_FILE=publish.zip
set RESOURCE_GROUP=bookstoreappkev-rg
set APP_NAME=bookstoreappkevapi
set RUNTIME=win-x86

echo Cleaning previous output...
rmdir /s /q "%PUBLISH_DIR%" 2>>error.log
del "%ZIP_FILE%" 2>>error.log

echo Publishing as self-contained for %RUNTIME%...
dotnet publish BookStoreApp.API.csproj -c Release -r %RUNTIME% --self-contained true -p:PublishSingleFile=false -o "%PUBLISH_DIR%" >> deploy.log 2>>error.log
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Publish failed. See deploy.log and error.log.
    pause
    exit /b %ERRORLEVEL%
)

echo Zipping published output...
powershell -Command "Compress-Archive -Path '%PUBLISH_DIR%\*' -DestinationPath '%ZIP_FILE%' -Force" >> deploy.log 2>>error.log
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Zipping failed. See deploy.log and error.log.
    pause
    exit /b %ERRORLEVEL%
)

echo Deploying to Azure Web App: %APP_NAME%...
az webapp deploy --resource-group %RESOURCE_GROUP% --name %APP_NAME% --src-path "%ZIP_FILE%" --type zip >> deploy.log 2>>error.log
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Deployment failed. See deploy.log and error.log.
    pause
    exit /b %ERRORLEVEL%
)

echo ✅ Deployment Successful!
pause
