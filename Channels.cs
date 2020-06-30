using Discord.WebSocket;
using Discord;
using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;

// This is ready to use now. It is set to save every 5 minutes and when the program closes. It is also set to save the data
// in a file called "LogChannels.json" within the same folder as the executeable

namespace Eris1.Settings
{
    public static class Channels
    {
        private static readonly string JsonFilePath = Path.Combine(AppContext.BaseDirectory, "Channels.json"); // You can change this if you want.
        private static Dictionary<ulong, GuildChannelInfo> ChannelDict;
        private static Timer _saveTimer = new Timer();

        public static void Initialize()
        {
            ChannelDict = Helpers.JsonManager.LoadJsonDict<ulong, GuildChannelInfo>(JsonFilePath);

            _saveTimer.Interval = TimeSpan.FromMinutes(5).TotalMilliseconds;
            _saveTimer.AutoReset = true;
            _saveTimer.Elapsed += SaveTimerElapsed;
            _saveTimer.Start();
        }

        private static void SaveTimerElapsed(object sender, ElapsedEventArgs e) => Save();
        public static void Save() => Helpers.JsonManager.SaveJson(JsonFilePath, ChannelDict);

        public static GuildChannelInfo GetChannels(ulong guildId)
        {
            if (!ChannelDict.ContainsKey(guildId)) // If the dictionary doesn't contain an entry for the guild
                return new GuildChannelInfo();
            return ChannelDict[guildId];
        }

        public static void SetLogChannel(ulong guildId, ulong channelId)
        {
            if (!ChannelDict.ContainsKey(guildId))
                ChannelDict.Add(guildId, new GuildChannelInfo  {  LogChannelId = channelId });
            else
                ChannelDict[guildId].LogChannelId = channelId;
        }
        public static void SetWelcomeChannel(ulong guildId, ulong channelId)
        {
            if (!ChannelDict.ContainsKey(guildId))
                ChannelDict.Add(guildId, new GuildChannelInfo { WelcomeChannelId = channelId });
            else
                ChannelDict[guildId].WelcomeChannelId = channelId;
        }


    }

    public class GuildChannelInfo
    {
        public ulong WelcomeChannelId { get; set; } = 0;
        public ulong LogChannelId { get; set; } = 0;
    }


}