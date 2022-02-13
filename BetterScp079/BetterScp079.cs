using Qurre;
using System;
using Events = Qurre.Events.Scp079;

namespace BetterScp079
{
    public class BetterScp079 : Plugin
    {
        public override string Name => "BetterScp079";

        public override string Developer => "Tsukuyomi#2884";

        public override Version Version => new(1, 0, 0);

        public override Version NeededQurreVersion => new(1, 11, 13);

        public static new Config Config { get; private set; }

        public static EventHandlers Handlers { get; private set; }

        public override void Enable()
        {
            Config = new Config();
            Handlers = new EventHandlers();

            CustomConfigs.Add(Config);

            Events.Speaker += Handlers.Speaker;
            Events.LockDoor += Handlers.LockDoor;
            Events.Lockdown += Handlers.Lockdown;
            Events.ElevatorTeleport += Handlers.ElevatorTeleport;
            Events.InteractTesla += Handlers.InteractTesla;
            Events.InteractDoor += Handlers.InteractDoor;
            Events.InteractLift += Handlers.InteractLift;
            Events.ChangeCamera += Handlers.ChangeCamera;
        }

        public override void Disable()
        {
            CustomConfigs.Remove(Config);

            Events.Speaker -= Handlers.Speaker;
            Events.LockDoor -= Handlers.LockDoor;
            Events.Lockdown -= Handlers.Lockdown;
            Events.ElevatorTeleport -= Handlers.ElevatorTeleport;
            Events.InteractTesla -= Handlers.InteractTesla;
            Events.InteractDoor -= Handlers.InteractDoor;
            Events.InteractLift -= Handlers.InteractLift;
            Events.ChangeCamera -= Handlers.ChangeCamera;

            Config = null;
            Handlers = null;
        }

        public override void Reload()
        {
            Disable();
            Enable();
            base.Reload();
        }
    }
}
