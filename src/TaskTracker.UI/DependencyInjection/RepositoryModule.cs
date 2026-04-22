using Autofac;
using System.Reflection;
using TaskTracker.Repo.Implementations;
using TaskTracker.Repo.Interfaces;

namespace TaskTracker.UI.DependencyInjection
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetAssembly(typeof(MenuAuthorizationRepo));

            builder.RegisterType<MenuAuthorizationRepo>()
                       .As<IMenuAuthorizationRepo>()
                       .InstancePerLifetimeScope();

            builder.RegisterType<AccountRepo>()
                .As<IAccountRepo>()
                     .InstancePerLifetimeScope();

            builder.RegisterType<TaskRepo>()
         .AsSelf()
         .InstancePerLifetimeScope();

        }
    }
}