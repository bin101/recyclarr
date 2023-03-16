using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using JetBrains.Annotations;
using Recyclarr.TrashLib;
using Spectre.Console.Cli;

namespace Recyclarr.Cli;

[UsedImplicitly]
[Description("OBSOLETE: Use `sync radarr` instead")]
internal class RadarrCommand : AsyncCommand<RadarrCommand.CliSettings>
{
    private readonly ILogger _log;
    private readonly CustomFormatDataLister _cfLister;
    private readonly QualitySizeDataLister _qualityLister;
    private readonly IMigrationExecutor _migration;
    private readonly IRepoUpdater _repoUpdater;
    private readonly ISyncProcessor _syncProcessor;

    [UsedImplicitly]
    [SuppressMessage("Design", "CA1034:Nested types should not be visible")]
    public class CliSettings : ServiceCommandSettings, ISyncSettings
    {
        public SupportedServices? Service => SupportedServices.Radarr;
        public IReadOnlyCollection<string> Instances { get; } = Array.Empty<string>();

        [CommandOption("-p|--preview")]
        [Description("Only display the processed markdown results without making any API calls.")]
        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public bool Preview { get; init; }

        [CommandOption("-c|--config")]
        [Description(
            "One or more YAML config files to use. All configs will be used and settings are additive. " +
            "If not specified, the script will look for `recyclarr.yml` in the same directory as the executable.")]
        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        [TypeConverter(typeof(FileInfoConverter))]
        public IFileInfo[] ConfigsOption { get; init; } = Array.Empty<IFileInfo>();
        public IReadOnlyCollection<IFileInfo> Configs => ConfigsOption;

        [CommandOption("--list-custom-formats")]
        [Description("List available custom formats from the guide in YAML format.")]
        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public bool ListCustomFormats { get; init; }

        [CommandOption("--list-qualities")]
        [Description("List available quality definition types from the guide.")]
        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public bool ListQualities { get; init; }
    }

    public RadarrCommand(
        ILogger log,
        CustomFormatDataLister cfLister,
        QualitySizeDataLister qualityLister,
        IMigrationExecutor migration,
        IRepoUpdater repoUpdater,
        ISyncProcessor syncProcessor)
    {
        _log = log;
        _cfLister = cfLister;
        _qualityLister = qualityLister;
        _migration = migration;
        _repoUpdater = repoUpdater;
        _syncProcessor = syncProcessor;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CliSettings settings)
    {
        // Will throw if migration is required, otherwise just a warning is issued.
        _migration.CheckNeededMigrations();
        await _repoUpdater.UpdateRepo();

        if (settings.ListCustomFormats)
        {
            _log.Warning("The `radarr` subcommand is DEPRECATED -- Use `list custom-formats radarr` instead!");
            _cfLister.ListCustomFormats(SupportedServices.Radarr);
            return 0;
        }

        if (settings.ListQualities)
        {
            _log.Warning("The `radarr` subcommand is DEPRECATED -- Use `list qualities radarr` instead!");
            _qualityLister.ListQualities(SupportedServices.Radarr);
            return 0;
        }

        _log.Warning("The `radarr` subcommand is DEPRECATED -- Use `sync` instead!");
        return (int) await _syncProcessor.ProcessConfigs(settings);
    }
}
