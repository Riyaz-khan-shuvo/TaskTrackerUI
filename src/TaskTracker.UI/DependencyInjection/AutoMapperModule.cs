using Autofac;
using AutoMapper;

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
          cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
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
