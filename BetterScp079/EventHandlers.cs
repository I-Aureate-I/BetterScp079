using Qurre.API.Events;
using Qurre.API.Objects;
using Qurre.API.Controllers;

namespace BetterScp079
{
    public class EventHandlers
    {
        public void Speaker(Scp079SpeakerEvent ev) => ev.PowerCost = BetterScp079.Config.SpeakerPowerCost;

        public void InteractLift(Scp079InteractLiftEvent ev) => ev.PowerCost = BetterScp079.Config.InteractLiftPowerCost;

        public void InteractDoor(Scp079InteractDoorEvent ev) => ev.PowerCost = BetterScp079.Config.InteractDoorPowerCost;

        public void LockDoor(Scp079LockDoorEvent ev) => ev.PowerCost = BetterScp079.Config.LockDoorPowerCost;

        public void ElevatorTeleport(Scp079ElevatorTeleportEvent ev)
        {
            if (ev.Scp079.Zone == ZoneType.Heavy)
                ev.Allowed = !Decontamination.InProgress;

            ev.PowerCost = BetterScp079.Config.ElevatorTeleportPowerCost;
        }

        public void ChangeCamera(ChangeCameraEvent ev) => ev.PowerCost = BetterScp079.Config.ChangeCameraPowerCost;

        public void Lockdown(Scp079LockdownEvent ev)
        {
            ev.Allowed = !ev.Room.IsLightsOff;
            ev.PowerCost = BetterScp079.Config.LockdownPowerCost;
        }

        public void InteractTesla(Scp079InteractTeslaEvent ev)
        { 
            ev.PowerCost = BetterScp079.Config.InteractTeslaPowerCost;
            ev.Instant = BetterScp079.Config.InteractTeslaInstant;
        }
    }
}
