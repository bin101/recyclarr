using Recyclarr.Common.Extensions;
using Recyclarr.TrashLib.Config.Services;

namespace Recyclarr.TrashLib.Config.Parsing;

public class ConfigCollection : IConfigCollection
{
    private readonly Dictionary<SupportedServices, List<IServiceConfiguration>> _configs = new();

    public void Add(SupportedServices configType, IServiceConfiguration config)
    {
        _configs.GetOrCreate(configType).Add(config);
    }

    public IReadOnlyCollection<IServiceConfiguration> Get(SupportedServices? serviceType)
    {
        return serviceType is null ? _configs.Values.SelectMany(x => x).ToList() : _configs[serviceType.Value];
    }

    public bool DoesConfigExist(string name)
    {
        return _configs.Values.Any(x => x.Any(y => y.InstanceName.EqualsIgnoreCase(name)));
    }
}
