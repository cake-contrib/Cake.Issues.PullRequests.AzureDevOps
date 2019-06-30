#load nuget:?package=Cake.Recipe&version=1.0.0

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context, 
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    title: "Cake.Issues.PullRequests.Tfs",
    repositoryOwner: "cake-contrib",
    repositoryName: "Cake.Issues.PullRequests.Tfs",
    appVeyorAccountName: "cakecontrib",
    shouldRunCodecov: true);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
    context: Context,
    dupFinderExcludePattern: new string[] { BuildParameters.RootDirectoryPath + "/src/Cake.Issues.PullRequests.Tfs.Tests/Capabilities/*.cs" },
    testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* -[Cake.Issues]* -[Cake.Issues.Testing]* -[Cake.Issues.PullRequests]* -[Cake.Tfs]* -[Shouldly]* -[*]Costura.AssemblyLoader -[*]ProcessedByFody",
    testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
    testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

BuildParameters.Tasks.UploadCoverageReportTask = Task("Buildserver")
  .IsDependentOn("Default")
  .IsDependentOn("Upload-Coverage-Report");

Build.Run();
