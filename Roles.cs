using System;
using System.Collections.Generic;
using System.IO;
using Discord.WebSocket;
using Eris1.Helpers;
using System.Timers;

namespace Eris1.Settings
{
    public static class Roles
    {
        private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Roles.json");
        private static Dictionary<ulong, GuildRoleSettings> roleDict;
        private static Timer _saveTimer = new Timer();

        public static void Initialize()
        {
            roleDict = JsonManager.LoadJsonDict<ulong, GuildRoleSettings>(FilePath);

            _saveTimer.Interval = TimeSpan.FromMinutes(5).TotalMilliseconds;
            _saveTimer.AutoReset = true;
            _saveTimer.Elapsed += (sender, args) => Save();
            _saveTimer.Start();
        }

        public static GuildRoleSettings GetRoleSettings(ulong guildId)
        {
            if (!roleDict.ContainsKey(guildId))
                return new GuildRoleSettings();
            return roleDict[guildId];
        }
        public static void SetWarnRole(ulong guildId, ulong roleId)
        {
            if (!roleDict.ContainsKey(guildId))
                roleDict.Add(guildId, new GuildRoleSettings { WarnRoleId = roleId });
            else
                roleDict[guildId].WarnRoleId = roleId;
        }
        public static void SetMuteRole(ulong guildId, ulong roleId)
        {
            if (!roleDict.ContainsKey(guildId))
                roleDict.Add(guildId, new GuildRoleSettings { MuteRoleId = roleId });
            else
                roleDict[guildId].MuteRoleId = roleId;
        }

        public static void Save() => JsonManager.SaveJson(FilePath, roleDict);
    }
    public class GuildRoleSettings
    {
        public ulong MuteRoleId = 0;
        public ulong WarnRoleId = 0;
    }
}