namespace Recyclarr.TrashLib;

public interface IQualityProfileService
{
    Task<IList<QualityProfileDto>> GetQualityProfiles(IServiceConfiguration config);
    Task<QualityProfileDto> UpdateQualityProfile(IServiceConfiguration config, QualityProfileDto profile);
}
