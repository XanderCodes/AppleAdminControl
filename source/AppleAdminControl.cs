﻿using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace AppleAdminControl
{
    public class Main : RocketPlugin
    {
        protected override void Load()
        {
            Logger.Log($"AppleAdminControl version {Assembly.GetName().Version} has been loaded!", ConsoleColor.Magenta);
            Logger.Log($"AppleAdminControl created by AppleManYT#8750 - For support, contact me via the UnturnedStore.com Discord, or submit a GitHub issue!", ConsoleColor.Cyan);

            U.Events.OnPlayerConnected += OnPlayerConnect;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnect;

            BarricadeManager.onTransformRequested += OnBarricadeTransformRequested;
            StructureManager.onTransformRequested += OnStructureTransformRequested;
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnect;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnect;

            BarricadeManager.onTransformRequested -= OnBarricadeTransformRequested;
            StructureManager.onTransformRequested -= OnStructureTransformRequested;

            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Magenta);
        }

        public void OnPlayerConnect(UnturnedPlayer player)
        {
            if (player.HasPermission("admin.freecam"))
            {
                player.Player.look.sendFreecamAllowed(true);
            } 
            else
            {
                player.Player.look.sendFreecamAllowed(false);
            }

            if (player.HasPermission("admin.editor"))
            {
                player.Player.look.sendWorkzoneAllowed(true);
            }
            else
            {
                player.Player.look.sendWorkzoneAllowed(false);
            }

            if (player.HasPermission("admin.spectate"))
            {
                player.Player.look.sendSpecStatsAllowed(true);
            }
            else
            {
                player.Player.look.sendSpecStatsAllowed(false);
            }
        }

        public void OnPlayerDisconnect(UnturnedPlayer player)
        {
            player.Player.look.sendFreecamAllowed(false);
            player.Player.look.sendWorkzoneAllowed(false);
            player.Player.look.sendSpecStatsAllowed(false);
        }

        private void OnBarricadeTransformRequested(CSteamID instigator, byte x, byte y, ushort plant, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow)
        {
            if (BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion region))
            {
                foreach (BarricadeDrop drop in region.drops)
                {
                    if (drop.instanceID == instanceID)
                    {
                        ulong owner = drop.GetServersideData().owner;

                        CSteamID ownerID = (CSteamID)owner;
                        if (ownerID != instigator)
                        {
                            if (!UnturnedPlayer.FromCSteamID(instigator).HasPermission("admin.editor.otherobjects"))
                            {
                                shouldAllow = false;
                                ChatManager.serverSendMessage("You can only move your own barricades!!", Color.red, null, UnturnedPlayer.FromCSteamID(instigator).SteamPlayer(), EChatMode.SAY, null, true);
                            }
                        }
                    }
                }
            }
        }

        private void OnStructureTransformRequested(CSteamID instigator, byte x, byte y, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow)
        {
            foreach (StructureRegion region in StructureManager.regions)
            {
                foreach (StructureDrop drop in region.drops)
                {
                    if (drop.instanceID == instanceID)
                    {
                        ulong owner = drop.GetServersideData().owner;
                        CSteamID ownerID = (CSteamID)owner;
                        if (ownerID != instigator)
                        {
                            if (!UnturnedPlayer.FromCSteamID(instigator).HasPermission("admin.editor.otherobjects"))
                            {
                                shouldAllow = false;
                                ChatManager.serverSendMessage("You can only move your own structures!!", Color.red, null, UnturnedPlayer.FromCSteamID(instigator).SteamPlayer(), EChatMode.SAY, null, true);
                            }
                        }
                    }
                }
            }
        }
    }
}