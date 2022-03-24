#load nuget:?package=Cake.Recipe&version=2.2.1

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    title: "Cake.Issues.PullRequests.AzureDevOps",
    repositoryOwner: "cake-contrib",
    repositoryName: "Cake.Issues.PullRequests.AzureDevOps",
    appVeyorAccountName: "cakecontrib",
    shouldGenerateDocumentation: false,
    shouldCalculateVersion: true,
    shouldRunDupFinder: false, // dupFinder is missing in 2021.3.0-eap
    shouldRunCodecov: false,
    shouldRunDotNetCorePack: true);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
    context: Context,
    dupFinderExcludePattern: new string[] { BuildParameters.RootDirectoryPath + "/src/Cake.Issues.PullRequests.AzureDevOps.Tests/Capabilities/*.cs" },
    testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Common]* -[Cake.Testing]* -[*.Tests]* -[Cake.Issues]* -[Cake.Issues.Testing]* -[Cake.Issues.PullRequests]* -[Cake.AzureDevOps]* -[Shouldly]* -[DiffEngine]* -[EmptyFiles]*",
    testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
    testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

// Workaround until https://github.com/cake-contrib/Cake.Recipe/issues/862 has been fixed in Cake.Recipe
ToolSettings.SetToolPreprocessorDirectives(
    reSharperTools: "#tool nuget:?package=JetBrains.ReSharper.CommandLineTools&version=2021.3.1",
    gitVersionGlobalTool: "#tool dotnet:?package=GitVersion.Tool&version=5.8.1");

// Disable Upload-Coveralls-Report task since it fails to install the tool on AppVeyor
BuildParameters.Tasks.UploadCoverallsReportTask.WithCriteria(() => false);

Build.RunDotNetCore();
