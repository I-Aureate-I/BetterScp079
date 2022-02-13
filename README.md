# **BetterScp079**
**Plugin for improving SCP-079.**
## **Configs**
```yaml
# Plugin Name
Name: BetterScp079
# Is plugin enabled?
IsEnabled: true
# Is command (key) enabled (value)?
CommandsEnabled:
  RoundTime: true
  TimeUntilRespawn: true
  Blackout: true
  Teslas: true
# Minimally level and energy for command (Blackout: Level, LevelFacility, EnergyRoom, EnergyZone, EnergyFacility
CommandLevels:
  Blackout:
  - 2
  - 3
  - 30
  - 40
  - 50
  Teslas:
  - 2
  - 40
# Can SCP-079 activate Tesla in a room where the blackout by his?
AllowTeslaInBlackout: false
# Instant trigger of the tesla in Scp079InteractTeslaEvent?
InteractTeslaInstant: true
InteractDoorPowerCost: 5
LockDoorPowerCost: 5
SpeakerPowerCost: 10
ChangeCameraPowerCost: 10
InteractLiftPowerCost: 10
ElevatorTeleportPowerCost: 30
InteractTeslaPowerCost: 45
LockdownPowerCost: 60
```
