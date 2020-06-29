using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Eris1
{
    public static class Extensions
    {
        public static void DeleteAfterDelay(this IUserMessage message, TimeSpan delay)
        {
            Task.Run(async () =>
            {
                Thread.Sleep(delay);
                await message.DeleteAsync();
            });
        }
        public static async Task SendTemporaryMessageAsync(this IMessageChannel channel, string message, TimeSpan deleteAfter)
        {
            var msg = await channel.SendMessageAsync(message);
            msg.DeleteAfterDelay(deleteAfter);
        }

        public static async Task TempReplyAsync(this SocketCommandContext context, string message, TimeSpan deleteAfter)
        {
            await context.Channel.SendTemporaryMessageAsync(message, deleteAfter);
        }
        // With these, within a command you can now use "Context.TempReplyAsync(message, delay)", "Context.Channel.SendTemporaryMessageAsync(message, delay)";
        // and "Message.DeleteAfterDelay(delay)"

    }
}