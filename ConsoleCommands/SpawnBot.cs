using System;
using System.Linq;
using System.Text.RegularExpressions;
using Comfort.Common;
using EFT.Trainer.Extensions;
using EFT.Trainer.Properties;
using JetBrains.Annotations;

#nullable enable

namespace EFT.Trainer.ConsoleCommands;

[UsedImplicitly]
internal class SpawnBot : ConsoleCommandWithArgument
{
	public const string MatchAll = "*";

	public override string Pattern => RequiredArgumentPattern;
	public override string Name => Strings.CommandSpawnBot;

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

		var bots = FindBots(search);

		switch (bots.Length)
		{
			case 0:
				AddConsoleLog(Strings.ErrorNoBotFound.Red());
				return;

			case > 1 when search != MatchAll:
				foreach (var bot in bots)
					AddConsoleLog(string.Format(Strings.CommandSpawnBotEnumerateFormat, bot.Green()));

				AddConsoleLog(string.Format(Strings.ErrorTooManyBotsFormat, bots.Length.ToString().Cyan()));
				return;
		}

		SpawnBots(bots);
	}

	private void ShowHelp()
	{
		AddConsoleLog(Strings.CommandSpawnBotHelpTitle.Cyan());
		AddConsoleLog("");
		AddConsoleLog(Strings.CommandSpawnBotHelpUsage.Green());
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

	private static void SpawnBots(string[] bots)
	{
		var instance = Singleton<IBotGame>.Instance;
		if (instance == null)
			return;

		var controller = instance.BotsController;
		var spawner = controller?.BotSpawner;

		if (spawner == null)
			return;

		foreach (var bot in bots)
			spawner.SpawnBotByTypeForce(1, (WildSpawnType)Enum.Parse(typeof(WildSpawnType), bot), BotDifficulty.normal, null);
	}

	private static string[] FindBots(string search)
	{
		var names = GetBotNames();

		if (search == MatchAll)
			return names;

		var exactMatch = names
			.Where(n => n.Equals(search, StringComparison.OrdinalIgnoreCase))
			.ToArray();

		if (exactMatch.Length == 1)
			return exactMatch;

		return [.. names.Where(n => n.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)];
	}

	private static string[] GetBotNames()
	{
		var filter = new[] { "test", "event", "spirit", "shooterbtr" };

		return [.. Enum
			.GetNames(typeof(WildSpawnType))
			.Where(n => !filter.Any(f => n.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0))
			.OrderBy(n => n)];
	}
}
