@echo off
set NUGET_PATH=packages\NuGet.CommandLine.4.1.0\tools\nuget.exe
cls
if "%1" == "build" goto build
if "%1" == "pack" goto pack
if "%1" == "push" goot push
goto help

:build
msbuild Nuget.UnlistAll.sln /p:Configuration=Release;Platform="Any CPU" /t:Rebuild
goto end

:pack
%NUGET_PATH% pack Nuget.UnlistAll\Nuget.UnlistAll.nuspec
goto end

:push
echo push
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

