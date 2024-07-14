
using MephiWatcher.Parsers;

namespace MephiWatcher
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var configFactory = new ConfigFactory("config.json");
            var mephiParser = new MephiParser();
            Application.Run(new MainForm(configFactory, mephiParser));
        }
    }
}