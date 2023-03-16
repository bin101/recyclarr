using Recyclarr.TestLibrary;

namespace Recyclarr.TrashLib.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class GitPathTest
{
    [Test, AutoMockData]
    public void Default_path_used_when_setting_is_null(
        [Frozen] ISettingsProvider settings,
        GitPath sut)
    {
        settings.Settings.Returns(new SettingsValues {Repository = new TrashRepository {GitPath = null}});

        var result = sut.Path;

        result.Should().Be(GitPath.Default);
    }

    [Test, AutoMockData]
    public void User_specified_path_used_instead_of_default(
        [Frozen] ISettingsProvider settings,
        GitPath sut)
    {
        var expectedPath = "/usr/local/bin/git";
        settings.Settings.Returns(new SettingsValues {Repository = new TrashRepository {GitPath = expectedPath}});

        var result = sut.Path;

        result.Should().Be(expectedPath);
    }
}
