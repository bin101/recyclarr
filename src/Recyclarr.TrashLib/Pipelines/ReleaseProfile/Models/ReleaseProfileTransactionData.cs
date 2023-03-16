namespace Recyclarr.TrashLib;

public record ReleaseProfileTransactionData(
    IReadOnlyCollection<SonarrReleaseProfile> UpdatedProfiles,
    IReadOnlyCollection<SonarrReleaseProfile> CreatedProfiles,
    IReadOnlyCollection<SonarrReleaseProfile> DeletedProfiles
);
