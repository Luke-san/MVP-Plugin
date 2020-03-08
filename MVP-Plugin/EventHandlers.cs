using System.Collections.Generic;
using System.IO;
using System;
using EXILED;
using EXILED.Extensions;

namespace MVP
{
    public class EventHandlers
    {
        public Plugin plugin;
        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        public List<ReferenceHub> playersWithKills = new List<ReferenceHub>();
        public List<int> playerKillNumber = new List<int>();

        public void OnPlayerDeath(ref PlayerDeathEvent ev)
        {
            ReferenceHub killer = Player.GetPlayer(ev.Killer.GetNickname());
            if (!playersWithKills.Contains(killer))
            {
                playersWithKills.Add(killer);
                playerKillNumber.Add(0);
            }
            playerKillNumber[playersWithKills.IndexOf(killer)] += 1;
        }

        public void OnRoundEnd()
        {
            string message = GetEndMesage;
            Map.Broadcast(message, 10, false);
            playersWithKills = new List<ReferenceHub>();
            playerKillNumber = new List<int>();
        }

        public int FindMVP()
        {
            if (playersWithKills.Count == 0)
                return (-1);
            int largestNumber = 0;
            for (int i=0; i<playersWithKills.Count; i++)
            {
                if (playerKillNumber[i] > playerKillNumber[largestNumber]) { largestNumber = i; }
            }
            return (largestNumber);
        }

        public string GetEndMesage
        {
            get
            {
                int MVP_ind = FindMVP();
                if (MVP_ind == -1)
                {
                    //return (EXILED.Plugin.Config.GetString("MVP_NoKill_Announce", "Nobody got any kills!"));
                    return (CustomConfigGet("MVP_NoKill_Announce", "Nobody got any kills!"));
                }

                ReferenceHub MVP_ent = playersWithKills[MVP_ind];
                string MVP = MVP_ent.GetNickname();
                //string message = EXILED.Plugin.Config.GetString("MVP_Announcement", "{MVP} was MVP of the match with {Kills} kills!");
                string message = CustomConfigGet("MVP_Announcement", "{MVP} was MVP of the match with {Kills} kills!");
                string Kills = playerKillNumber[playersWithKills.IndexOf(MVP_ent)].ToString();
                
                if (!message.Contains("{MVP}") || !message.Contains("{Kills}"))
                    return ($"{MVP} was MVP of the match with {Kills} kills!");

                message = message.Replace("{MVP}", MVP);
                message = message.Replace("{Kills}", Kills);
                return (message);
            }
        }

        public string CustomConfigGet(string key, string default_value)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string pluginPath = Path.Combine(appData, "Plugins");
            string path = Path.Combine(pluginPath, "MVP");
            string ConfigFile = Path.Combine(path, "MVP_Config.txt");

            string[] ConfigData = File.ReadAllLines(ConfigFile);

            for (int i=0; i<ConfigData.Length; i++)
            {
                if (ConfigData[i].Split(':')[0] == key)
                    return (ConfigData[i].Split(':')[1]);
            }
            return (default_value);
        }
    }
}