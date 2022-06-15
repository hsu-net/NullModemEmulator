using Nuke.Common.CI.GitHubActions;

[GitHubActions(
    "continuous",
    GitHubActionsImage.WindowsLatest,
    AutoGenerate = true,
    PublishArtifacts = true,
    // On = new[] { GitHubActionsTrigger.Push },
    OnPushBranches = new[] { "main" },
    InvokedTargets = new[] { nameof(Deploy) },
    ImportSecrets = new[] { nameof(GitAccessToken), nameof(NuGetApiKey), nameof(MyGetApiKey) },
    CacheKeyFiles = new string[0]
)]
internal partial class Build
{
    //private Target Release => _ => _
    //    .Description("Release")
    //    .Executes(() =>
    //    {
    //        GitReleaseManagerCreate(new Nuke.Common.Tools.GitReleaseManager.GitReleaseManagerCreateSettings());
    //        //GitReleaseManagerAddAssets( )
    //    });
}