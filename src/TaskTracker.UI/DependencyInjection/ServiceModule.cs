using Autofac;
using TaskTracker.Shared.ApiClients;
using TaskTracker.Repo.Configuration;
using System.Reflection;

namespace TaskTracker.UI.DependencyInjection
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpRequestHelper>().AsSelf().SingleInstance();

            builder.RegisterType<HttpClient>().SingleInstance();

            var apiClientsAssembly = Assembly.GetAssembly(typeof(BaseApiClient));

            builder.RegisterAssemblyTypes(apiClientsAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsSelf()
                .InstancePerLifetimeScope()
                .WithParameter(
                    (pi, ctx) => pi.ParameterType == typeof(HttpClient),
                    (pi, ctx) => ctx.Resolve<HttpClient>()
                )
                .WithParameter(
                    (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "baseUrl",
                    (pi, ctx) => ctx.Resolve<IConfiguration>()["ApiSettings:BaseUrl"] ?? "https://api.example.com/"
                );
        }
    }
}
