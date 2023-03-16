namespace Recyclarr.TrashLib;

public class QualityProfileApiFetchPhase
{
    private readonly IQualityProfileService _api;

    public QualityProfileApiFetchPhase(IQualityProfileService api)
    {
        _api = api;
    }

    public async Task<IList<QualityProfileDto>> Execute(IServiceConfiguration config)
    {
        return await _api.GetQualityProfiles(config);
    }
}
