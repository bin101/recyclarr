using System.IO.Abstractions;
using Recyclarr.Cli.TestLibrary;
using Recyclarr.Common;
using Recyclarr.Common.TestLibrary;

namespace Recyclarr.TrashLib.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class QualityGuideServiceTest : IntegrationFixture
{
    [TestCase(SupportedServices.Sonarr, "sonarr")]
    [TestCase(SupportedServices.Radarr, "radarr")]
    public void Get_data_for_service(SupportedServices service, string serviceDir)
    {
        Fs.AddFileFromEmbeddedResource(
            Paths.RepoDirectory.SubDir("docs", "json", serviceDir, "quality-size").File("metadata.json"),
            GetType(),
            "Data.quality_size.json");

        var sut = Resolve<QualityGuideService>();

        var result = sut.GetQualitySizeData(service);

        result.Should().ContainSingle(x => x.Type == "series");
    }
}
