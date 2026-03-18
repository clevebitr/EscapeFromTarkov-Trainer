
# EscapeFromTarkov-Trainer

[![Sponsor](https://img.shields.io/badge/sponsor-%E2%9D%A4-lightgrey?logo=github&style=flat-square)](https://github.com/sponsors/sailro)

[zh-cn](https://github.com/clevebitr/EscapeFromTarkov-Trainer/blob/master/README-zh.md)

# This project has been discontinued
> After 5 years of proud service, this project has been discontinued. With the release of version 1.0 and, above all, the switch to IL2CPP technology, internal modding is going to be completely different (and much more complex).
> It has been really interesting to find techniques for modding the game, compiling on the go and allowing features to be added easily. Thanks !

*I'm not responsible for any consequences that result from using this code. BattleState / BattlEye will ban you if you try to use it 'live'. Use it safely offline with [SPT](https://sp-tarkov.com/).*

***TLDR => Use the [Universal Installer](https://github.com/sailro/EscapeFromTarkov-Trainer/releases).*** Default key for in-game GUI is `Right-Alt`.

`master` branch can build against `EFT 0.16.9.40087` (tested with [`SPT Version 4.0.4`](https://hub.sp-tarkov.com/files/file/16-spt/#versions)). If you are looking for another version, see [`branches`](https://github.com/sailro/EscapeFromTarkov-Trainer/branches) and [`releases`](https://github.com/sailro/EscapeFromTarkov-Trainer/releases).

> If you want to compile the code yourself, make sure you cleaned-up your solution properly after upgrading your EFT/sptarkov bits (even removing `bin` and `obj` folders) and check all your references.

> If you are using `SPT`, please make sure you have run the game at least once before compiling/installing the trainer. `SPT` is patching binaries during the first run, and we need to compile against those patched binaries.

> The typical issue when something is out of sync is that the game will freeze at the startup screen with type/tokens errors in `%LOCALAPPDATA%Low\Battlestate Games\EscapeFromTarkov\Player.log`

## Features

| trainer.ini section       | GUI/console  | Description |
|---------------------------|--------------|-------------|
| `Aimbot`                  | `aimbot`     | Aimbot (distance, smoothness, silent aim with speed factor and shot delay, fov radius, fov circle). |
| `AirDrop`                 | `airdrop`    | Triggers an airdrop at the player's location. |
| `Ammunition`              | `ammo`       | Unlimited ammo. |
| `AutomaticGun`            | `autogun`    | Force all guns (even bolt action guns) to use automatic firing mode with customizable fire rate. |
| `Commands`                | `commands`   | Popup window to enable/disable all features (use right-alt or setup your own key in [trainer.ini](#sample-trainerini-configuration-file)). |
| `CrossHair`               | `crosshair`  | Crosshair with customizable size, color, thickness and auto-hide feature when aiming. |
| `Durability`              | `durability` | Maximum durability of items. |
| `Examine`                 | `examine`    | All items already examined. Instant search. |
| `ExfiltrationPoints`      | `exfil`      | Exfiltration points with customizable colors given eligibility, status filter, distance. |
| `FovChanger`              | `fovchanger` | Change Field Of View (FOV). |
| `FreeCamera`              | `camera`     | Free camera with fast mode and teleportation. |
| `Ghost`                   | `ghost`      | Stop bots from seeing you. |
| `Grenades`                | `grenade`    | Grenades outline. |
| `Health`                  | `health`     | Full health, prevent any damage (so even when falling), keep energy and hydration at maximum. |
| `Hits`                    | `hits`       | Hit markers (hit, armor, health with configurable colors). |
| `Hud`                     | `hud`        | HUD (compass, ammo left in chamber / magazine, fire mode, coordinates). |
| `Interact`                | `interact`   | Change distance for loot/door interaction. |
| `LootableContainers`      | `stash`      | Hidden/special stashes like buried barrels, ground caches, air drops or corpses. |
| `LootItems`               | `loot`       | List all lootable items and track any item by name or rarity or in-game wishlist in raid (even in containers and corpses). |
| `Map`                     | `map`        | Full screen map with radar esp. |
| `Mortar`                  | `mortar`     | Triggers a mortar strike at the player's location. |
| `NightVision`             | `night`      | Night vision. |
| `NoCollision`             | `nocoll`     | No physical collisions, making you immune to bullets, grenades and barbed wires. |
| `NoFlash`                 | `noflash`    | No persistent flash or eye-burn effect after a flash grenade. |
| `NoMalfunctions`          | `nomal`      | No weapon malfunctions: no misfires or failures to eject or feed. No jammed bolts or overheating. |
| `NoRecoil`                | `norecoil`   | No recoil. |
| `NoSway`                  | `nosway`     | No sway. |
| `NoVisor`                 | `novisor`    | No visor, so even when using a face shield-visor you won't see it. |
| `Players`                 | `players`    | Players (you'll see Bear/Boss/Cultist/Scav/Usec with configurable colors through walls). Charms, boxes, info (weapon and health), skeletons and distance. |
| `Quests`                  | `quest`      | Locations for taking/placing quest items. Only items related to your started quests are displayed. |
| `QuickThrow`              | `quickthrow` | Quick-throw grenades. |
| `Radar`                   | `radar`      | 2D radar. |
| `Skills`                  | `skills`     | All skills to Elite level (51) and all weapons mastering to level 3. |
| `Speed`                   | `speed`      | Speed boost to be able to go through walls/objects, or to move faster. Be careful to not kill yourself. |
| `Stamina`                 | `stamina`    | Unlimited stamina. |
| `ThermalVision`           | `thermal`    | Thermal vision. |
| `Train`                   | `train`      | Summon train on compatible maps like Reserve or Lighthouse. |
| `WallShoot`               | `wallshoot`  | Shoot through wall/helmet/vest/material with maximum penetration and minimal deviation/ricochet. |
| `Weather`                 | `weather`    | Clear weather. |
| `WorldInteractiveObjects` | `opener`     | Door/Keycard reader/Car unlocker. |

You can Load/Save all settings using the `console` or the `GUI`.

![Players](https://user-images.githubusercontent.com/638167/222186879-a88a267e-16ba-4532-85ec-8cb385737947.png)
![Radar](https://user-images.githubusercontent.com/638167/222524208-589dc7ff-f053-4b0c-902b-49fa8d1f7ddd.png)
![Map](https://user-images.githubusercontent.com/769465/224330696-d09960a2-8940-4980-8489-0533b44534f9.png)
![Exfils](https://user-images.githubusercontent.com/638167/135586735-143ab160-ca20-4ec9-8ad4-9ce7bde58295.png)
![Loot](https://user-images.githubusercontent.com/638167/135587083-938a3d9b-2082-4231-9fa8-e7807ad4a3d1.png)
![Quests](https://user-images.githubusercontent.com/638167/121975175-d8d91c00-cd35-11eb-86cd-6b49360fe370.png)
![Stashes](https://user-images.githubusercontent.com/638167/135586933-4cf57740-aff2-47c8-9cec-94e2eb062dd0.png)
![Track](https://user-images.githubusercontent.com/638167/222189119-f25413a7-511b-43cf-b6d8-a57320347034.png)
![NightVision](https://user-images.githubusercontent.com/638167/135586268-c175e999-a60d-40db-9960-06cdf5fe27d7.png)
![Popup](https://user-images.githubusercontent.com/638167/222188079-3ddb81e2-fb4c-446d-8716-5f54a40ad01b.png)

## Easy and automatic installation

Simply use the [Universal Installer](https://github.com/sailro/EscapeFromTarkov-Trainer/releases).

## Configuration

![console](https://user-images.githubusercontent.com/638167/149630825-7d76b102-0836-4eb9-a27f-d33fb519452f.png)

This trainer hooks into the command system, so you can easily setup features using the built-in console:

| Command    | Values              | Default | Description                          |
|------------|---------------------|---------|--------------------------------------|
| ammo       | `on` or `off`       | `off`   | Enable/Disable unlimited ammo        |
| autogun    | `on` or `off`       | `off`   | Enable/Disable automatic gun mode    |
| crosshair  | `on` or `off`       | `off`   | Show/Hide crosshair                  |
| dump       |                     |         | Dump game state for analysis         |
| durability | `on` or `off`       | `off`   | Enable/Disable maximum durability    |
| examine    | `on` or `off`       | `off`   | Enable/Disable all item examined     |
| exfil      | `on` or `off`       | `on`    | Show/Hide exfiltration points        |
| fovchanger | `on` or `off`       | `off`   | Change FOV value                     |
| ghost      | `on` or `off`       | `off`   | Enable/Disable ghost mode            |
| grenade    | `on` or `off`       | `off`   | Show/Hide grenades                   |
| health     | `on` or `off`       | `off`   | Enable/Disable full health           |
| hits       | `on` or `off`       | `off`   | Show/Hide hit markers                |
| hud        | `on` or `off`       | `on`    | Show/Hide hud                        |
| interact   | `on` or `off`       | `off`   | Enable/Disable interaction changes   |
| list       | `[name]` or `*`     |         | List lootable items                  |
| listr      | `[name]` or `*`     |         | List only rare lootable items        |
| listsr     | `[name]` or `*`     |         | List only super rare lootable items  |
| load       |                     |         | Load settings from `trainer.ini`     |
| loadtl     | `[filename]`        |         | Load current tracklist from file     |
| loot       | `on` or `off`       |         | Show/Hide tracked items              |
| night      | `on` or `off`       | `off`   | Enable/Disable night vision          |
| nocoll     | `on` or `off`       | `off`   | Disable/Enable physical collisions   |
| noflash    | `on` or `off`       | `off`   | Disable/Enable flash/eyeburn effects |
| nomal      | `on` or `off`       | `off`   | Disable/Enable weapon malfunctions   |
| norecoil   | `on` or `off`       | `off`   | Disable/Enable recoil                |
| nosway     | `on` or `off`       | `off`   | Disable/Enable sway                  |
| novisor    | `on` or `off`       | `off`   | Disable/Enable visor                 |
| players    | `on` or `off`       | `on`    | Show/hide players                    |
| quest      | `on` or `off`       | `off`   | Show/Hide quest POI                  |
| radar      | `on` or `off`       | `off`   | Show/Hide radar                      |
| save       |                     |         | Save settings to `trainer.ini`       |
| savetl     | `[filename]`        |         | Save current tracklist to file       |
| spawn      | `[name]`            |         | Spawn object in front of player      |
| spawnbot   | `[name],[player],[coordinate]` or `*`     |         | Spawn a bot, ex `spawnbot bossKilla,[player],[100,1,100]` |
| spawnhi    |                     |         | Spawn required hideout items         |
| spawnqi    |                     |         | Spawn items-to-find in active quests |
| stamina    | `on` or `off`       | `off`   | Enable/Disable unlimited stamina     |
| stash      | `on` or `off`       | `off`   | Show/Hide stashes                    |
| status     |                     |         | Show status of all features          |
| template   | `[name]`            |         | Search for templates by short/name   |
| thermal    | `on` or `off`       | `off`   | Enable/Disable thermal vision        |
| track      | `[name]` or `*`     |         | Track all items matching `name`      |
| track      | `[name]` `<color>`  |         | Ex: track `roler` `red`              |
| track      | `[name]` `<rgba>`   |         | Ex: track `roler` `[1,1,1,0.5]`      |
| trackr     | same as `track`     |         | Track rare items only                |
| tracksr    | same as `track`     |         | Track super rare items only          |
| tracklist  |                     |         | Show tracked items                   |
| untrack    | `[name]` or `*`     |         | Untrack a `name` or `*` for all      |
| wallshoot  | `on` or `off`       | `on`    | Enable/Disable shoot through walls   |
| buff       | `[buffname] [bodypart] [parameter]`  |    | Apply a buff to player, ex: `buff pain`, `buff fracture LeftArm`, `buff stim 1` |

## Buff Command

The `buff` command is used to apply various effects to the player character in the game.

### Usage
- `buff help` - Show help information
- `buff list` - Show all available effects
- `buff <effect name>` - Apply the specified effect
- `buff <effect name> <body part>` - Apply the specified effect to a specific body part
- `buff <effect name> <parameter>` - Apply the specified effect with a parameter
- `buff <effect name> <body part> <parameter>` - Apply the specified effect to a specific body part with a parameter

### Available Effect Categories

1. **Bleeding**
   - `lightbleed`, `lightbleeding` - Light bleeding
   - `heavybleed`, `heavybleeding` - Heavy bleeding
   - `bleed` - Light bleeding

2. **Injuries**
   - `fracture` - Fracture

3. **Pain/Stun**
   - `pain` - Pain effect
   - `tremor` - Tremor effect
   - `stun` - Stun effect
   - `contusion` - Contusion effect
   - `disorientation` - Disorientation effect

4. **Toxins**
   - `intoxication`, `poison` - Intoxication effect
   - `lethalintoxication` - Lethal intoxication effect
   - `radexposure`, `radiation` - Radiation exposure effect

5. **Medicines**
   - `painkiller`, `stop`, `pills` - Painkiller effect
   - `stim`, `stimulator` - Stimulator effect
   - `healthboost` - Health boost effect
   - `regeneration` - Regeneration effect

6. **Special**
   - `misfire` - Weapon misfire effect
   - `staminazero` - Stamina zero effect
   - `fatigue` - Fatigue effect
   - `berserk` - Berserk effect
   - `paniceffect` - Panic effect
   - `sandingscreen` - Sanding screen effect
   - `tunnelvision` - Tunnel vision effect
   - `dehydration`, `dehydrate`, `thirst` - Dehydration effect

7. **Visual**
   - `flash` - Flash effect

8. **Weight**
   - `encumbered` - Encumbered status
   - `overencumbered` - Over encumbered status

9. **Infection**
   - `zombieinfection` - Zombie infection effect

10. **Cleansing**
    - `remove` - Remove negative effects
    - `removeall` - Remove all effects
    - `fullheal` - Full health regeneration
    - `restorebodypart` - Restore body part

### Body Parts

Available body parts:
- `Head` - Head
- `Thorax` - Thorax
- `Stomach` - Stomach
- `LeftArm` - Left Arm
- `RightArm` - Right Arm
- `LeftLeg` - Left Leg
- `RightLeg` - Right Leg
- `Common` - Full body (default)

### Examples

- `buff pain` - Apply pain effect to full body
- `buff fracture LeftArm` - Apply fracture effect to left arm
- `buff stim 1` - Apply stimulator effect with index 1
- `buff fullheal` - Restore full health
- `buff removeall` - Remove all effects

### Notes

- Some effects require parameters, such as `stim` which can specify different stimulator types
- Effect names are case-insensitive
- If the specified effect does not exist, the command will try to find and apply the corresponding effect type through reflection

## Translations

This trainer comes in English but we also provide French, Japanese and Chinese simplified versions. You can use the [Universal Installer](https://github.com/sailro/EscapeFromTarkov-Trainer/releases) to specify your language, using `./Installer -l zh-cn` for Chinese simplified for instance.

You can also tweak or add your own language by having a look [here](https://github.com/sailro/EscapeFromTarkov-Trainer/issues/541).
