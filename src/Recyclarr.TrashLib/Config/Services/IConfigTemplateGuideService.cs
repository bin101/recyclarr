namespace Recyclarr.TrashLib;

public interface IConfigTemplateGuideService
{
    IReadOnlyCollection<TemplatePath> TemplateData { get; }
}
