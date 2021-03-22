using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace AdminManager.Server
{
    public class Manager : BaseScript
    {
        [EventHandler("playerJoining")]
        private void OnPlayerJoining([FromSource] Player player)
        {
            TriggerClientEvent("admin_manager:setAcePermission", IsPlayerAceAllowed(player.Handle, "command"));
        }
    }
}
