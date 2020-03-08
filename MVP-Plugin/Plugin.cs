using System;
using System.IO;
using EXILED;

namespace MVP
{
    public class Plugin : EXILED.Plugin
    {
		public EventHandlers EventHandlers;

		public override void OnEnable()
		{
			string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string pluginPath = Path.Combine(appData, "Plugins");
			string path = Path.Combine(pluginPath, "MVP");
			string ConfigFile = Path.Combine(path, "MVP_Config.txt");
			if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
			if (!File.Exists(ConfigFile)) { 
				File.Create(ConfigFile).Close();
				File.WriteAllText(ConfigFile, "MVP_NoKill_Announce:Nobody got any kills!\nMVP_Announcement:{MVP} was MVP of the match with {Kills} kills!");
			}
			
			EventHandlers = new EventHandlers(this);
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDeath;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
		}
		public override void OnDisable()
		{
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDeath;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
		}
		public override void OnReload()
		{
			throw new NotImplementedException();
		}

		public override string getName { get; } = "MVP by Luke-san";
	}
}
