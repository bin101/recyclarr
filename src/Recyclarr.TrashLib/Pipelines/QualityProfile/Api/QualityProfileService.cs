using Flurl.Http;

namespace Recyclarr.TrashLib;

internal class QualityProfileService : IQualityProfileService
{
    private readonly IServiceRequestBuilder _service;

    public QualityProfileService(IServiceRequestBuilder service)
    {
        _service = service;
    }

    public async Task<IList<QualityProfileDto>> GetQualityProfiles(IServiceConfiguration config)
    {
        return await _service.Request(config, "qualityprofile")
            .GetJsonAsync<IList<QualityProfileDto>>();
    }

    public async Task<QualityProfileDto> UpdateQualityProfile(IServiceConfiguration config, QualityProfileDto profile)
    {
        return await _service.Request(config, "qualityprofile", profile.Id)
            .PutJsonAsync(profile)
            .ReceiveJson<QualityProfileDto>();
    }
}
