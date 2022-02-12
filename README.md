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
  - 20
  - 30
  - 50
  Teslas:
  - 2
  - 25
# Can SCP-079 activate Tesla in a room where the blackout by his?
AllowTeslaInBlackout: false
ChangeCameraPowerCost: 0
SpeakerPowerCost: 0
InteractDoorPowerCost: 5
InteractLiftPowerCost: 10
LockDoorPowerCost: 10
ElevatorTeleportPowerCost: 10
InteractTeslaPowerCost: 15
LockdownPowerCost: 25

```
