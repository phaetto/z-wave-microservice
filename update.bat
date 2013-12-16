@echo off

echo Download admin service...
Services.Packages.Tools\Services.Package.Download "{'UpdateServerHostname':'update.msd.am','UpdateServerPort': 12345,'PackageFolder':'.\\\\Admin\\\\','PackageNames':['Services.Executioner']}"

echo Download service wrapper...
Services.Packages.Tools\Services.Package.Download "{'UpdateServerHostname':'update.msd.am','UpdateServerPort': 12345,'PackageFolder':'.\\\\MicroServicesStarter\\\\','PackageNames':['Developer.MicroServicesStarter']}"