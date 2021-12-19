using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DevTools.Server
{
    public class MainServer : BaseScript
    {
        public MainServer()
        {
            this.EventHandlers.Add("playerJoining", new Action<Player>(OnPlayerJoining));
        }

        #region Events
        internal void OnPlayerJoining([FromSource] Player player)
        {
            try
            {
                bool isAllowed = IsPlayerAceAllowed(player?.Handle, "command");
                if (!isAllowed) return;

                player?.TriggerEvent("admin_manager:setAcePermission");
                Logger.LogDebug($"[{nameof(OnPlayerJoining)}] - Player: {player?.Handle} granted all permissions.");
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
        #endregion
    }
}
