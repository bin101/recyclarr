namespace Recyclarr.TrashLib;

public interface IConfigRegistry
{
    int Count { get; }
    bool DoesConfigExist(string name);
    IEnumerable<IServiceConfiguration> GetConfigsBasedOnSettings(ISyncSettings settings);
    IEnumerable<IServiceConfiguration> GetAllConfigs();
    IEnumerable<IServiceConfiguration> GetConfigsOfType(SupportedServices? serviceType);
}
