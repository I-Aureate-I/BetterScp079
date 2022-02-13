using CommandSystem;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

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

        internal bool _cooldown = false;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get((CommandSender)sender);

            if (!player.Scp079Controller.Is079)
            {
                response = "\nYou are not SCP-079";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "\nUsing: .scp079 [CommandName]";
                response += string.Format("\nAllowedCommands: \n{0}", string.Join("\n", BetterScp079.Config.CommandsEnabled.Keys.ToList()));
                return false;
            }

            List<string> args = arguments.ToList();

            if (_cooldown && !args.ElementAt(0).ToString().ToLower().Contains("time"))
            {
                response = "\nCooldown";
                return false;
            }

            switch (args.ElementAt(0).ToString().ToLower())
            {
                case "time" or "roundtime":
                    {
                        if (!BetterScp079.Config.CommandsEnabled["RoundTime"])
                        {
                            response = "\nCommand not allowed";
                            return false;
                        }

                        var time = Round.ElapsedTime;
                        response = "\nCurrent time elapsed since the start of the round: \t";

                        if (time.Hours != 0)
                            response += string.Format("{0}:", time.Hours.ToString());

                        response += string.Format("{0}:{1}", time.Minutes.ToString("00"), time.Seconds.ToString("00"));
                        
                        return true;
                    }
                case "respawntime":
                    {
                        if (!BetterScp079.Config.CommandsEnabled["TimeUntilRespawn"])
                        {
                            response = "\nCommand not allowed";
                            return false;
                        }

                        float time = Round.NextRespawn;
                        response = "\nTime until the next Respawn: \t";

                        response += string.Format("{0}:{1}", (time / 60).ToString("00"), (time % 60).ToString("00"));

                        return true;
                    }
                case "gas":
                    {
                        if (!BetterScp079.Config.CommandsEnabled["Gas"])
                        {
                            response = "\nCommand not allowed";
                            return false;
                        }

                        if (player.Scp079Controller.Lvl < BetterScp079.Config.CommandLevels["Gas"][0])
                        {
                            response = "\nInsufficient access";
                            return false;
                        }

                        if (args.Count == 1)
                        {
                            if (player.Zone == ZoneType.Surface)
                            {
                                response = "\nError...";
                                return false;
                            }

                            if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["Gas"][1] && !player.BypassMode)
                            {
                                response = "\nNot enough mana";
                                return false;
                            }

                            if (player.Room.Type.ToString().ToLower().Contains("chkp"))
                            {
                                if (player.Room.ToString().EndsWith("A"))
                                {
                                    Map.Lifts.Where(x => x.Type.ToString().Contains("ElA")).ToList().ForEach(x => x.Locked = true);
                                    Timing.CallDelayed(15f, () => Map.Lifts.Where(x => x.Type.ToString().Contains("ElA")).ToList().ForEach(x => x.Locked = false));
                                }
                                else
                                {
                                    Map.Lifts.Where(x => x.Type.ToString().Contains("ElB")).ToList().ForEach(x => x.Locked = true);
                                    Timing.CallDelayed(15f, () => Map.Lifts.Where(x => x.Type.ToString().Contains("ElB")).ToList().ForEach(x => x.Locked = false));
                                }
                            }
                            foreach (var door in player.Room.Doors)
                            {
                                door.Locked = true;
                                door.Open = false;

                                Timing.CallDelayed(15f, delegate ()
                                {
                                    door.Locked = false;
                                });
                            }

                            player.Scp079Controller.Energy -= player.BypassMode ? 0 : BetterScp079.Config.CommandLevels["Gas"][1];
                            player.Room.Players.Where(x => x.Role != RoleType.Scp079 && x.GodMode != false).ToList().ForEach(x => x.EnableEffect(EffectType.Decontaminating, 15));
                            float previousIntensity = player.Room.LightIntensity;
                            player.Room.LightIntensity = (player.Room.LightIntensity / 5) * 4;
                            Timing.CallDelayed(15f, () => player.Room.LightIntensity = previousIntensity);

                            _cooldown = true;
                            Timing.CallDelayed(BetterScp079.Config.CooldownTime, () => _cooldown = false);
                            response = "\nSuccessfully!";
                            return true;
                        }
                        else
                        {
                            switch (args.ElementAt(1).ToString().ToLower())
                            {
                                case "light" or "lightzone":
                                    {
                                        if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["Gas"][2] && !player.BypassMode)
                                        {
                                            response = "\nNot enough mana";
                                            return false;
                                        }

                                        player.Scp079Controller.Energy -= player.BypassMode ? 0 : BetterScp079.Config.CommandLevels["Gas"][2];
                                        Decontamination.InstantStart();

                                        _cooldown = !player.BypassMode;
                                        Timing.CallDelayed(BetterScp079.Config.CooldownTime, () => _cooldown = false);
                                        response = "\nSuccessfully!";
                                        return true;
                                    }
                                case "default" or "room":
                                default:
                                    {
                                        if (player.Zone == ZoneType.Surface)
                                        {
                                            response = "\nError...";
                                            return false;
                                        }

                                        if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["Gas"][1] && !player.BypassMode)
                                        {
                                            response = "\nNot enough mana";
                                            return false;
                                        }

                                        if (player.Room.Type.ToString().ToLower().Contains("chkp"))
                                        {
                                            if (player.Room.ToString().EndsWith("A"))
                                            {
                                                Map.Lifts.Where(x => x.Type.ToString().Contains("ElA")).ToList().ForEach(x => x.Locked = true);
                                                Timing.CallDelayed(15f, () => Map.Lifts.Where(x => x.Type.ToString().Contains("ElA")).ToList().ForEach(x => x.Locked = false));
                                            }
                                            else
                                            {
                                                Map.Lifts.Where(x => x.Type.ToString().Contains("ElB")).ToList().ForEach(x => x.Locked = true);
                                                Timing.CallDelayed(15f, () => Map.Lifts.Where(x => x.Type.ToString().Contains("ElB")).ToList().ForEach(x => x.Locked = false));
                                            }
                                        }
                                        foreach (var door in player.Room.Doors)
                                        {
                                            door.Locked = true;
                                            door.Open = false;

                                            Timing.CallDelayed(15f, delegate ()
                                            {
                                                door.Locked = false;
                                            });
                                        }

                                        player.Scp079Controller.Energy -= player.BypassMode ? 0 : BetterScp079.Config.CommandLevels["Gas"][1];
                                        player.Room.Players.Where(x => x.Role != RoleType.Scp079 && x.GodMode != false).ToList().ForEach(x => x.EnableEffect(EffectType.Decontaminating, 15));
                                        float previousIntensity = player.Room.LightIntensity;
                                        player.Room.LightIntensity = (player.Room.LightIntensity / 5) * 4;
                                        Timing.CallDelayed(15f, () => player.Room.LightIntensity = previousIntensity);

                                        _cooldown = !player.BypassMode;
                                        Timing.CallDelayed(BetterScp079.Config.CooldownTime, () => _cooldown = false);
                                        response = "\nSuccessfully!";
                                        return true;
                                    }
                            }
                        }
                    }
                case "teslas":
                    {
                        if (!BetterScp079.Config.CommandsEnabled["Teslas"])
                        {
                            response = "\nCommand not allowed";
                            return false;
                        }

                        if (player.Scp079Controller.Lvl < BetterScp079.Config.CommandLevels["Teslas"][0])
                        {
                            response = "\nInsufficient access";
                            return false;
                        }

                        if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["Teslas"][1] && !player.BypassMode)
                        {
                            response = "\nNot enough mana";
                            return false;
                        }

                        player.Scp079Controller.Energy -= player.BypassMode ? 0 : BetterScp079.Config.CommandLevels["Teslas"][1];
                        Map.Teslas.Where(x => x.Allow079Interact).ToList().ForEach(x => x.Trigger(true));

                        _cooldown = !player.BypassMode;
                        Timing.CallDelayed(BetterScp079.Config.CooldownTime, () => _cooldown = false);
                        response = "\nSuccessfully!";
                        return true;
                    }
                case "grenadefrag" or "frag":
                    {
                        if (!BetterScp079.Config.CommandsEnabled["GrenadeFrag"])
                        {
                            response = "\nCommand not allowed";
                            return false;
                        }

                        if (player.Scp079Controller.Lvl < BetterScp079.Config.CommandLevels["GrenadeFrag"][0])
                        {
                            response = "\nInsufficient access";
                            return false;
                        }

                        if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["GrenadeFrag"][1] && !player.BypassMode)
                        {
                            response = "\nNot enough mana";
                            return false;
                        }

                        player.Scp079Controller.Energy -= player.BypassMode ? 0 : BetterScp079.Config.CommandLevels["GrenadeFrag"][1];
                        EventHandlers.ThrowGrenade(new GrenadeFrag(ItemType.GrenadeHE, player));

                        _cooldown = !player.BypassMode;
                        Timing.CallDelayed(BetterScp079.Config.CooldownTime, () => _cooldown = false);
                        response = "\nSuccessfully!";
                        return true;
                    }
                case "grenadeflash" or "flash":
                    {
                        if (!BetterScp079.Config.CommandsEnabled["GrenadeFlash"])
                        {
                            response = "\nCommand not allowed";
                            return false;
                        }

                        if (player.Scp079Controller.Lvl < BetterScp079.Config.CommandLevels["GrenadeFlash"][0])
                        {
                            response = "\nInsufficient access";
                            return false;
                        }

                        if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["GrenadeFlash"][1] && !player.BypassMode)
                        {
                            response = "\nNot enough mana";
                            return false;
                        }

                        player.Scp079Controller.Energy -= player.BypassMode ? 0 : BetterScp079.Config.CommandLevels["GrenadeFlash"][1];
                        EventHandlers.ThrowGrenade(new GrenadeFlash(ItemType.GrenadeFlash, player));

                        _cooldown = !player.BypassMode;
                        Timing.CallDelayed(BetterScp079.Config.CooldownTime, () => _cooldown = false);
                        response = "\nSuccessfully!";
                        return true;
                    }
                case "blackout":
                    {
                        if (!BetterScp079.Config.CommandsEnabled["Blackout"])
                        {
                            response = "\nCommand not allowed";
                            return false;
                        }

                        if (player.Scp079Controller.Lvl < BetterScp079.Config.CommandLevels["Blackout"][0])
                        {
                            response = "\nInsufficient access";
                            return false;
                        }

                        if (args.Count == 1 || args.ElementAt(1).ToString().ToLower() == "room")
                        {
                            if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["Blackout"][2] && !player.BypassMode)
                            {
                                response = "\nNot enough mana";
                                return false;
                            }

                            player.Scp079Controller.Energy -= player.BypassMode ? 0 : BetterScp079.Config.CommandLevels["Blackout"][2];
                            player.Room.LightsOff(BetterScp079.Config.CommandLevels["Blackout"][2] - 5);
                            player.Room.Tesla.Enable = false;
                            player.Room.Tesla.Allow079Interact = BetterScp079.Config.AllowTeslaInBlackout;
                            Timing.CallDelayed(BetterScp079.Config.CommandLevels["Blackout"][2] - 5, delegate ()
                            {
                                player.Room.Tesla.Enable = true;
                                player.Room.Tesla.Allow079Interact = !BetterScp079.Config.AllowTeslaInBlackout;
                            });

                            _cooldown = !player.BypassMode;
                            Timing.CallDelayed(BetterScp079.Config.CooldownTime, () => _cooldown = false);
                            response = "\nSuccessfully!";
                            return true;
                        }
                        else if (args.ElementAt(1).ToString().ToLower() == "zone")
                        {
                            if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["Blackout"][3] && !player.BypassMode)
                            {
                                response = "\nNot enough mana";
                                return false;
                            }

                            player.Scp079Controller.Energy -= player.BypassMode ? 0 : BetterScp079.Config.CommandLevels["Blackout"][3];
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

                            _cooldown = !player.BypassMode;
                            Timing.CallDelayed(BetterScp079.Config.CooldownTime, () => _cooldown = false);
                            response = "\nSuccessfully!";
                            return true;
                        }
                        else if (args.ElementAt(1).ToString().ToLower() == "facility" || args.ElementAt(1).ToString().ToLower() == "complex")
                        {
                            if (player.Scp079Controller.Lvl < BetterScp079.Config.CommandLevels["Blackout"][1])
                            {
                                response = "\nInsufficient access";
                                return false;
                            }

                            if (player.Scp079Controller.Energy < BetterScp079.Config.CommandLevels["Blackout"][4] && !player.BypassMode)
                            {
                                response = "\nNot enough mana";
                                return false;
                            }

                            player.Scp079Controller.Energy -= player.BypassMode ? 0 : BetterScp079.Config.CommandLevels["Blackout"][4];
                            Lights.TurnOff(BetterScp079.Config.CommandLevels["Blackout"][4] - 15);
                            Map.Teslas.ForEach(x => x.Enable = false);
                            Map.Teslas.ForEach(x => x.Allow079Interact = BetterScp079.Config.AllowTeslaInBlackout);
                            Timing.CallDelayed(BetterScp079.Config.CommandLevels["Blackout"][4] - 15, delegate ()
                            {
                                Map.Teslas.ForEach(x => x.Enable = true);
                                Map.Teslas.ForEach(x => x.Allow079Interact = !BetterScp079.Config.AllowTeslaInBlackout);
                            });

                            _cooldown = !player.BypassMode;
                            Timing.CallDelayed(BetterScp079.Config.CooldownTime, () => _cooldown = false);
                            response = "\nSuccessfully!";
                            return true;
                        }

                        player.Scp079Controller.Energy -= player.BypassMode ? 0 : BetterScp079.Config.CommandLevels["Blackout"][1];
                        player.Room.LightsOff(15);
                        player.Room.Tesla.Enable = false;
                        player.Room.Tesla.Allow079Interact = BetterScp079.Config.AllowTeslaInBlackout;
                        Timing.CallDelayed(15, delegate ()
                        {
                            player.Room.Tesla.Enable = true;
                            player.Room.Tesla.Allow079Interact = !BetterScp079.Config.AllowTeslaInBlackout;
                        });

                        _cooldown = !player.BypassMode;
                        Timing.CallDelayed(BetterScp079.Config.CooldownTime, () => _cooldown = false);
                        response = "\nSuccessfully!";
                        return true;
                    }
                default:
                    {
                        response = "\nUsing: .scp079 [CommandName]";
                        response += string.Format("\nAllowedCommands: \n{0}", string.Join("\n", BetterScp079.Config.CommandsEnabled.Keys.ToList()));
                        return false;
                    }
            }
        }
    }
}
