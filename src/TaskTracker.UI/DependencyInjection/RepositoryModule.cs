using Autofac;
using TaskTracker.Repo.Implementations;
using TaskTracker.Repo.Interfaces;
using System.Reflection;

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

        }
    }
}