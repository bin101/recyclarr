namespace Recyclarr.TrashLib;

public class CustomFormatApiFetchPhase
{
    private readonly ICustomFormatService _api;

    public CustomFormatApiFetchPhase(ICustomFormatService api)
    {
        _api = api;
    }

    public async Task<IReadOnlyCollection<CustomFormatData>> Execute(IServiceConfiguration config)
    {
        var result = await _api.GetCustomFormats(config);
        return result.AsReadOnly();
    }
}
