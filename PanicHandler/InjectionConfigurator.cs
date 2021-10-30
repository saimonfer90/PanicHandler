using System;
using Microsoft.Extensions.Configuration;
using PanicHandler.Data;
using PanicHandler.Models;
using Serilog;
using SimpleInjector;

namespace PanicHandler
{
    /// <summary>
    /// This class is used to configure the DI environment
    /// </summary>
    public static class InjectionConfigurator
    {
        public static Container GetContainerService()
            => new();

        public static void InitializeContainer(this Container container)
        {
            var appsettings = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json";

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(appsettings, optional: false, reloadOnChange: true)
                .Build();

            container.RegisterInstance(configuration);
            container.RegisterSingleton<ConfigurationHandler>();

            container.RegisterSingleton<KeyBindingsCollection>();

            container.RegisterSingleton<ILogger>(()
                => new LoggerConfiguration()
                    .ReadFrom
                    .Configuration(configuration, sectionName: "PanicHandler:Serilog")
                    .CreateLogger());

            /*to manage hooks from input devices*/
            container.RegisterSingleton<HookConfig>();
            container.RegisterSingleton<HookEventsBinder>();
            container.RegisterSingleton<KeyInterceptor>();
        }
    }
}