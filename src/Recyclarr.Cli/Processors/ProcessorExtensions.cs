using Recyclarr.Cli.Console.Settings;
using Recyclarr.Common.Extensions;
using Recyclarr.TrashLib.Config;
using Recyclarr.TrashLib.Config.Services;

namespace Recyclarr.Cli.Processors;

public static class ProcessorExtensions
{
    public static IEnumerable<IServiceConfiguration> GetConfigsBasedOnSettings(
        this IEnumerable<IServiceConfiguration> configs,
        ISyncSettings settings)
    {
        // later, if we filter by "operation type" (e.g. release profiles, CFs, quality sizes) it's just another
        // ".Where()" in the LINQ expression below.
        return configs.GetConfigsOfType(settings.Service)
            .Where(x => settings.Instances.IsEmpty() ||
                settings.Instances!.Any(y => y.EqualsIgnoreCase(x.InstanceName)));
    }

    public static IEnumerable<IServiceConfiguration> Merge(this IEnumerable<IServiceConfiguration> configs)
    {
        return configs
            .ToLookup(x => x.BaseUrl)
            .Select(x => MergeConfigs(x.Key, x.ToList()));
    }

    private static IServiceConfiguration MergeConfigs(Uri baseUrl, IList<IServiceConfiguration> configs)
    {
        var merged = configs[0];
        for (var i = 1; i < configs.Count; ++i)
        {
            merged = merged.Merge(configs[i]);
        }

        return merged;
    }
}
