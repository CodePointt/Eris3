using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using Eris1.Attributes;
using System.Collections.Generic;
using System.Globalization;
using Discord.Rest;
using System.Threading;
using Eris1.Settings;

namespace Eris1.Modules
{
    [Name("MainCommands")]
    public class MainCommandModule : ModuleBase<SocketCommandContext>
    {
        private static readonly Color EmbedColor = new Color(192, 203, 255);
        // here actually let me put it somewhere else
        [Command("thonk")]
        [Summary("sends the thonk emoji")]
        [Category("Emotes")]
        [Useage("`<prefix>thonk")]
        public async Task Thonk([Remainder] string args = null)
        {
            var originalThonk = Emote.Parse("<:Thonk: 356936480221954048 >");
            Context.Message.DeleteAsync();
            await Context.Channel.SendMessageAsync(originalThonk as IEmote);
        }
        [Command("embed")]
        [Summary("Create a custom embed")]
        [Category("Embed")]
        [Useage("`<prefix>embed [title text] | [body text]`")]
        public async Task Announce([Remainder] string args = null)
        {
           
            if (args == null)
            {
                await ReplyAsync("Please put your embed content after the command.");
                return;
            }
            var split = args.Split('|');
            if (split[0].Length <= 0)
            {
                await ReplyAsync("Please put your embed content after the command.");
                return;
            }
            if (split[1].Length <=0)
            {
                await ReplyAsync("Please put your embed content after the command.");
                return;
            }
            Context.Message.DeleteAsync();
            var eb = new EmbedBuilder();
            eb.WithColor(EmbedColor);
            eb.WithTitle(split[0]);
            eb.WithDescription(split[1]);
            eb.WithFooter("Sent from " + Context.User);

            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("vote")]
        [Summary("Start a vote (Eris reacts with up and down arrows by default)")]
        [Category("Embed")]
        [Useage("`<prefix>vote [vote description/information]` `<prefix>vote [vote topic] | [vote description/information]`")]
        public async Task Vote([Remainder] string args = null)
        {
            if (args == null)
            {
                await ReplyAsync("Please put your vote information after the command.");
                return;
            }
            var split = args.Split('|');
            if (split[0].Length <= 0)
            {
                await ReplyAsync("Please put your vote information after the command.");
                return;
            }
            if (args.ToString().Contains("|"))
            {
                var length = split[1].Length;
                var title = split[0];
                var content = length <= 0 ? $"{title}" : $"{split[1]}";
                var vote = $"Vote";
                vote += length > 0 ? ": " : "";
                var eb = new EmbedBuilder();
                eb.WithColor(EmbedColor);
                eb.WithTitle(vote + title);
                eb.WithDescription(content);
                eb.WithFooter("Vote started by " + Context.User);

                var message = await Context.Channel.SendMessageAsync("", false, eb.Build());
                var upArrow = Emote.Parse("<:GreenUpArrow: 716046937727434882>");
                var downArrow = Emote.Parse("<:RedDownArrow:716046912255688725>");
                await message.AddReactionAsync(upArrow);
                await message.AddReactionAsync(downArrow);
                return;
            }
            Context.Message.DeleteAsync();
            var eb1 = new EmbedBuilder();
            eb1.WithColor(EmbedColor);
            eb1.WithTitle("Vote");
            eb1.WithDescription(args);
            eb1.WithFooter("Vote started by " + Context.User);
            await Context.Channel.SendMessageAsync("", false, eb1.Build());
        }

        [Command("rate")]
        [Summary("Rates the specified item")]
        [Category("Opinion")]
        [Useage("`<prefix>rate [item to rate]`")]
        public async Task Rate([Remainder] string args = null) // Get the text passed to the command as a string
        {
            if (args == null)
            {
                // Nothing was specified
                await ReplyAsync("Please try again and tell me what to rate.");
                return;
            }

            var rand = new Random();
            var rating = rand.Next(1, 11); // Pick a random number between 1 and 10 
            await ReplyAsync($"I rate {args} a {rating} out of 10.");

        }
        [Command("help")] // Simple help command that enumerates through the available commands using the "Summary" as the description.
        [Summary("Displays the help menu")]
        [Category("Info")]
        [Useage("`<prefix>help`\n`<prefix>help [command]`")]
        public async Task Help([Remainder] string args = null)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            if (args == null)
            {
                var commandsByCategory = new Dictionary<string, List<HelpObject>>();
                foreach (var command in HelpModule.GetAllCommands())
                {

                    if (string.IsNullOrWhiteSpace(command.Category))
                        continue;
                    if (!commandsByCategory.ContainsKey(command.Category))
                        commandsByCategory.Add(command.Category, new List<HelpObject>());
                    commandsByCategory[command.Category].Add(command);
                }

                var eb = new EmbedBuilder();
                eb.WithColor(EmbedColor);
                eb.WithTitle("Commands");
                eb.WithDescription($"My prefix is `{Common.Prefix}`.");

                var fieldNum = 0;
                foreach (var pair in commandsByCategory)
                {
                    if (pair.Key.ToLower() == "moderation") continue;
                    if (fieldNum >= 25) // You can only have 25 fields per embed
                    {
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        eb = new EmbedBuilder();
                        fieldNum = 0;
                    }
                    
                    var iterator = 1;
                    var fieldValue = string.Empty;
                    foreach (var commandInfo in pair.Value)
                    {
                        fieldValue += "`" + commandInfo.Command + "` ";
                        if ((iterator % 3) == 0) // Check if iterator is a multiple of 3
                            fieldValue += "\n";
                        iterator++;
                    }
                    

                    eb.AddField(textInfo.ToTitleCase(pair.Key), fieldValue, true);
                    fieldNum++;

                }


                var commands = commandsByCategory["moderation"];
                var fieldValue2 = string.Empty;
                
                var iterator2 = 1;
                foreach (var commandInfo in commands)
                {
                    fieldValue2 += "`" + commandInfo.Command + "` ";
                    if ((iterator2 % 3) == 0) // Check if iterator is a multiple of 3
                        fieldValue2 += "\n";
                    iterator2++;
                }
                eb.AddField("Moderation", fieldValue2, true);
                await Context.Channel.SendMessageAsync("", false, eb.Build());


            }
            else
            {
                var commandInfo = HelpModule.GetCommandByName(args.ToLower());
                if (commandInfo == null) // Command doesn't exist
                {
                    await ReplyAsync($"That command doesn't exist. To get a list of commands, type `{Common.Prefix}help`.");
                }
                else
                {
                    var eb = new EmbedBuilder();
                    eb.WithColor(EmbedColor);
                    eb.WithTitle(textInfo.ToTitleCase(commandInfo.Command));
                    eb.WithDescription(commandInfo.Summary);
                    if (!string.IsNullOrWhiteSpace(commandInfo.Useage))
                        eb.AddField("Useage", commandInfo.Useage.Replace("<prefix>", Common.Prefix));
                    await Context.Channel.SendMessageAsync("", false, eb.Build());
                }
            }
        }

        [Command("info")]
        [Summary("Displays some basic information and stats for the bot.")]
        [Category("Info")]
        public async Task info([Remainder] string args = null)
        {
            if (args == null)
            {
                var eb = new EmbedBuilder();
                eb.WithColor(EmbedColor);
                eb.WithTitle("Info");
                eb.AddField(name: "Info", value: "Developer: <@!682327214934589446>\n" +
                                                  $"Library: Discord.Net ({DiscordConfig.Version})\n" +
                                                  $"Runtime: {RuntimeInformation.FrameworkDescription}{RuntimeInformation.OSArchitecture}\n" +
                                                  $"Uptime: {GetUptime()}");
                eb.AddField(name: "Stats", value: $"Heap Size: {GetHeapSize()} MB\n" +
                                                  $"Guilds: {Context.Client.Guilds.Count}\n" +
                                                  $"Channels: {Context.Client.Guilds.Sum(g => g.Channels.Count)}\n" +
                                                  $"Users: {Context.Client.Guilds.Sum(g => g.Users.Count)}");
                eb.WithFooter("You're using the Beta version of Eris v3.0.3");

                await Context.Channel.SendMessageAsync("", false, eb.Build());
                return;
            }


        }
        private static string GetUptime()
            => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();

        [Command("support")]
        [Summary("Sends you the link to the Eris Help Server")]
        [Category("Info")]
        [Useage("`<prefix>support`")]
        public async Task Support([Remainder] string args = null)
        {

            var eb = new EmbedBuilder();
            eb.WithColor(EmbedColor);
            eb.WithTitle("Need more help?");
            eb.WithDescription("Join the [Eris support server](https://discord.gg/29m46En)");
            eb.WithFooter("Suggestions and bug reports are also sent here");

            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
    }
}