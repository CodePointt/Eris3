using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;
using System;
using Discord;

namespace Eris1
{
    public class EventHandler
    {
        private static readonly Color EmbedColor = new Color(192, 203, 255);
        private static readonly Color JoinColor = new Color(158, 238, 74);
        private static readonly Color LeaveColor = new Color(225, 87, 51);
        public static void Initialize(DiscordSocketClient client) // call this in program.cs with the rest of the initialize methods
        {
            client.UserJoined += AnnounceJoinedUser; //Hook into the UserJoined event of the client.
            client.UserLeft += AnnounceUserLeave;
        }

        //I got really used to writing arduino again soo whoops

        private static async Task AnnounceJoinedUser(SocketGuildUser user)
        {
            var channelId = Settings.Channels.GetChannels(user.Guild.Id).WelcomeChannelId; // You can only use var inside a function. Also user doesnt exist outside of one oops
            if (channelId == 0)
            {
                // Channel is not set

                return;
            }
            SocketTextChannel channel;
            try
            {
                channel = user.Guild.GetTextChannel(channelId);
            }
            catch (Exception ex)
            {
                // The channel was likely deleted

                Console.WriteLine(ex);
                return;
            }
            var guildName = user.Guild.Name;
            var tag = user.ToString();
            var eb = new EmbedBuilder();

            eb.WithColor(JoinColor);
            eb.WithTitle("User Join | " + tag);
            eb.WithDescription($"{user.Mention} joined {guildName}.");
            eb.WithFooter($"{user}");
            eb.WithCurrentTimestamp();
            await channel.SendMessageAsync("", false, eb.Build());

        }

        private static async Task AnnounceUserLeave(SocketGuildUser user)
        {
            var channelId = Settings.Channels.GetChannels(user.Guild.Id).WelcomeChannelId; // You can only use var inside a function. Also user doesnt exist outside of one oops
            if (channelId == 0)
            {
                // Channel is not set

                return;
            }
            SocketTextChannel channel;
            try
            {
                channel = user.Guild.GetTextChannel(channelId);
            }
            catch (Exception ex)
            {
                // The channel was likely deleted

                Console.WriteLine(ex);
                return;
            }
            var guildName = user.Guild.Name;
            var tag = user.ToString();
            var eb = new EmbedBuilder();

            eb.WithColor(LeaveColor);
            eb.WithTitle("User Leave | " + tag);
            eb.WithDescription($"{user.Mention} left {guildName}.");
            eb.WithFooter($"{user}");
            eb.WithCurrentTimestamp();
            await channel.SendMessageAsync("", false, eb.Build());

        }
    }

}