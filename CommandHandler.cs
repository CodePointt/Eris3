using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;
using System;

namespace Eris1
{
    /// <summary> Detect whether a message is a command, then execute it. </summary>
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        public static CommandService _cmds;

        public async Task InstallAsync(DiscordSocketClient c)
        {
            _client = c;                                                 // Save an instance of the discord client.
            _cmds = new CommandService();

            var assembly = Assembly.GetEntryAssembly();
            await _cmds.AddModulesAsync(assembly, null);    // Load all modules from the assembly.
            Modules.HelpModule.Initialize(assembly);

            _client.MessageReceived += HandleCommandAsync;   // Register the messagereceived event to handle commands.
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null)                                          // Check if the received message is from a user.
                return;
            var context = new SocketCommandContext(_client, msg);     // Create a new command context.


            int argPos = 0;
            if (msg.HasStringPrefix(Common.Prefix, ref argPos, StringComparison.OrdinalIgnoreCase)) // Check if the message has a string prefix
            {
                if (msg.Author.IsBot)
                {
                    return; 
                }
                else
                {
                    var result = await _cmds.ExecuteAsync(context, argPos, null);
                }
            }
        }
    }
}