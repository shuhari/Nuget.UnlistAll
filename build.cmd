@echo off
REM build scripts
REM Prerequirement:
REM   nuget setApiKey <APIKEY>
REM Usage: see help label

set NUGET_PATH=packages\NuGet.CommandLine.4.9.6\tools\nuget.exe
set NUGET_SERVER=https://www.nuget.org/api/v2/package

cls
if "%1" == "build" goto build
if "%1" == "pack" goto pack
if "%1" == "push" goto push
goto help

:build
msbuild Nuget.UnlistAll.sln /p:Configuration=Release;Platform="Any CPU" /t:Rebuild
goto end

:pack
del *.nupkg
%NUGET_PATH% pack Nuget.UnlistAll\Nuget.UnlistAll.nuspec
goto end

:push
%NUGET_PATH% push Nuget.UnlistAll.*.nupkg -source %NUGET_SERVER%
goto end

:help
echo Build script for Nuget.UnlistAll project.
echo =========================================
echo build build - Rebuild project
echo build pack - Create nuget package
echo build push - Push nuget package to server
echo build help - Show this help
goto end

:end

