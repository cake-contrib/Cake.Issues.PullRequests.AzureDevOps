#load nuget:?package=Cake.Recipe&version=3.1.1

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
    shouldRunCoveralls: false, // Disabled because it's currently failing
    shouldPostToGitter: false, // Disabled because it's currently failing
    shouldGenerateDocumentation: false,
    shouldRunDotNetCorePack: true);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
    context: Context,
    testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Common]* -[Cake.Testing]* -[*.Tests]* -[Cake.Issues]* -[Cake.Issues.Testing]* -[Cake.Issues.PullRequests]* -[Cake.AzureDevOps]* -[Shouldly]* -[DiffEngine]* -[EmptyFiles]*",
    testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
    testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

//*************************************************************************************************
// Custom tasks
//*************************************************************************************************

Task("BreakBuildOnIssues")
    .Description("Breaks build if any issues in the code are found.")
    .Does<IssuesData>((data) =>
{
    if (data.Issues.Any())
    {
        throw new Exception("Issues found in code.");
    }
});

IssuesBuildTasks.IssuesTask
    .IsDependentOn("BreakBuildOnIssues");

//*************************************************************************************************
// Execution
//*************************************************************************************************

Build.RunDotNetCore();
