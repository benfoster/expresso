// Install .NET Core Global tools.
#tool "dotnet:?package=dotnet-reportgenerator-globaltool&version=5.0.0"
#tool "dotnet:?package=coveralls.net&version=3.0.0"
#tool "dotnet:?package=dotnet-sonarscanner&version=5.4.0"
#tool nuget:?package=docfx.console&version=2.58.9
#tool nuget:?package=KuduSync.NET&version=1.5.3

// Install addins
#addin nuget:?package=Cake.Coverlet&version=2.5.4
#addin nuget:?package=Cake.Sonar&version=1.1.26
#addin nuget:?package=Cake.DocFx&version=1.0.0
#addin nuget:?package=Cake.Git&version=1.1.0
#addin nuget:?package=Cake.Kudu&version=1.3.0

 #r "System.Text.Json"
 #r "System.IO"
 #r "System"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Test");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    //CleanDirectory($"./src/Example/bin/{configuration}");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild("expresso.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetTest("expresso.sln", new DotNetTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);