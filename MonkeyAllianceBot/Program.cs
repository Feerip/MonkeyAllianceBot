using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using MonkeyAllianceBot.Services;

namespace MonkeyAllianceBot
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            IHostBuilder builder = new HostBuilder()
                   .ConfigureAppConfiguration(x =>
                   {
                       IConfigurationRoot configuration = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("Config/appsettings.json", false, true)
                           .Build();

                       x.AddConfiguration(configuration);
                   })
                   .ConfigureLogging(logging =>
                   {
                       logging.AddConsole();
                       logging.SetMinimumLevel(LogLevel.Trace); // Defines what kind of information should be logged (e.g. Debug, Information, Warning, Critical) adjust this to your liking
                })
                   .ConfigureDiscordHost((context, config) =>
                   {
                       config.SocketConfig = new DiscordSocketConfig
                       {
                           LogLevel = LogSeverity.Verbose, // Defines what kind of information should be logged from the API (e.g. Verbose, Info, Warning, Critical) adjust this to your liking
                        AlwaysDownloadUsers = true,
                           MessageCacheSize = 1000,
                           GatewayIntents = GatewayIntents.All,
                           LargeThreshold = 250,

                       };

                       config.Token = context.Configuration["token"];

                   })
                   .UseCommandService((context, config) =>
                   {
                       config.CaseSensitiveCommands = false;
                       config.LogLevel = LogSeverity.Debug;
                       config.DefaultRunMode = RunMode.Sync;
                   })
                   .ConfigureServices((context, services) =>
                   {
                       services.AddHostedService<CommandHandler>();
                   })
                   .UseConsoleLifetime();

            IHost host = builder.Build();
            using (host)
            {

                await host.RunAsync();

            }
            Environment.ExitCode = 1;
            return 1;
        }
    }
}
