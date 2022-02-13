using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using MEC;

namespace BetterScp079
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Scp079Command : ICommand
    {
        public string Command => "scp079";

        public string[] Aliases => new string[]
        {
            "079",
        };

        public string Description => "SCP-079 commands";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get((CommandSender)sender);

            if (!player.Scp079Controller.Is079)
            {
                response = "You are not SCP-079";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "Using: .scp079 [CommandName]";
                response += string.Format("\nAllowedCommands: {0}", BetterScp079.Config.CommandsEnabled.Keys.ToList().GetAllowedCommand());
                return false;
            }

            var args = arguments.ToList();

            switch (args.ElementAt(0).ToString().ToLower())
            {
                case "time" or "roundtime":
                    {
                        var time = Round.ElapsedTime;
                        response = "\nCurrent time elapsed since the start of the round: \t";

                        if (time.Hours != 0)
                            response += string.Format("{0}:", time.Hours.ToString());

                        response += string.Format("{0}:{1}", time.Minutes.ToString("00"), time.Seconds.ToString("00"));
                        
                        return true;
                    }
                case "respawntime":
                    {
                        var time = Round.NextRespawn;
                        response = "\nTime until the next Respawn: \t";

                        response += string.Format("{0}:{1}", (time / 60).ToString("00"), (time % 60).ToString("00"));

                        return true;
                    }
                case "teslas":
                    {
                        if (player.Scp079Controller.Lvl < BetterScp079.Config.CommandLevels["Teslas"][0])
                        {
                            response = "\nInsufficient access";
                            return false;
                        }

                        if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["Teslas"][1])
                        {
                            response = "\nNot enough mana";
                            return false;
                        }

                        player.Scp079Controller.Energy -= BetterScp079.Config.CommandLevels["Teslas"][1];
                        Map.Teslas.Where(x => x.Allow079Interact).ToList().ForEach(x => x.Trigger(true));

                        response = "\nSuccessfully!";
                        return true;
                    }
                case "blackout":
                    {
                        if (player.Scp079Controller.Lvl < BetterScp079.Config.CommandLevels["Blackout"][0])
                        {
                            response = "Insufficient access";
                            return false;
                        }

                        if (args.Count == 1 || args.ElementAt(1).ToString().ToLower() == "room")
                        {
                            if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["Blackout"][2])
                            {
                                response = "Not enough mana";
                                return false;
                            }

                            player.Scp079Controller.Energy -= BetterScp079.Config.CommandLevels["Blackout"][2];
                            player.Room.LightsOff(BetterScp079.Config.CommandLevels["Blackout"][2] - 5);
                            player.Room.Tesla.Enable = false;
                            player.Room.Tesla.Allow079Interact = BetterScp079.Config.AllowTeslaInBlackout;
                            Timing.CallDelayed(BetterScp079.Config.CommandLevels["Blackout"][2] - 5, delegate ()
                            {
                                player.Room.Tesla.Enable = true;
                                player.Room.Tesla.Allow079Interact = !BetterScp079.Config.AllowTeslaInBlackout;
                            });

                            response = "Successfully!";
                            return true;
                        }
                        else if (args.ElementAt(1).ToString().ToLower() == "zone")
                        {
                            if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["Blackout"][3])
                            {
                                response = "Not enough mana";
                                return false;
                            }

                            player.Scp079Controller.Energy -= BetterScp079.Config.CommandLevels["Blackout"][3];
                            Lights.TurnOff(BetterScp079.Config.CommandLevels["Blackout"][3] - 10, player.Zone);

                            if (player.Zone == ZoneType.Heavy)
                            {
                                Map.Teslas.ForEach(x => x.Enable = false);
                                Map.Teslas.ForEach(x => x.Allow079Interact = BetterScp079.Config.AllowTeslaInBlackout);
                                Timing.CallDelayed(BetterScp079.Config.CommandLevels["Blackout"][3] - 10, delegate ()
                                {
                                    Map.Teslas.ForEach(x => x.Enable = true);
                                    Map.Teslas.ForEach(x => x.Allow079Interact = !BetterScp079.Config.AllowTeslaInBlackout);
                                });
                            }

                            response = "Successfully!";
                            return true;
                        }
                        else if (args.ElementAt(1).ToString().ToLower() == "facility" || args.ElementAt(1).ToString().ToLower() == "complex")
                        {
                            if (player.Scp079Controller.Lvl < BetterScp079.Config.CommandLevels["Blackout"][1])
                            {
                                response = "Insufficient access";
                                return false;
                            }

                            if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["Blackout"][4])
                            {
                                response = "Not enough mana";
                                return false;
                            }

                            player.Scp079Controller.Energy -= BetterScp079.Config.CommandLevels["Blackout"][4];
                            Lights.TurnOff(BetterScp079.Config.CommandLevels["Blackout"][4] - 15);
                            Map.Teslas.ForEach(x => x.Enable = false);
                            Map.Teslas.ForEach(x => x.Allow079Interact = BetterScp079.Config.AllowTeslaInBlackout);
                            Timing.CallDelayed(BetterScp079.Config.CommandLevels["Blackout"][4] - 15, delegate ()
                            {
                                Map.Teslas.ForEach(x => x.Enable = true);
                                Map.Teslas.ForEach(x => x.Allow079Interact = !BetterScp079.Config.AllowTeslaInBlackout);
                            });

                            response = "Successfully!";
                            return true;
                        }

                        player.Scp079Controller.Energy -= BetterScp079.Config.CommandLevels["Blackout"][1];
                        player.Room.LightsOff(15);
                        player.Room.Tesla.Enable = false;
                        player.Room.Tesla.Allow079Interact = BetterScp079.Config.AllowTeslaInBlackout;
                        Timing.CallDelayed(15, delegate ()
                        {
                            player.Room.Tesla.Enable = true;
                            player.Room.Tesla.Allow079Interact = !BetterScp079.Config.AllowTeslaInBlackout;
                        });

                        response = "Successfully!";
                        return true;
                    }
                default:
                    {
                        response = "Using: .scp079 [CommandName]";
                        response += string.Format("\nAllowedCommands: {0}", BetterScp079.Config.CommandsEnabled.Keys.ToList().GetAllowedCommand());
                        return false;
                    }
            }
        }
    }
}
