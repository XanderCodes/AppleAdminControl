using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppleAdminControl
{
    public class Main : RocketPlugin
    {
        protected override void Load()
        {
            Logger.Log($"AppleAdminControl version {Assembly.GetName().Version} has been loaded!", ConsoleColor.Magenta);
            Logger.Log($"AppleAdminControl created by AppleManYT#8750, for use on RedstonePlugins.com!", ConsoleColor.Cyan);

            U.Events.OnPlayerConnected += OnPlayerConnect;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnect;
        }

        protected override void Unload()
        {
            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Magenta);
        }

        public void OnPlayerConnect(UnturnedPlayer player)
        {
            if (player.HasPermission("admin.freecam"))
            {
                player.Player.look.sendFreecamAllowed(true);
            }
            else if (player.HasPermission("admin.editor"))
            {
                player.Player.look.sendWorkzoneAllowed(true);
            }
            else if (player.HasPermission("admin.spectate"))
            {
                player.Player.look.sendSpecStatsAllowed(true);
            }
            else
            {
                player.Player.look.sendFreecamAllowed(false);
                player.Player.look.sendWorkzoneAllowed(false);
                player.Player.look.sendSpecStatsAllowed(false);
            }
        }

        public void OnPlayerDisconnect(UnturnedPlayer player)
        {
            player.Player.look.sendFreecamAllowed(false);
            player.Player.look.sendWorkzoneAllowed(false);
            player.Player.look.sendSpecStatsAllowed(false);
        }
    }
}