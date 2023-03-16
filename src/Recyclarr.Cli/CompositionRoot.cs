using System.IO.Abstractions;
using System.Reflection;
using Autofac;
using Autofac.Extras.Ordering;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using AutoMapper.EquivalencyExpression;
using Recyclarr.Common;
using Recyclarr.TrashLib;
using Spectre.Console.Cli;

namespace Recyclarr.Cli;

public static class CompositionRoot
{
    public static void Setup(ContainerBuilder builder)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName?.StartsWithIgnoreCase("Recyclarr") ?? false)
            .ToArray();

        RegisterAppPaths(builder);
        RegisterLogger(builder);

        builder.RegisterModule<VersionControlAutofacModule>();
        builder.RegisterModule<MigrationAutofacModule>();
        builder.RegisterModule<RepoAutofacModule>();
        builder.RegisterModule<CompatibilityAutofacModule>();
        builder.RegisterModule<ApiServicesAutofacModule>();
        builder.RegisterModule(new ConfigAutofacModule(assemblies));
        builder.RegisterModule<ServiceProcessorsAutofacModule>();
        builder.RegisterModule(new CommonAutofacModule(Assembly.GetExecutingAssembly()));

        // Needed for Autofac.Extras.Ordering
        builder.RegisterSource<OrderedRegistrationSource>();

        builder.RegisterModule<CacheAutofacModule>();
        builder.RegisterType<CacheStoragePath>().As<ICacheStoragePath>();
        builder.RegisterType<ServiceRequestBuilder>().As<IServiceRequestBuilder>();

        CommandRegistrations(builder);
        PipelineRegistrations(builder);

        builder.RegisterAutoMapper(c =>
            {
                c.AddCollectionMappers();
            },
            false, assemblies);

        builder.RegisterType<FlurlClientFactory>().As<IFlurlClientFactory>().SingleInstance();
    }

    private static void PipelineRegistrations(ContainerBuilder builder)
    {
        builder.RegisterModule<TagsAutofacModule>();
        builder.RegisterModule<CustomFormatAutofacModule>();
        builder.RegisterModule<QualityProfileAutofacModule>();
        builder.RegisterModule<QualitySizeAutofacModule>();
        builder.RegisterModule<ReleaseProfileAutofacModule>();

        builder.RegisterTypes(
                typeof(TagSyncPipeline),
                typeof(CustomFormatSyncPipeline),
                typeof(QualityProfileSyncPipeline),
                typeof(QualitySizeSyncPipeline),
                typeof(ReleaseProfileSyncPipeline))
            .As<ISyncPipeline>()
            .OrderByRegistration();
    }

    private static void RegisterLogger(ContainerBuilder builder)
    {
        builder.RegisterType<LogJanitor>().As<ILogJanitor>();
        builder.RegisterType<LoggerFactory>();
        builder.Register(c => c.Resolve<LoggerFactory>().Create()).As<ILogger>().SingleInstance();
    }

    private static void RegisterAppPaths(ContainerBuilder builder)
    {
        builder.RegisterType<FileSystem>().As<IFileSystem>();
        builder.RegisterType<DefaultAppDataSetup>();

        builder.Register(c =>
            {
                var appData = c.Resolve<AppDataPathProvider>();
                var dataSetup = c.Resolve<DefaultAppDataSetup>();
                return dataSetup.CreateAppPaths(appData.AppDataPath);
            })
            .As<IAppPaths>()
            .SingleInstance();
    }

    private static void CommandRegistrations(ContainerBuilder builder)
    {
        builder.RegisterTypes(
                typeof(AppPathSetupTask),
                typeof(JanitorCleanupTask))
            .As<IBaseCommandSetupTask>()
            .OrderByRegistration();

        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .AssignableTo<CommandSettings>();
    }
}
