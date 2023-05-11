using System.Data.HashFunction.FNV;
using System.IO.Abstractions;
using System.IO.Abstractions.Extensions;
using System.Reflection;
using System.Text;
using Autofac;
using Recyclarr.Common.Extensions;
using Recyclarr.TrashLib;
using Recyclarr.TrashLib.TestLibrary;
using Serilog.Sinks.TestCorrelator;

namespace Recyclarr.Cli.Tests.Integration;

[TestFixture]
public class ConfigCommandTest
{
    [Test]
    public void List_local()
    {
        var hash = FNV1aFactory.Instance.Create(FNVConfig.GetPredefinedConfig(32));
        var methodIdentifier = $"{nameof(ConfigCommandTest)}.{MethodBase.GetCurrentMethod()!.Name}";
        var guid = hash.ComputeHash(Encoding.ASCII.GetBytes(methodIdentifier)).AsHexString();

        // Working directory will be used by both the real file system and the fake one.
        // The real file system will have just the git repo on it.
        // The fake (mock) file system will have the cache, configs, and other things.
        var workingDir = Directory.GetCurrentDirectory();
        var fs = new MockFileSystem(new Dictionary<string, MockFileData>(), workingDir);
        var appDataDir = fs.CurrentDirectory().SubDir("test-data", "config-command", guid);

        var thisAsm = typeof(ConfigCommandTest).Assembly;
        fs.AddFilesFromEmbeddedNamespace(appDataDir.SubDir("configs").FullName, thisAsm,
            $"{thisAsm.GetName().Name}.Data.Configs");

        var app = RecyclarrCli.Initialize(builder =>
        {
            builder.RegisterType<MockLoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.RegisterInstance(fs).As<IFileSystem>();
        });

        using var logContext = TestCorrelator.CreateContext();
        var exitCode = RecyclarrCli.Run(app, "config", "list", "local", "--app-data", appDataDir.FullName);

        exitCode.Should().Be(0);

        var logs = TestCorrelator.GetLogEventsFromContextGuid(logContext.Guid);
    }

    [Test]
    public void List_templates()
    {
        var hash = FNV1aFactory.Instance.Create(FNVConfig.GetPredefinedConfig(32));
        var methodIdentifier = $"{nameof(ConfigCommandTest)}.{MethodBase.GetCurrentMethod()!.Name}";
        var guid = hash.ComputeHash(Encoding.ASCII.GetBytes(methodIdentifier)).AsHexString();

        // Working directory will be used by both the real file system and the fake one.
        // The real file system will have just the git repo on it.
        // The fake (mock) file system will have the cache, configs, and other things.
        var workingDir = Directory.GetCurrentDirectory();
        var fs = new MockFileSystem(new Dictionary<string, MockFileData>(), workingDir);
        var appDataDir = fs.CurrentDirectory().SubDir("test-data", "config-command", guid);

        var thisAsm = typeof(ConfigCommandTest).Assembly;
        fs.AddFilesFromEmbeddedNamespace(appDataDir.SubDir("configs").FullName, thisAsm,
            $"{thisAsm.GetName().Name}.Data.Configs");

        var app = RecyclarrCli.Initialize(builder =>
        {
            builder.RegisterType<MockLoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.RegisterInstance(fs).As<IFileSystem>();
        });

        using var logContext = TestCorrelator.CreateContext();
        var exitCode = RecyclarrCli.Run(app, "config", "list", "templates", "--app-data", appDataDir.FullName);

        exitCode.Should().Be(0);

        TestCorrelator.GetLogEventsFromContextGuid(logContext.Guid);
    }
}
