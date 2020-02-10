using System;
using EXILED;

namespace MVP
{
    public class Plugin : EXILED.Plugin
    {
		public EventHandlers EventHandlers;

		public override void OnEnable()
		{
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
