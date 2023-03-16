using Autofac;
using Autofac.Extras.AggregateService;

namespace Recyclarr.TrashLib;

public class CustomFormatAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CustomFormatGuideService>().As<ICustomFormatGuideService>().SingleInstance();
        builder.RegisterType<ProcessedCustomFormatCache>().As<IPipelineCache>().AsSelf().InstancePerLifetimeScope();

        builder.RegisterType<CustomFormatService>().As<ICustomFormatService>();
        builder.RegisterType<CachePersister>().As<ICachePersister>();
        builder.RegisterType<CustomFormatLoader>().As<ICustomFormatLoader>();
        builder.RegisterType<CustomFormatParser>().As<ICustomFormatParser>();
        builder.RegisterType<CustomFormatCategoryParser>().As<ICustomFormatCategoryParser>();
        builder.RegisterType<CustomFormatDataLister>();

        builder.RegisterAggregateService<ICustomFormatPipelinePhases>();
        builder.RegisterType<CustomFormatConfigPhase>();
        builder.RegisterType<CustomFormatApiFetchPhase>();
        builder.RegisterType<CustomFormatTransactionPhase>();
        builder.RegisterType<CustomFormatPreviewPhase>();
        builder.RegisterType<CustomFormatApiPersistencePhase>();
    }
}
