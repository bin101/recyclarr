namespace Recyclarr.TrashLib;

public interface IQualityGuideService
{
    IReadOnlyList<QualitySizeData> GetQualitySizeData(SupportedServices serviceType);
}
