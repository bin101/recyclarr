namespace Recyclarr.TrashLib;

public enum FormatScoreUpdateReason
{
    Updated,
    Reset
}

public record UpdatedFormatScore(
    string CustomFormatName,
    int OldScore,
    int NewScore,
    FormatScoreUpdateReason Reason);
