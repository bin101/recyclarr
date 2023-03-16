using System.IO.Abstractions;
using Recyclarr.Cli.TestLibrary;
using Recyclarr.TrashLib;

namespace Recyclarr.Cli.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class BaseCommandSetupIntegrationTest : IntegrationFixture
{
    [Test]
    public void Base_command_startup_tasks_are_registered()
    {
        var registrations = Resolve<IEnumerable<IBaseCommandSetupTask>>();
        registrations.Select(x => x.GetType()).Should().BeEquivalentTo(new[]
        {
            typeof(JanitorCleanupTask),
            typeof(AppPathSetupTask)
        });
    }

    [Test]
    public void Log_janitor_cleans_up_user_specified_max_files()
    {
        const int maxFiles = 25;

        Fs.AddFile(Paths.SettingsPath.FullName, new MockFileData($@"
log_janitor:
  max_files: {maxFiles}
"));

        for (var i = 0; i < maxFiles + 20; ++i)
        {
            Fs.AddFile(Paths.LogDirectory.File($"logfile-{i}.log").FullName, new MockFileData(""));
        }

        var sut = Resolve<JanitorCleanupTask>();
        sut.OnFinish();

        Fs.AllFiles.Where(x => x.StartsWith(Paths.LogDirectory.FullName))
            .Should().HaveCount(maxFiles);
    }

    [Test]
    public void Log_janitor_cleans_up_default_max_files()
    {
        var settingsProvider = Resolve<ISettingsProvider>();
        var maxFiles = settingsProvider.Settings.LogJanitor.MaxFiles;

        for (var i = 0; i < maxFiles + 20; ++i)
        {
            Fs.AddFile(Paths.LogDirectory.File($"logfile-{i}.log").FullName, new MockFileData(""));
        }

        var sut = Resolve<JanitorCleanupTask>();
        sut.OnFinish();

        maxFiles.Should().BeGreaterThan(0);
        Fs.AllFiles.Where(x => x.StartsWith(Paths.LogDirectory.FullName))
            .Should().HaveCount(maxFiles);
    }

    [Test]
    public void App_paths_setup_creates_initial_directories()
    {
        for (var i = 0; i < 50; ++i)
        {
            Fs.AddFile(Paths.LogDirectory.File($"logfile-{i}.log").FullName, new MockFileData(""));
        }

        var sut = Resolve<AppPathSetupTask>();
        sut.OnStart();

        var expectedDirs = new[]
        {
            Paths.LogDirectory.FullName,
            Paths.RepoDirectory.FullName,
            Paths.CacheDirectory.FullName
        };

        expectedDirs.Should().IntersectWith(Fs.AllDirectories);
    }
}
