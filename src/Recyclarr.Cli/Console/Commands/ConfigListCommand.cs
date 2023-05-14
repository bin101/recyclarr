using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Recyclarr.Cli.Console.Helpers;
using Recyclarr.Cli.Processors.Config;
using Recyclarr.TrashLib.Config.Listers;
using Recyclarr.TrashLib.ExceptionTypes;
using Spectre.Console.Cli;

namespace Recyclarr.Cli.Console.Commands;

[UsedImplicitly]
[Description("List configuration files in various ways.")]
public class ConfigListCommand : AsyncCommand<ConfigListCommand.CliSettings>
{
    private readonly ILogger _log;
    private readonly ConfigListProcessor _processor;

    [SuppressMessage("Design", "CA1034:Nested types should not be visible")]
    public class CliSettings : ServiceCommandSettings
    {
        [CommandArgument(0, "[ListCategory]")]
        [EnumDescription<ConfigListCategory>(
            "The type of configuration information to list. If not specified, defaults to 'local'.")]
        public ConfigListCategory ListCategory { get; [UsedImplicitly] init; } = ConfigListCategory.Local;
    }

    public ConfigListCommand(ILogger log, ConfigListProcessor processor)
    {
        _log = log;
        _processor = processor;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CliSettings settings)
    {
        try
        {
            await _processor.Process(settings.ListCategory);
        }
        catch (FileExistsException e)
        {
            _log.Error(
                "The file {ConfigFile} already exists. Please choose another path or " +
                "delete/move the existing file and run this command again", e.AttemptedPath);

            return 1;
        }

        return 0;
    }
}
