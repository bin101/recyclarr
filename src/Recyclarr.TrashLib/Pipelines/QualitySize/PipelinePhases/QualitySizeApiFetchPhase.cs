namespace Recyclarr.TrashLib;

public class QualitySizeApiFetchPhase
{
    private readonly IQualityDefinitionService _api;

    public QualitySizeApiFetchPhase(IQualityDefinitionService api)
    {
        _api = api;
    }

    public async Task<IList<ServiceQualityDefinitionItem>> Execute(IServiceConfiguration config)
    {
        return await _api.GetQualityDefinition(config);
    }
}
