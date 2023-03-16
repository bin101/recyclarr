namespace Recyclarr.TrashLib;

public class CustomFormatApiPersistencePhase
{
    private readonly ICustomFormatService _api;

    public CustomFormatApiPersistencePhase(ICustomFormatService api)
    {
        _api = api;
    }

    public async Task Execute(IServiceConfiguration config, CustomFormatTransactionData transactions)
    {
        foreach (var cf in transactions.NewCustomFormats)
        {
            var response = await _api.CreateCustomFormat(config, cf);
            if (response is not null)
            {
                cf.Id = response.Id;
            }
        }

        foreach (var dto in transactions.UpdatedCustomFormats)
        {
            await _api.UpdateCustomFormat(config, dto);
        }

        if (config.DeleteOldCustomFormats)
        {
            foreach (var map in transactions.DeletedCustomFormats)
            {
                await _api.DeleteCustomFormat(config, map.CustomFormatId);
            }
        }
    }
}
