using Footprinting;
using InventorySystem.Items.Pickups;
using Mirror;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
using Qurre.API.Events;
using Qurre.API.Objects;
using UnityEngine;

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

        internal static void ThrowGrenade(GrenadeFlash grenade)
        {
            grenade.FuseTime = 1f;
            grenade.Base._destroyTime = Time.timeSinceLevelLoad + grenade.Base._postThrownAnimationTime;
            grenade.Base._alreadyFired = true;
            Respawning.GameplayTickets.Singleton.HandleItemTickets(grenade.Base);
            var thrownProjectile = Object.Instantiate(grenade.Base.Projectile, grenade.Owner.Scp079Controller.Camera.GameObject.transform.position, grenade.Owner.Scp079Controller.Camera.GameObject.transform.rotation);

            PickupSyncInfo pickupSyncInfo = new()
            {
                ItemId = grenade.Base.ItemTypeId,
                Locked = !grenade.Base._repickupable,
                Serial = grenade.Base.ItemSerial,
                Weight = grenade.Base.Weight,
                Position = thrownProjectile.transform.position,
                Rotation = new LowPrecisionQuaternion(thrownProjectile.transform.rotation)
            };

            thrownProjectile.NetworkInfo = pickupSyncInfo;
            thrownProjectile.PreviousOwner = new Footprint(grenade.Base.Owner);
            NetworkServer.Spawn(thrownProjectile.gameObject, (NetworkConnection)null);
            thrownProjectile.InfoReceived(default, pickupSyncInfo);

            if (thrownProjectile.TryGetComponent(out Rigidbody rb))
            {
                grenade.Base.PropelBody(rb, grenade.Base.WeakThrowSettings.StartTorque, Vector3.one, grenade.Base.WeakThrowSettings.StartVelocity, grenade.Base.WeakThrowSettings.UpwardsFactor);
            }

            thrownProjectile.ServerActivate();
        }

        internal static void ThrowGrenade(GrenadeFrag grenade)
        {
            grenade.FuseTime = 1f;
            grenade.Base._destroyTime = Time.timeSinceLevelLoad + grenade.Base._postThrownAnimationTime;
            grenade.Base._alreadyFired = true;
            Respawning.GameplayTickets.Singleton.HandleItemTickets(grenade.Base);
            var thrownProjectile = Object.Instantiate(grenade.Base.Projectile, grenade.Owner.Scp079Controller.Camera.GameObject.transform.position, grenade.Owner.Scp079Controller.Camera.GameObject.transform.rotation);

            PickupSyncInfo pickupSyncInfo = new()
            {
                ItemId = grenade.Base.ItemTypeId,
                Locked = !grenade.Base._repickupable,
                Serial = grenade.Base.ItemSerial,
                Weight = grenade.Base.Weight,
                Position = thrownProjectile.transform.position,
                Rotation = new LowPrecisionQuaternion(thrownProjectile.transform.rotation)
            };

            thrownProjectile.NetworkInfo = pickupSyncInfo;
            thrownProjectile.PreviousOwner = new Footprint(grenade.Base.Owner);
            NetworkServer.Spawn(thrownProjectile.gameObject, (NetworkConnection)null);
            thrownProjectile.InfoReceived(default, pickupSyncInfo);

            if (thrownProjectile.TryGetComponent(out Rigidbody rb))
            {
                grenade.Base.PropelBody(rb, grenade.Base.WeakThrowSettings.StartTorque, Vector3.one, grenade.Base.WeakThrowSettings.StartVelocity, grenade.Base.WeakThrowSettings.UpwardsFactor);
            }

            thrownProjectile.ServerActivate();
        }
    }
}
