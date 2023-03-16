namespace Recyclarr.TrashLib;

public record ConflictingCustomFormat(
    CustomFormatData GuideCf,
    int ConflictingId
);
