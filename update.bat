@echo off

echo Updating developer project...
IF EXIST "MicroServicesStarter\bin\debug\MicroServicesStarter.exe" (
MicroServicesStarter\bin\debug\MicroServicesStarter.exe --update
) ELSE IF EXIST "MicroServicesStarter\bin\release\MicroServicesStarter.exe" (
MicroServicesStarter\bin\release\MicroServicesStarter.exe --update
) ELSE IF EXIST "MicroServicesStarter\bin\deploy\MicroServicesStarter.exe" (
MicroServicesStarter\bin\deploy\MicroServicesStarter.exe --update
) ELSE IF EXIST "MicroServicesStarter\bin\integration test\MicroServicesStarter.exe" (
"MicroServicesStarter\bin\integration test\MicroServicesStarter.exe" --update
)