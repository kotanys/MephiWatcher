
using CefSharp.OffScreen;
using CefSharp;
using MephiWatcher.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace MephiWatcher
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var settings = new CefSettings();
            Cef.Initialize(settings);

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var services = GetServiceProvider();
            var form = services.GetRequiredService<MainForm>();
            Application.Run(form);
        }

        private static ServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IConfigFactory>(new ConfigFactory("config.json"));
            services.AddSingleton<IVuzParser, MephiParser>();
            services.AddSingleton<IVuzParser, MireaParser>();
            services.AddSingleton<IHttpClientProvider, HttpClientProvider>();
            services.AddTransient<MainForm>();
            return services.BuildServiceProvider();
        }
    }
}