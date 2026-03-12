using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.Trainer.Extensions;
using EFT.Trainer.Features;
using EFT.Trainer.Properties;
using JetBrains.Annotations;
using UnityEngine;

#nullable enable

namespace EFT.Trainer.ConsoleCommands;

[UsedImplicitly]
internal class SpawnBot : ConsoleCommandWithArgument
{
	public const string MatchAll = "*";

	public override string Name => Strings.CommandSpawnBot;
	public override string Pattern => $"(?<{ValueGroup}>.*)";

	public override void Execute(Match match)
	{
		var matchGroup = match.Groups[ValueGroup];
		if (matchGroup is not { Success: true })
			return;

		var search = matchGroup.Value.Trim();

		if (search.Equals("help", StringComparison.OrdinalIgnoreCase))
		{
			ShowHelp();
			return;
		}

		// 改进分割逻辑：支持空格、逗号、冒号，同时保留负号和小数点
		var parts = search.Split(new[] { ' ', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length == 0) return;

		string botSearch = parts[0];
		Vector3? spawnPosition = null;

		// 坐标模式解析 (e.g., bear,-100.5,20.2,300)
		if (parts.Length >= 4)
		{
			// 使用 System.Globalization 确保点号解析不受系统语言影响
			if (float.TryParse(parts[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var x) &&
				float.TryParse(parts[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var y) &&
				float.TryParse(parts[3], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var z))
			{
				// 转换为整数坐标 (取整)
				spawnPosition = new Vector3(Mathf.Round(x), Mathf.Round(y), Mathf.Round(z));
			}
			else
			{
				AddConsoleLog("Invalid coordinates format. Use: name,x,y,z".Red());
				return;
			}
		}
		// 玩家位置模式
		else if (parts.Length >= 2 && parts[1].Equals("player", StringComparison.OrdinalIgnoreCase))
		{
			var player = GameState.Current?.LocalPlayer;
			if (player != null)
			{
				var pos = player.Transform.position;
				// 同样对玩家坐标取整
				spawnPosition = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
			}
		}

		var bots = FindBots(botSearch);
		if (bots.Length == 0)
		{
			AddConsoleLog(Strings.ErrorNoBotFound.Red());
			return;
		}

		if (bots.Length > 1 && botSearch != MatchAll)
		{
			foreach (var bot in bots) AddConsoleLog(string.Format(Strings.CommandSpawnBotEnumerateFormat, bot.Green()));
			AddConsoleLog(string.Format(Strings.ErrorTooManyBotsFormat, bots.Length.ToString().Cyan()));
			return;
		}

		PerformSpawn(bots, spawnPosition);
	}

	private async void PerformSpawn(string[] bots, Vector3? targetPos)
	{
		var instance = Singleton<IBotGame>.Instance;
		if (instance?.BotsController?.BotSpawner == null) return;

		var spawner = instance.BotsController.BotSpawner;

		foreach (var botName in bots)
		{
			if (!Enum.TryParse<WildSpawnType>(botName, out var type)) continue;

			if (targetPos.HasValue)
			{
				Vector3 destination = targetPos.Value;

				Action<BotOwner>? onCreated = null;
				onCreated = async owner =>
				{
					// 增加对 owner 的 null 检查和类型校验
					if (owner != null && owner.Profile?.Info?.Settings?.Role == type)
					{
						try
						{
							// 立即解绑，防止多次触发
							spawner.OnBotCreated -= onCreated;

							// 稍微等待，让游戏引擎完成初始化的随机摆放
							await Task.Yield();
							await Task.Delay(100);

							if (owner == null || owner.Transform == null) return;

							// 反射操作 NavMeshAgent
							var agent = owner.GetPlayer?.gameObject.GetComponent("NavMeshAgent");
							if (agent != null)
							{
								var agentType = agent.GetType();
								var enabledProp = agentType.GetProperty("enabled");
								var warpMethod = agentType.GetMethod("Warp", BindingFlags.Public | BindingFlags.Instance);

								enabledProp?.SetValue(agent, false);
								owner.Transform.position = destination;
								enabledProp?.SetValue(agent, true);

								// 使用 Warp 强制同步导航网格
								warpMethod?.Invoke(agent, new object[] { destination });
							}
							else
							{
								owner.Transform.position = destination;
							}

							AddConsoleLog($"Bot {botName} deployed to Int-Coordinates: {destination.x}, {destination.y}, {destination.z}".Cyan());
						}
						catch (Exception ex)
						{
							AddConsoleLog($"Teleport Failed: {ex.Message}".Red());
						}
					}
				};

				spawner.OnBotCreated += onCreated;
				_ = Task.Delay(10000).ContinueWith(_ => spawner.OnBotCreated -= onCreated);
			}

			await spawner.SpawnBotByTypeForce(1, type, BotDifficulty.normal, null);
		}
	}

	private void ShowHelp()
	{
		// 全部映射到你提供的 XML 资源文件中的 Key
		AddConsoleLog(Strings.CommandSpawnBotHelpTitle.Cyan());
		AddConsoleLog("");
		AddConsoleLog(Strings.CommandSpawnBotHelpUsage.Green());
		AddConsoleLog(Strings.CommandSpawnBotHelpUsageCoordinates.Green());
		AddConsoleLog(Strings.CommandSpawnBotHelpUsagePlayer.Green());

		AddConsoleLog("");
		AddConsoleLog(Strings.CommandSpawnBotHelpFactionTypes.Yellow());
		AddConsoleLog(Strings.CommandSpawnBotHelpUsec);
		AddConsoleLog(Strings.CommandSpawnBotHelpBear);
		AddConsoleLog(Strings.CommandSpawnBotHelpAssault);
		AddConsoleLog(Strings.CommandSpawnBotHelpMarksman);
		AddConsoleLog(Strings.CommandSpawnBotHelpCursedAssault);
		AddConsoleLog(Strings.CommandSpawnBotHelpInfectedAssault);

		AddConsoleLog("");
		AddConsoleLog(Strings.CommandSpawnBotHelpBosses.Yellow());
		AddConsoleLog(Strings.CommandSpawnBotHelpTagilla);
		AddConsoleLog(Strings.CommandSpawnBotHelpSanitar);
		AddConsoleLog(Strings.CommandSpawnBotHelpGluhar);
		AddConsoleLog(Strings.CommandSpawnBotHelpKilla);
		AddConsoleLog(Strings.CommandSpawnBotHelpBoar);
		AddConsoleLog(Strings.CommandSpawnBotHelpKolontay);
		AddConsoleLog(Strings.CommandSpawnBotHelpZryachiy);
		AddConsoleLog(Strings.CommandSpawnBotHelpKnight);
		AddConsoleLog(Strings.CommandSpawnBotHelpPartisan);

		AddConsoleLog("");
		AddConsoleLog(Strings.CommandSpawnBotHelpCultists.Yellow());
		AddConsoleLog(Strings.CommandSpawnBotHelpSectantPriest);
		AddConsoleLog(Strings.CommandSpawnBotHelpSectantWarrior);
		AddConsoleLog(Strings.CommandSpawnBotHelpSectantPrizrak);
		AddConsoleLog(Strings.CommandSpawnBotHelpSectantPredvestnik);
		AddConsoleLog(Strings.CommandSpawnBotHelpSectantOni);

		AddConsoleLog("");
		AddConsoleLog(Strings.CommandSpawnBotHelpSpecialTypes.Yellow());
		AddConsoleLog(Strings.CommandSpawnBotHelpSkier);

		AddConsoleLog("");
		AddConsoleLog(Strings.CommandSpawnBotHelpAll.Cyan());
	}

	private static string[] FindBots(string search)
	{
		var names = GetBotNames();
		if (search == MatchAll) return names;
		var exactMatch = names.Where(n => n.Equals(search, StringComparison.OrdinalIgnoreCase)).ToArray();
		if (exactMatch.Length == 1) return exactMatch;
		return [.. names.Where(n => n.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)];
	}

	private static string[] GetBotNames()
	{
		var filter = new[] { "test", "event", "spirit", "shooterbtr" };
		return [.. Enum.GetNames(typeof(WildSpawnType))
			.Where(n => !filter.Any(f => n.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0))
			.OrderBy(n => n)];
	}
}
