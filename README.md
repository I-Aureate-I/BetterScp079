<a href="https://github.com/I-Tsukuyomi-I/BetterScp079/releases/latest">
  <img src="https://img.shields.io/github/downloads/I-Tsukuyomi-I/BetterScp079/total" />
</a>

# **BetterScp079**
**Plugin improving SCP-079.**
## **Configs**
```yaml
# Is plugin enabled?
IsEnabled: true
# Is command (key) enabled (value)?
CommandsEnabled:
  RoundTime: true
  TimeUntilRespawn: true
  Blackout: true
  Teslas: true
  GrenadeFrag: true
  GrenadeFlash: true
  Gas: true
# Minimally level and energy for command (Blackout: Level, LevelFacility, EnergyRoom, EnergyZone, EnergyFacility
CommandLevels:
  Blackout:
  - 2
  - 3
  - 30
  - 40
  - 50
  Teslas:
  - 3
  - 50
  GrenadeFrag:
  - 3
  - 35
  GrenadeFlash:
  - 2
  - 25
  Gas:
  - 4
  - 100
  - 150
# Can SCP-079 activate Tesla in a room where the blackout by his?
AllowTeslaInBlackout: false
# Cooldown time in seconds
CooldownTime: 25
# Instant trigger of the tesla in Scp079InteractTeslaEvent?
InteractTeslaInstant: true
InteractDoorPowerCost: 5
LockDoorPowerCost: 5
SpeakerPowerCost: 5
ChangeCameraPowerCost: 10
InteractLiftPowerCost: 10
ElevatorTeleportPowerCost: 20
InteractTeslaPowerCost: 40
LockdownPowerCost: 60

```
## Plugin using [Qurre](https://github.com/Qurre-Team/Qurre-sl)
