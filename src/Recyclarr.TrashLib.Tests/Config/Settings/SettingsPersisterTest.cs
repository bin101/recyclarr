using Recyclarr.TestLibrary;

namespace Recyclarr.TrashLib.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class SettingsPersisterTest
{
    [Test, AutoMockData]
    public void Load_should_create_settings_file_if_not_exists(
        [Frozen] MockFileSystem fileSystem,
        [Frozen] IAppPaths paths,
        SettingsProvider sut)
    {
        _ = sut.Settings;

        fileSystem.AllFiles.Should().ContainSingle(paths.SettingsPath.FullName);
    }

    [Test, AutoMockData]
    public void Load_defaults_when_file_does_not_exist(
        [Frozen(Matching.ImplementedInterfaces)] YamlSerializerFactory serializerFactory,
        [Frozen] IAppPaths paths,
        SettingsProvider sut)
    {
        var expectedSettings = new SettingsValues();

        var settings = sut.Settings;

        settings.Should().BeEquivalentTo(expectedSettings);
    }
}
