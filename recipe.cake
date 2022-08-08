#load nuget:?package=Cake.Recipe&version=3.0.0

//*************************************************************************************************
// Settings
//*************************************************************************************************

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
    shouldRunCodecov: false,
    shouldRunDotNetCorePack: true);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
    context: Context,
    testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Common]* -[Cake.Testing]* -[*.Tests]* -[Cake.Issues]* -[Cake.Issues.Testing]* -[Cake.Issues.PullRequests]* -[Cake.AzureDevOps]* -[Shouldly]* -[DiffEngine]* -[EmptyFiles]*",
    testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
    testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

//*************************************************************************************************
// Execution
//*************************************************************************************************

Build.RunDotNetCore();
