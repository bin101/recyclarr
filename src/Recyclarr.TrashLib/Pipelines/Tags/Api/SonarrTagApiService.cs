using Flurl.Http;

namespace Recyclarr.TrashLib;

public class SonarrTagApiService : ISonarrTagApiService
{
    private readonly IServiceRequestBuilder _service;

    public SonarrTagApiService(IServiceRequestBuilder service)
    {
        _service = service;
    }

    public async Task<IList<SonarrTag>> GetTags(IServiceConfiguration config)
    {
        return await _service.Request(config, "tag")
            .GetJsonAsync<List<SonarrTag>>();
    }

    public async Task<SonarrTag> CreateTag(IServiceConfiguration config, string tag)
    {
        return await _service.Request(config, "tag")
            .PostJsonAsync(new {label = tag})
            .ReceiveJson<SonarrTag>();
    }
}
