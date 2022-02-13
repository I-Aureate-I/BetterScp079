using System.Collections.Generic;
using System.ComponentModel;
using IConfig = Qurre.API.Addons.IConfig;

namespace BetterScp079
{
    public class Config : IConfig
    {
        [Description("Plugin Name")]
        public string Name { get; set; } = "BetterScp079";

        [Description("Is plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Is command (key) enabled (value)?")]
        public Dictionary<string, bool> CommandsEnabled { get; set; } = new()
        {
            { "RoundTime", true },
            { "TimeUntilRespawn", true },
            { "Blackout", true },
            { "Teslas", true },
            { "GrenadeFrag", true },
            { "GrenadeFlash", true },
            { "Gas", true }
        };

        [Description("Minimally level and energy for command (Blackout: Level, LevelFacility, EnergyRoom, EnergyZone, EnergyFacility")]
        public Dictionary<string, List<int>> CommandLevels { get; set; } = new()
        {
            { "Blackout", new(5) { 2, 3, 30, 40, 50 } },
            { "Teslas", new(2) { 3, 50 } },
            { "GrenadeFrag", new(2) { 3, 35 } },
            { "GrenadeFlash", new(2) { 2, 25 } },
            { "Gas", new(3) { 4, 100, 150 } }
        };

        [Description("Can SCP-079 activate Tesla in a room where the blackout by his?")]
        public bool AllowTeslaInBlackout { get; set; } = false;

        [Description("Cooldown time in seconds")]
        public float CooldownTime { get; set; } = 25;

        [Description("Instant trigger of the tesla in Scp079InteractTeslaEvent?")]
        public bool InteractTeslaInstant { get; set; } = true;

        public int InteractDoorPowerCost { get; set; } = 5;

        public int LockDoorPowerCost { get; set; } = 5;

        public int SpeakerPowerCost { get; set; } = 5;

        public int ChangeCameraPowerCost { get; set; } = 10;

        public int InteractLiftPowerCost { get; set; } = 10;

        public int ElevatorTeleportPowerCost { get; set; } = 20;

        public int InteractTeslaPowerCost { get; set; } = 40;

        public int LockdownPowerCost { get; set; } = 60;
    }
}
