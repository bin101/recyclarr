using System.IO.Abstractions;
using Recyclarr.Cli.TestLibrary;
using Recyclarr.TestLibrary;
using Spectre.Console.Testing;

namespace Recyclarr.TrashLib.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class ConfigTemplateListerTest : IntegrationFixture
{
    [Test, AutoMockData]
    public async Task Hidden_templates_are_not_rendered(
        IFileInfo stubFile,
        [Frozen(Matching.ImplementedInterfaces)] TestConsole console,
        [Frozen] IConfigTemplateGuideService guideService,
        ConfigTemplateLister sut)
    {
        guideService.TemplateData.Returns(new[]
        {
            new TemplatePath(SupportedServices.Radarr, "r1", stubFile, false),
            new TemplatePath(SupportedServices.Radarr, "r2", stubFile, false),
            new TemplatePath(SupportedServices.Sonarr, "s1", stubFile, false),
            new TemplatePath(SupportedServices.Sonarr, "s2", stubFile, true)
        });

        await sut.List();

        console.Output.Should().NotContain("s2");
    }
}
