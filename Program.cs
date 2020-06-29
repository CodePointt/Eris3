using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Runtime.InteropServices;
using Eris1.Settings;

namespace Eris1
{
    class Program
    {
        public static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        public DiscordSocketClient _client;
        private CommandHandler _commands;
        private EventHandler _events;
        private const string Token = "NzE5MzU5NzMxMjUyMTk5NDg1.XvKENQ.S-qLx2rzvde-gNuDEp-WIv4N6ig";
        public async Task StartAsync()
        {
            SetConsoleCtrlHandler(handler, true);

            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose, // Specify console verbose information level.
                MessageCacheSize = 1000, // Tell discord.net how long to store messages (per channel).
                ExclusiveBulkDelete = true // raise only MessagesBulkDeleted
            });

            _client.Log += (l) // Register the console log event.
                => Console.Out.WriteLineAsync(l.ToString());
            await _client.LoginAsync(TokenType.Bot, Token);
            await _client.StartAsync();

            Channels.Initialize(); // Load the log channels from their JSON file and start up the auto-save timer.
            Roles.Initialize();
            EventHandler.Initialize(_client);

            _commands = new CommandHandler(); // Initialize the command handler service
            await _commands.InstallAsync(_client);
            await Task.Delay(5000); // Wait for the bot to connect
            // Do anything that should be done once the bot is connected to all guilds
            await Task.Delay(-1); // Prevent the console from closing
        }

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                Channels.Save();
                Roles.Save();
            }
            return false;
        }
        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
                                               // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
    }
}