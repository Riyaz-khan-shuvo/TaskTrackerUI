using Autofac;
using AutoMapper;
using TaskTracker.UI.Mappings;
using System.Reflection;

namespace TaskTracker.UI.DependencyInjection
{
    public class AutoMapperModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>(); // Applies all IMapFrom<> mappings
                });
                return config;
            }).AsSelf().SingleInstance();

            builder.Register(ctx =>
            {
                var config = ctx.Resolve<MapperConfiguration>();
                return config.CreateMapper();
            }).As<IMapper>().InstancePerLifetimeScope();
        }
    }
}
