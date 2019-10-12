---
Order: 30
Title: Using with Azure Pipelines
Description: Example how to use the Cake.Issues.PullRequests.AzureDevOps addin from an Azure Pipelines build.
---
This example shows how to write issues as comments to an Azure DevOps pull request from an Azure Pipelines build.

To write issues as comments to Azure DevOps pull requests you need to import the core addin,
the core pull request addin, the Azure DevOps support including the Cake.AzureDevOps addin, and one or more issue providers,
in this example for JetBrains InspectCode:

```csharp
#addin "Cake.Issues"
#addin "Cake.Issues.InspectCode"
#addin "Cake.Issues.PullRequests"
#addin "Cake.Issues.PullRequests.AzureDevOps"
#addin "Cake.AzureDevOps"
```

:::{.alert .alert-warning}
Please note that you always should pin addins to a specific version to make sure your builds are deterministic and
won't break due to updates to one of the addins.

See [pinning addin versions](https://cakebuild.net/docs/tutorials/pinning-cake-version#pinning-addin-version) for details.
:::

In the following task we'll first determine if the build is running on Azure DevOps and for a pull request,
then read the remote repository URL and pull request id from environment variables set by the Azure Pipelines build 
and finally call the [AzureDevOpsPullRequests] alias using the OAuth token provided by the Azure Pipeline build.

:::{.alert .alert-info}
Please note that you'll need to setup your Azure Pipelines build to allow scripts to
access the OAuth token and need to setup proper permissions.

See [OAuth authentication from Azure Pipelines] for details.
:::

```csharp
Task("ReportIssuesToPullRequest").Does(() =>
{
    var isRunningOnAzureDevOps =
        !string.IsNullOrWhiteSpace(context.EnvironmentVariable("TF_BUILD")) &&
        !string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_COLLECTIONURI")) &&
        (
            new Uri(context.EnvironmentVariable("SYSTEM_COLLECTIONURI")).Host == "dev.azure.com" ||
            new Uri(context.EnvironmentVariable("SYSTEM_COLLECTIONURI")).Host.EndsWith("visualstudio.com")
        );

    var isPullRequestBuild =
        !string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID"));

    if (isRunningOnAzureDevOps)
        {
            var repositoryUrl = new Uri(context.EnvironmentVariable("BUILD_REPOSITORY_URI"));

            if (isPullRequestBuild)
            {
                var pullRequestIdVariable = context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID");
                if (!Int32.TryParse(pullRequestIdVariable, out var pullRequestId))
                {
                    throw new Exception($"Invalid pull request ID: {pullRequestIdVariable}");
                }
                else
                {
                    var repoRootFolder = MakeAbsolute(Directory("./"));

                    ReportIssuesToPullRequest(
                        InspectCodeIssuesFromFilePath(
                            @"C:\build\inspectcode.log"),
                        AzureDevOpsPullRequests(
                            repositoryUrl,
                            pullRequestId,
                            AzureDevOpsAuthenticationOAuth(EnvironmentVariable("SYSTEM_ACCESSTOKEN"))),
                        repoRootFolder);
                }
            }
        }
});
```

[AzureDevOpsPullRequests]: ../../../../api/Cake.Issues.PullRequests.AzureDevOps/AzureDevOpsPullRequestSystemAliases/64912B0A
[Allow scripts to access the OAuth token]: https://docs.microsoft.com/en-us/azure/devops/pipelines/build/options#allow-scripts-to-access-the-oauth-token
[OAuth authentication from Azure Pipelines]: ../setup#oauth-authentication-from-azure-pipelines
