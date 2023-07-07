using System.IO.Abstractions;
using AutoMapper;
using Recyclarr.Cli.Pipelines.ReleaseProfile.Api.Objects;
using Recyclarr.Cli.TestLibrary;
using Recyclarr.TrashLib.Config.Parsing;
using Recyclarr.TrashLib.Config.Services;
using Recyclarr.TrashLib.Settings;

namespace Recyclarr.Cli.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class ServiceCompatibilityIntegrationTest : CliIntegrationFixture
{
    [Test]
    public void Load_settings_yml_correctly_when_file_exists()
    {
        var sut = Resolve<SettingsProvider>();
        // For this test, it doesn't really matter if the YAML data matches what SettingsValue expects.
        // This test only ensures that the data deserialized is from the actual correct file.
        const string yamlData = @"
repositories:
  trash_guides:
    clone_url: http://the_url.com
";

        Fs.AddFile(Paths.AppDataDirectory.File("settings.yml"), new MockFileData(yamlData));

        var settings = sut.Settings;

        settings.Repositories.TrashGuides.CloneUrl.Should().Be("http://the_url.com");
    }

    [Test]
    public void Test_automapper_di()
    {
        var mapper = Resolve<IMapper>();

        mapper.Map<CustomFormatConfig>(new CustomFormatConfigYaml());
        // SonarrReleaseProfileV1, SonarrReleaseProfile
        mapper.Map<SonarrReleaseProfile>(new SonarrReleaseProfileV1());
    }
}
