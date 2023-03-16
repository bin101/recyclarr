using JetBrains.Annotations;

namespace Recyclarr.TrashLib;

public record TrashRepository
{
    public Uri CloneUrl { get; [UsedImplicitly] init; } = new("https://github.com/TRaSH-/Guides.git");
    public string Branch { get; [UsedImplicitly] init; } = "master";
    public string? Sha1 { get; [UsedImplicitly] init; }
    public string? GitPath { get; [UsedImplicitly] init; }
}

public record LogJanitorSettings
{
    public int MaxFiles { get; [UsedImplicitly] init; } = 20;
}

public record SettingsValues
{
    public TrashRepository Repository { get; [UsedImplicitly] init; } = new();
    public bool EnableSslCertificateValidation { get; [UsedImplicitly] init; } = true;
    public LogJanitorSettings LogJanitor { get; [UsedImplicitly] init; } = new();
}
