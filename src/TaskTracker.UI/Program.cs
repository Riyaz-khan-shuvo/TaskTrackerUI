using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using TaskTracker.Shared.ApiClients;
using TaskTracker.UI.DependencyInjection;
using TaskTracker.UI.Mappings;
using TaskTracker.UI.Middlewares;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

try
{
    // ---------------------------
    // Add services to container
    // ---------------------------
    builder.Services.AddControllersWithViews();
    builder.Services.AddMemoryCache(); // for caching in repo layer
                                       // Register HttpClient factory
    builder.Services.AddHttpClient();
    builder.Services.Configure<FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit = 104857600; // 100 MB
    });

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromHours(2);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    //builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


    // ---------------------------
    // Configure Autofac DI
    // ---------------------------
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<RepositoryModule>();
        containerBuilder.RegisterModule<ServiceModule>();
        containerBuilder.RegisterModule<AutoMapperModule>();
    });

    var app = builder.Build();
    app.UseRouting();
    app.UseSession();
    app.UseTokenExpirationMiddleware();

    BaseApiClient.GetTokenFromSession = () =>
    {
        var httpContextAccessor = new HttpContextAccessor();
        var context = httpContextAccessor.HttpContext;
        return context?.Session.GetString("Token");
    };
    // ---------------------------
    // Configure HTTP request pipeline
    // ---------------------------
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Areas", "Admin", "js")),
        RequestPath = "/js"
    });
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Areas", "Setup", "js")),
        RequestPath = "/js"
    });
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
     Path.Combine(builder.Environment.ContentRootPath, "Areas", "Common", "js")),
        RequestPath = "/js"
    });
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseAuthorization();

    // Areas route
    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    // Default route
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Application failed to start: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
    throw; 
}