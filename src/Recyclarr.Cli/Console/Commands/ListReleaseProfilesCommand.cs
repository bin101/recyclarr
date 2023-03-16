using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Recyclarr.TrashLib;
using Spectre.Console.Cli;

#pragma warning disable CS8765

namespace Recyclarr.Cli;

[UsedImplicitly]
[Description("List Sonarr release profiles in the guide for a particular service.")]
internal class ListReleaseProfilesCommand : AsyncCommand<ListReleaseProfilesCommand.CliSettings>
{
    private readonly ILogger _log;
    private readonly ReleaseProfileDataLister _lister;
    private readonly IRepoUpdater _repoUpdater;

    [UsedImplicitly]
    [SuppressMessage("Design", "CA1034:Nested types should not be visible")]
    public class CliSettings : BaseCommandSettings
    {
        [CommandOption("--terms")]
        [Description(
            "For the given Release Profile Trash ID, list terms in it that can be filtered in YAML format. " +
            "Note that not every release profile has terms that may be filtered.")]
        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public string? ListTerms { get; init; }
    }

    public ListReleaseProfilesCommand(
        ILogger log,
        ReleaseProfileDataLister lister,
        IRepoUpdater repoUpdater)
    {
        _log = log;
        _lister = lister;
        _repoUpdater = repoUpdater;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CliSettings settings)
    {
        try
        {
            await _repoUpdater.UpdateRepo();

            if (settings.ListTerms is not null)
            {
                // Ignore nullability of ListTerms since the Settings.Validate() method will check for null/empty.
                _lister.ListTerms(settings.ListTerms!);
            }
            else
            {
                _lister.ListReleaseProfiles();
            }
        }
        catch (ArgumentException e)
        {
            _log.Error(e, "Error");
            return 1;
        }

        return 0;
    }
}
