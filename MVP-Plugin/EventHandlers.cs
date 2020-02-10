using System.Collections.Generic;
using EXILED;
using EXILED.Extensions;
using UnityEngine;

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
            ReferenceHub mvp = findMVP();
            foreach (GameObject ply in PlayerManager.players)
            {
                Player.GetPlayer(ply.name).Broadcast(10, $"{mvp.GetNickname()} was MVP of the match with {playerKillNumber[playersWithKills.IndexOf(mvp)].ToString()} kills!");
            }
            playersWithKills = new List<ReferenceHub>();
            playerKillNumber = new List<int>();
        }

        public ReferenceHub findMVP()
        {
            int largestNumber = 0;
            for (int i=0; i<playersWithKills.Count; i++)
            {
                if (playerKillNumber[i] > playerKillNumber[largestNumber]) { largestNumber = i; }
            }
            return (playersWithKills[largestNumber]);
        }
    }
}