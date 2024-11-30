﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Comfort.Common;
using Diz.Utils;
using EFT.CameraControl;
using EFT.InventoryLogic;
using EFT.Trainer.Extensions;
using EFT.Trainer.Features;
using EFT.Trainer.Properties;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

#nullable enable

namespace EFT.Trainer.ConsoleCommands;

[UsedImplicitly]
internal class Spawn : BaseTemplateCommand
{
	public override string Name => Strings.CommandSpawn;

	public override void Execute(Match match)
	{
		var matchGroup = match.Groups[ValueGroup];
		if (matchGroup is not { Success: true })
			return;

		var player = GameState.Current?.LocalPlayer;
		if (player == null)
			return;

		var search = matchGroup.Value;
		var templates = TemplateHelper.FindTemplates(search);

		switch (templates.Length)
		{
			case 0:
				AddConsoleLog(Strings.ErrorNoTemplateFound.Red());
				return;
			case > 1:
				foreach (var template in templates)
					AddConsoleLog(string.Format(Strings.CommandTemplateEnumerateFormat, template._id, template.ShortNameLocalizationKey.Localized().Green(), template.NameLocalizationKey.Localized()));

				AddConsoleLog(string.Format(Strings.ErrorTooManyTemplatesFormat, templates.Length.ToString().Cyan()));
				return;
		}

		var tpl = templates[0];
		SpawnTemplate(tpl, player, this);
	}

	internal static void SpawnTemplate(string template, Player player, ConsoleCommand command, Func<ItemTemplate, bool> filter)
	{
		var result = TemplateHelper.FindTemplates(template)
			.FirstOrDefault(filter);

		if (result == null)
			return;

		SpawnTemplate(result, player, command);
	}

	private static void SpawnTemplate(ItemTemplate template, Player player, ConsoleCommand command)
	{
		var poolManager = Singleton<PoolManager>.Instance;

		poolManager
			.LoadBundlesAndCreatePools(PoolManager.PoolsCategory.Raid, PoolManager.AssemblyType.Online, [.. template.AllResources], JobPriority.Immediate)
			.ContinueWith(task =>
			{
				AsyncWorker.RunInMainTread(delegate
				{
					if (task.IsFaulted)
					{
						command.AddConsoleLog(Strings.ErrorFailedToLoadItemBundle.Red());
					}
					else
					{
						var itemFactory = Singleton<ItemFactoryClass>.Instance;
						var item = itemFactory.CreateItem(MongoID.Generate(), template._id, null);
						if (item == null)
						{
							command.AddConsoleLog(Strings.ErrorFailedToCreateItem.Red());
						}
						else
						{
							_ = new TraderControllerClass(item, item.Id, item.ShortName);
							var go = poolManager.CreateLootPrefab(item, ECameraType.Default);

							go.SetActive(value: true);
							var lootItem = Singleton<GameWorld>.Instance.CreateLootWithRigidbody(go, item, item.ShortName, randomRotation: false, null, out _, true);

							var transform = player.Transform;
							var position = transform.position
										   + transform.right * Random.Range(-1f, 1f)
										   + transform.forward * 2f
										   + transform.up * 0.5f;

							lootItem.transform.SetPositionAndRotation(position, transform.rotation);
							lootItem.LastOwner = player;

							// setup after loot item is created, else we are hitting issues with weapon
							SetupItem(itemFactory, item);
						}
					}
				});

				return Task.CompletedTask;
			});
	}

	private static void SetupItem(ItemFactoryClass itemFactory, Item item)
	{
		item.SpawnedInSession = true; // found in raid

		if (item.TryGetItemComponent<DogtagComponent>(out var dogtag))
		{
			dogtag.AccountId = Random.Range(0, int.MaxValue).ToString();
			dogtag.ProfileId = Random.Range(0, int.MaxValue).ToString();
			dogtag.Nickname = $"Rambo{Random.Range(1, 256)}";
			dogtag.Side = Enum.GetValues(typeof(EPlayerSide)).Cast<EPlayerSide>().Random();
			dogtag.Level = Random.Range(1, 69);
			dogtag.Time = DateTime.Now;
			dogtag.Status = "died";
			dogtag.KillerAccountId = Random.Range(0, int.MaxValue).ToString();
			dogtag.KillerProfileId = Random.Range(0, int.MaxValue).ToString();
			dogtag.KillerName = "";
			dogtag.WeaponName = "";
		}

		if (item.TryGetItemComponent<ArmorHolderComponent>(out var armorHolder))
			FillSlots(itemFactory, armorHolder.ArmorSlots);

		if (item.TryGetItemComponent<RepairableComponent>(out var repairable))
		{
			repairable.MaxDurability = repairable.TemplateDurability;
			repairable.Durability = repairable.MaxDurability;
		}

		if (item is CompoundItem compound)
			FillSlots(itemFactory, compound.AllSlots);

		if (item is IAmmoContainer container) // AmmoBox or Magazine
			FillStackSlot(itemFactory, container.Cartridges);
	}

	private static void FillSlots(ItemFactoryClass itemFactory, IEnumerable<Slot> slots)
	{
		foreach (var slot in slots)
		{
			if (slot.Items.Any())
				continue;

			var filter = slot
				.Filters.FirstOrDefault()?
				.Filter.Random();

			if (filter == null)
				continue;

			var item = itemFactory.CreateItem(MongoID.Generate(), filter, null);
			SetupItem(itemFactory, item);

			slot.AddWithoutRestrictions(item);
		}
	}

	private static void FillStackSlot(ItemFactoryClass itemFactory, StackSlot slot)
	{
		var filter = slot
			.Filters.FirstOrDefault()?
			.Filter.Random();

		if (filter == null)
			return;

		while (slot.Count < slot.MaxCount)
		{
			var item = itemFactory.CreateItem(MongoID.Generate(), filter, null);
			slot.Add(item, false);
		}
	}
}
