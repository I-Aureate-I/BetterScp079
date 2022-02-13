using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterScp079
{
    public static class Extensions
    {
        public static string GetAllowedCommand(this List<string> commands)
        {
            var result = commands.Where(x => BetterScp079.Config.CommandsEnabled[x]);

            result.ToList().ForEach(x => x = "." + x.GetCommandByName());

            return string.Join("\n", result);
        }

        public static string GetCommandByName(this string name)
        {
            switch (name)
            {
                case "RoundTime":
                    return "time - no parameters";
                case "TimeUntilRespawn":
                    return "respawntime - no parameters";
                case "Blackout":
                    return string.Format("blackout - level {0} , room {2} , zone {3} , facility level {1} energy {4}", BetterScp079.Config.CommandLevels["Blackout"][0], BetterScp079.Config.CommandLevels["Blackout"][1], BetterScp079.Config.CommandLevels["Blackout"][2], BetterScp079.Config.CommandLevels["Blackout"][3], BetterScp079.Config.CommandLevels["Blackout"][4]);
                case "Teslas":
                    return string.Format("teslas - level {0} energy {1}", BetterScp079.Config.CommandLevels["Teslas"][0], BetterScp079.Config.CommandLevels["Teslas"][1]);
                case "":
                default: 
                    return "null";
            }
        }
    }
}
