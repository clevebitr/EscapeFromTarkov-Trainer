# EscapeFromTarkov-Trainer

[![Sponsor](https://img.shields.io/badge/sponsor-%E2%9D%A4-lightgrey?logo=github&style=flat-square)](https://github.com/sponsors/sailro)

# 该项目已停止维护
> 在5年的服务后，该项目已停止维护。随着版本1.0的发布，尤其是切换到IL2CPP技术后，内部修改将变得完全不同（且复杂得多）。
> 寻找游戏修改技术、动态编译和轻松添加功能的过程非常有趣。谢谢！

*我不对使用此代码产生的任何后果负责。如果您尝试在" live"环境中使用它，BattleState / BattlEye会封禁您。请在离线模式下安全使用[SPT](https://sp-tarkov.com/)。*

***TLDR => 使用[通用安装程序](https://github.com/sailro/EscapeFromTarkov-Trainer/releases)。*** 游戏内GUI的默认键是`Right-Alt`。

`master`分支可以针对`EFT 0.16.9.40087`构建（已使用[`SPT Version 4.0.4`](https://hub.sp-tarkov.com/files/file/16-spt/#versions)测试）。如果您正在寻找其他版本，请查看[`branches`](https://github.com/sailro/EscapeFromTarkov-Trainer/branches)和[`releases`](https://github.com/sailro/EscapeFromTarkov-Trainer/releases)。

> 如果您想自己编译代码，请确保在升级EFT/sptarkov组件后正确清理解决方案（甚至删除`bin`和`obj`文件夹）并检查所有引用。

> 如果您使用`SPT`，请确保在编译/安装训练器之前至少运行游戏一次。`SPT`在首次运行时会修补二进制文件，我们需要针对这些修补后的二进制文件进行编译。

> 不同步的典型问题是游戏会在启动屏幕上冻结，并在`%LOCALAPPDATA%Low\Battlestate Games\EscapeFromTarkov\Player.log`中出现类型/令牌错误。

## 功能

| trainer.ini 部分          | GUI/控制台  | 描述                                                                 |
|---------------------------|-------------|----------------------------------------------------------------------|
| `Aimbot`                  | `aimbot`    | 自瞄（距离、平滑度、带有速度因子和射击延迟的静默瞄准、视野半径、视野圆圈）。 |
| `AirDrop`                 | `airdrop`   | 在玩家位置触发空投。                                                 |
| `Ammunition`              | `ammo`      | 无限弹药。                                                           |
| `AutomaticGun`            | `autogun`   | 强制所有枪支（甚至 bolt action 枪支）使用自动射击模式，可自定义射速。 |
| `Commands`                | `commands`  | 弹出窗口以启用/禁用所有功能（使用右Alt键或在[trainer.ini](#sample-trainerini-configuration-file)中设置自己的键）。 |
| `CrossHair`               | `crosshair` | 十字准星，可自定义大小、颜色、粗细和瞄准自动隐藏功能。               |
| `Durability`              | `durability`| 物品的最大耐久度。                                                   |
| `Examine`                 | `examine`   | 所有物品已检查。即时搜索。                                           |
| `ExfiltrationPoints`      | `exfil`     | 撤离点，根据资格、状态过滤器、距离提供可自定义颜色。                 |
| `FovChanger`              | `fovchanger`| 更改视野（FOV）。                                                    |
| `FreeCamera`              | `camera`    | 自由相机，带有快速模式和 teleportation。                             |
| `Ghost`                   | `ghost`     | 阻止机器人看到你。                                                   |
| `Grenades`                | `grenade`   | 手榴弹轮廓。                                                         |
| `Health`                  | `health`    | 满血，防止任何伤害（即使在跌倒时），保持能量和水分最大化。           |
| `Hits`                    | `hits`      | 击中标记（击中、装甲、健康，带有可配置的颜色）。                     |
| `Hud`                     | `hud`       | HUD（指南针、弹膛/弹匣中剩余的弹药、射击模式、坐标）。               |
| `Interact`                | `interact`  | 更改 loot/door 交互的距离。                                          |
| `LootableContainers`      | `stash`     | 隐藏/特殊储藏处，如埋在地下的桶、地面缓存、空投或尸体。              |
| `LootItems`               | `loot`      | 列出所有可掠夺物品，并按名称或稀有度或游戏内愿望清单在 raid 中跟踪任何物品（甚至在容器和尸体中）。 |
| `Map`                     | `map`       | 带雷达 esp 的全屏地图。                                              |
| `Mortar`                  | `mortar`    | 在玩家位置触发迫击炮打击。                                           |
| `NightVision`             | `night`     | 夜视。                                                               |
| `NoCollision`             | `nocoll`    | 无物理碰撞，使你对子弹、手榴弹和铁丝网免疫。                         |
| `NoFlash`                 | `noflash`   | 闪光手榴弹后无持续闪光或眼部烧伤效果。                               |
| `NoMalfunctions`          | `nomal`     | 无武器故障：无卡壳或退壳/供弹失败。无卡住的螺栓或过热。             |
| `NoRecoil`                | `norecoil`  | 无后坐力。                                                           |
| `NoSway`                  | `nosway`    | 无晃动。                                                             |
| `NoVisor`                 | `novisor`   | 无visor，因此即使使用面部护罩-visor也不会看到它。                    |
| `Players`                 | `players`   | 玩家（你会通过墙壁看到 Bear/Boss/Cultist/Scav/Usec，带有可配置的颜色）。饰品、箱子、信息（武器和健康）、骨骼和距离。 |
| `Quests`                  | `quest`     | 放置/获取任务物品的位置。仅显示与已开始任务相关的物品。              |
| `QuickThrow`              | `quickthrow`| 快速投掷手榴弹。                                                     |
| `Radar`                   | `radar`     | 2D雷达。                                                            |
| `Skills`                  | `skills`    | 所有技能达到精英级别（51），所有武器掌握达到3级。                   |
| `Speed`                   | `speed`     | 速度提升，能够穿过墙壁/物体，或移动更快。注意不要自杀。              |
| `Stamina`                 | `stamina`   | 无限耐力。                                                           |
| `ThermalVision`           | `thermal`   | 热成像视觉。                                                         |
| `Train`                   | `train`     | 在兼容地图（如 Reserve 或 Lighthouse）上召唤火车。                   |
| `WallShoot`               | `wallshoot` | 穿墙/头盔/背心/材料射击，最大穿透力和最小偏差/跳弹。                |
| `Weather`                 | `weather`   | 晴朗天气。                                                           |
| `WorldInteractiveObjects` | `opener`    | 门/钥匙卡读卡器/汽车解锁器。                                         |

您可以使用`console`或`GUI`加载/保存所有设置。

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

## 简单自动安装

只需使用[通用安装程序](https://github.com/sailro/EscapeFromTarkov-Trainer/releases)。

## 配置

![console](https://user-images.githubusercontent.com/638167/149630825-7d76b102-0836-4eb9-a27f-d33fb519452f.png)

该训练器挂钩到命令系统，因此您可以使用内置控制台轻松设置功能：

| 命令       | 值                  | 默认值  | 描述                                 |
|------------|---------------------|---------|--------------------------------------|
| ammo       | `on` 或 `off`       | `off`   | 启用/禁用无限弹药                     |
| autogun    | `on` 或 `off`       | `off`   | 启用/禁用自动枪支模式                 |
| crosshair  | `on` 或 `off`       | `off`   | 显示/隐藏十字准星                     |
| dump       |                     |         | 转储游戏状态以进行分析                |
| durability | `on` 或 `off`       | `off`   | 启用/禁用最大耐久度                   |
| examine    | `on` 或 `off`       | `off`   | 启用/禁用所有物品已检查               |
| exfil      | `on` 或 `off`       | `on`    | 显示/隐藏撤离点                       |
| fovchanger | `on` 或 `off`       | `off`   | 更改 FOV 值                          |
| ghost      | `on` 或 `off`       | `off`   | 启用/禁用幽灵模式                     |
| grenade    | `on` 或 `off`       | `off`   | 显示/隐藏手榴弹                       |
| health     | `on` 或 `off`       | `off`   | 启用/禁用满血                         |
| hits       | `on` 或 `off`       | `off`   | 显示/隐藏击中标记                     |
| hud        | `on` 或 `off`       | `on`    | 显示/隐藏 HUD                        |
| interact   | `on` 或 `off`       | `off`   | 启用/禁用交互更改                     |
| list       | `[name]` 或 `*`     |         | 列出可掠夺物品                        |
| listr      | `[name]` 或 `*`     |         | 仅列出稀有可掠夺物品                  |
| listsr     | `[name]` 或 `*`     |         | 仅列出超级稀有可掠夺物品              |
| load       |                     |         | 从 `trainer.ini` 加载设置             |
| loadtl     | `[filename]`        |         | 从文件加载当前跟踪列表                |
| loot       | `on` 或 `off`       |         | 显示/隐藏跟踪的物品                   |
| night      | `on` 或 `off`       | `off`   | 启用/禁用夜视                         |
| nocoll     | `on` 或 `off`       | `off`   | 禁用/启用物理碰撞                     |
| noflash    | `on` 或 `off`       | `off`   | 禁用/启用闪光/眼部烧伤效果            |
| nomal      | `on` 或 `off`       | `off`   | 禁用/启用武器故障                     |
| norecoil   | `on` 或 `off`       | `off`   | 禁用/启用后坐力                       |
| nosway     | `on` 或 `off`       | `off`   | 禁用/启用晃动                         |
| novisor    | `on` 或 `off`       | `off`   | 禁用/启用 visor                       |
| players    | `on` 或 `off`       | `on`    | 显示/隐藏玩家                         |
| quest      | `on` 或 `off`       | `off`   | 显示/隐藏任务 POI                     |
| radar      | `on` 或 `off`       | `off`   | 显示/隐藏雷达                         |
| save       |                     |         | 将设置保存到 `trainer.ini`            |
| savetl     | `[filename]`        |         | 将当前跟踪列表保存到文件              |
| spawn      | `[name]`            |         | 在玩家面前生成物体                    |
| spawnbot   | `[name],[player],[coordinate]` 或 `*` | | 生成机器人，例如 `spawnbot bossKilla,[player],[100,1,100]` |
| spawnhi    |                     |         | 生成所需的藏身处物品                  |
| spawnqi    |                     |         | 生成活跃任务中要查找的物品            |
| stamina    | `on` 或 `off`       | `off`   | 启用/禁用无限耐力                     |
| stash      | `on` 或 `off`       | `off`   | 显示/隐藏储藏处                       |
| status     |                     |         | 显示所有功能的状态                    |
| template   | `[name]`            |         | 按简称/名称搜索模板                   |
| thermal    | `on` 或 `off`       | `off`   | 启用/禁用热成像视觉                   |
| track      | `[name]` 或 `*`     |         | 跟踪所有匹配 `name` 的物品            |
| track      | `[name]` `<color>`  |         | 例如：track `roler` `red`             |
| track      | `[name]` `<rgba>`   |         | 例如：track `roler` `[1,1,1,0.5]`     |
| trackr     | 与 `track` 相同     |         | 仅跟踪稀有物品                        |
| tracksr    | 与 `track` 相同     |         | 仅跟踪超级稀有物品                    |
| tracklist  |                     |         | 显示跟踪的物品                        |
| untrack    | `[name]` 或 `*`     |         | 取消跟踪 `name` 或 `*` 所有物品       |
| wallshoot  | `on` 或 `off`       | `on`    | 启用/禁用穿墙射击                     |
| buff       | `[buffname] [bodypart] [parameter]` | | 对玩家应用buff，例如：`buff pain`、`buff fracture LeftArm`、`buff stim 1` |

## Buff 命令

`buff` 命令用于在游戏中向玩家角色应用各种效果。

### 用法
- `buff help` - 显示帮助信息
- `buff list` - 显示所有可用的效果
- `buff <效果名称>` - 应用指定的效果
- `buff <效果名称> <身体部位>` - 向特定身体部位应用指定的效果
- `buff <效果名称> <参数>` - 应用指定的效果并设置参数
- `buff <效果名称> <身体部位> <参数>` - 向特定身体部位应用指定的效果并设置参数

### 可用的效果类别

1. **出血**
   - `lightbleed`, `lightbleeding` - 轻微出血
   - `heavybleed`, `heavybleeding` - 严重出血
   - `bleed` - 轻微出血

2. **伤害**
   - `fracture` - 骨折

3. **疼痛/眩晕**
   - `pain` - 疼痛效果
   - `tremor` - 震颤效果
   - `stun` - 眩晕效果
   - `contusion` - 挫伤效果
   - `disorientation` - 迷失方向效果

4. **毒素**
   - `intoxication`, `poison` - 中毒效果
   - `lethalintoxication` - 致命中毒效果
   - `radexposure`, `radiation` - 辐射暴露效果

5. **药物**
   - `painkiller`, `stop`, `pills` - 止痛药效果
   - `stim`, `stimulator` - 兴奋剂效果
   - `healthboost` - 健康提升效果
   - `regeneration` - 再生效果

6. **特殊效果**
   - `misfire` - 武器卡壳效果
   - `staminazero` - 体力耗尽效果
   - `fatigue` - 疲劳效果
   - `berserk` - Berserk 效果
   - `paniceffect` - 恐慌效果
   - `sandingscreen` - 沙尘屏幕效果
   - `tunnelvision` - 隧道视觉效果
   - `dehydration`, `dehydrate`, `thirst` - 脱水效果

7. **视觉效果**
   - `flash` - 闪光效果

8. **负重状态**
   - `encumbered` - 负重状态
   - `overencumbered` - 超重状态

9. **感染**
   - `zombieinfection` - 僵尸感染效果

10. **清除效果**
    - `remove` - 移除负面效果
    - `removeall` - 移除所有效果
    - `fullheal` - 完全恢复健康
    - `restorebodypart` - 恢复身体部位

### 身体部位

可用的身体部位：
- `Head` - 头部
- `Thorax` - 胸部
- `Stomach` - 腹部
- `LeftArm` - 左臂
- `RightArm` - 右臂
- `LeftLeg` - 左腿
- `RightLeg` - 右腿
- `Common` - 全身（默认）

### 示例

- `buff pain` - 向全身应用疼痛效果
- `buff fracture LeftArm` - 向左臂应用骨折效果
- `buff stim 1` - 应用兴奋剂效果，使用索引 1
- `buff fullheal` - 完全恢复健康
- `buff removeall` - 移除所有效果

### 注意事项

- 某些效果需要指定参数，例如 `stim` 效果可以指定不同的兴奋剂类型
- 效果名称不区分大小写
- 如果指定的效果不存在，命令会尝试通过反射查找并应用对应的效果类型

## 翻译

该训练器默认使用英文，但我们也提供法语、日语和中文简体版本。您可以使用[通用安装程序](https://github.com/sailro/EscapeFromTarkov-Trainer/releases)指定您的语言，例如使用 `./Installer -l zh-cn` 表示中文简体。

您还可以通过查看[这里](https://github.com/sailro/EscapeFromTarkov-Trainer/issues/541)来调整或添加您自己的语言。