@echo off

echo Building solution and service project
for %%i in (*.sln) do (
	"./nuget/nuget.exe" restore "%%i"
	"%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" /T:Build /p:Configuration=Release "%%i"
)

echo Updating developer project...
IF EXIST "MicroServicesStarter\bin\release\MicroServicesStarter.exe" (
	MicroServicesStarter\bin\release\MicroServicesStarter.exe --update
)