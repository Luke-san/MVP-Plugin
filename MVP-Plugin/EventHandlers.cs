﻿using System.Collections.Generic;
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
            Map.Broadcast(message, 1, false);
            playersWithKills = new List<ReferenceHub>();
            playerKillNumber = new List<int>();
        }

        public int FindMVP()
        {
            int largestNumber = -1;
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
                    return (EXILED.Plugin.Config.GetString("MVP_NoKill_Announce", "Nobody got any kills!"));
                
                ReferenceHub MVP_ent = playersWithKills[MVP_ind];
                string MVP = MVP_ent.GetNickname();
                string message = EXILED.Plugin.Config.GetString("MVP_Announcement", "{MVP} was MVP of the match with {Kills} kills!");
                string Kills = playerKillNumber[playersWithKills.IndexOf(MVP_ent)].ToString();
                
                if (!message.Contains("{MvP}") || !message.Contains("{Kills}"))
                    return ($"{MVP} was MVP of the match with {Kills} kills!");

                message.Replace("{MVP}", MVP);
                message.Replace("{Kills}", Kills);
                return (message);
            }
        }
    }
}