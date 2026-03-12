using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using EFT.HealthSystem;
using EFT.Trainer.Features;
using EFT.Trainer.Properties;
using JetBrains.Annotations;
using UnityEngine;

#nullable enable

namespace EFT.Trainer.ConsoleCommands;

[UsedImplicitly]
internal class Buff : ConsoleCommandWithArgument
{
	public override string Name => "buff";
	public override string Pattern => $"(?<{ValueGroup}>.*)";

	// 效果类型映射字典
	private static readonly Dictionary<string, EffectHandler> EffectHandlers = new Dictionary<string, EffectHandler>
	{
		// 出血类
		["lightbleed"] = new EffectHandler("LightBleeding", (h, p, b) => h.DoBleed(false, b)),
		["lightbleeding"] = new EffectHandler("LightBleeding", (h, p, b) => h.DoBleed(false, b)),
		["heavybleed"] = new EffectHandler("HeavyBleeding", (h, p, b) => h.DoBleed(true, b)),
		["heavybleeding"] = new EffectHandler("HeavyBleeding", (h, p, b) => h.DoBleed(true, b)),
		["bleed"] = new EffectHandler("LightBleeding", (h, p, b) => h.DoBleed(false, b)),

		// 骨折
		["fracture"] = new EffectHandler("Fracture", (h, p, b) => h.DoFracture(b)),

		// 疼痛类
		["pain"] = new EffectHandler("Pain", (h, p, b) => AddPainEffect(h, b, p)),
		["tremor"] = new EffectHandler("Tremor", (h, p, b) => AddTremorEffect(h, b)),
		["stun"] = new EffectHandler("Stun", (h, p, b) => h.DoStun(p ?? 5f, 1f)),
		["contusion"] = new EffectHandler("Contusion", (h, p, b) => h.DoContusion(p ?? 5f, 1f)),
		["disorientation"] = new EffectHandler("Disorientation", (h, p, b) => h.DoDisorientation(p ?? 10f)),

		// 中毒/辐射
		["intoxication"] = new EffectHandler("Intoxication", (h, p, b) => h.DoIntoxication()),
		["poison"] = new EffectHandler("Intoxication", (h, p, b) => h.DoIntoxication()),
		["lethalintoxication"] = new EffectHandler("LethalIntoxication", (h, p, b) => h.DoLethalIntoxication(0f, p ?? 30f)),
		["radexposure"] = new EffectHandler("RadExposure", (h, p, b) => h.DoRadExposure()),
		["radiation"] = new EffectHandler("RadExposure", (h, p, b) => h.DoRadExposure()),

		// 药物/增益类
		["painkiller"] = new EffectHandler("PainKiller", (h, p, b) => h.DoPainKiller()),
		["stop"] = new EffectHandler("PainKiller", (h, p, b) => h.DoPainKiller()),
		["pills"] = new EffectHandler("PainKiller", (h, p, b) => h.DoPainKiller()),
		["stim"] = new EffectHandler("Stimulator", ApplyStimulator),
		["stimulator"] = new EffectHandler("Stimulator", ApplyStimulator),
		["healthboost"] = new EffectHandler("HealthBoost", (h, p, b) => h.DoPermanentHealthBoost(p ?? 1f)),
		["regeneration"] = new EffectHandler("Regeneration", ApplyRegeneration),

		// 特殊效果
		["misfire"] = new EffectHandler("MisfireEffect", (h, p, b) => h.AddMisfireEffect(p, true)),
		["staminazero"] = new EffectHandler("StaminaZero", (h, p, b) => h.AddStaminaZeroffect(p)),
		["fatigue"] = new EffectHandler("ChronicStaminaFatigue", (h, p, b) => h.AddFatigue()),
		["berserk"] = new EffectHandler("Berserk", (h, p, b) => AddBerserkEffect(h, b)),
		["paniceffect"] = new EffectHandler("PanicEffect", (h, p, b) => h.DoEventEffect()),
		["sandingscreen"] = new EffectHandler("SandingScreen", (h, p, b) => h.DoSandingScreen(p ?? 5f)),
		["tunnelvision"] = new EffectHandler("TunnelVision", (h, p, b) => AddTunnelVisionEffect(h, b, p)),
		["dehydration"] = new EffectHandler("Dehydration", ApplyDehydration),
		["dehydrate"] = new EffectHandler("Dehydration", ApplyDehydration),
		["thirst"] = new EffectHandler("Dehydration", ApplyDehydration),

		// 视觉类
		["flash"] = new EffectHandler("Flash", ApplyFlash),

		// 负重状态
		["encumbered"] = new EffectHandler("Encumbered", (h, p, b) => h.SetEncumbered(true)),
		["overencumbered"] = new EffectHandler("OverEncumbered", (h, p, b) => h.SetOverEncumbered(true)),

		// 感染类
		["zombieinfection"] = new EffectHandler("ZombieInfection", (h, p, b) => h.DoZombieInfection(b)),

		// 移除效果
		["remove"] = new EffectHandler("RemoveNegativeEffects", RemoveNegativeEffects),
		["removeall"] = new EffectHandler("RemoveAllEffects", RemoveAllEffects),

		// 恢复类
		["fullheal"] = new EffectHandler("FullHealthRegenerationEffect", (h, p, b) => h.RestoreFullHealth()),
		["restorebodypart"] = new EffectHandler("RestoreBodyPart", RestoreBodyPart),
	};

	public override void Execute(Match match)
	{
		var matchGroup = match.Groups[ValueGroup];
		if (matchGroup is not { Success: true })
			return;

		var input = matchGroup.Value.Trim();

		// 处理 help 命令
		if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
		{
			ShowHelp();
			return;
		}

		if (input.Equals("list", StringComparison.OrdinalIgnoreCase))
		{
			ShowAvailableEffects();
			return;
		}

		// 改进分割逻辑：支持空格、逗号、冒号，同时保留负号和小数点
		var parts = input.Split(new[] { ' ', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length == 0) return;

		string effectInput = parts[0].ToLower();
		EBodyPart bodyPart = EBodyPart.Common;
		float? parameter = null;

		// 解析身体部位和参数 (支持 "effect,bodypart,strength" 格式)
		if (parts.Length >= 2)
		{
			// 使用 System.Globalization 确保点号解析不受系统语言影响
			if (float.TryParse(parts[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var paramValue))
			{
				parameter = paramValue;
			}
			else if (Enum.TryParse<EBodyPart>(parts[1], true, out var parsedPart))
			{
				bodyPart = parsedPart;
			}
		}

		if (parts.Length >= 3)
		{
			if (float.TryParse(parts[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var paramValue2))
			{
				parameter = paramValue2;
			}
		}

		var player = GameState.Current?.LocalPlayer;
		if (player == null)
		{
			AddConsoleLog(Strings.CommandBuffErrorPlayerNotFound.Red());
			return;
		}

		var healthController = player.ActiveHealthController;
		if (healthController == null)
		{
			AddConsoleLog(Strings.CommandBuffErrorHealthControllerNotFound.Red());
			return;
		}

		try
		{
			if (!EffectHandlers.TryGetValue(effectInput, out var handler))
			{
				// 尝试通过反射查找
				if (!TryApplyViaReflection(healthController, effectInput, bodyPart, parameter))
				{
					AddConsoleLog(string.Format(Strings.CommandBuffErrorUnknownEffect, effectInput).Red());
					ShowAvailableEffects();
				}
				return;
			}

			// 执行效果处理器
			handler.Apply(healthController, parameter, bodyPart);

			string message = string.Format(Strings.CommandBuffSuccessApplied, handler.DisplayName.Yellow());
			if (bodyPart != EBodyPart.Common)
				message += $" {Strings.CommandBuffSuccessTo} {bodyPart.ToString().Green()}";
			if (parameter.HasValue)
				message += $" {Strings.CommandBuffSuccessWithStrength} {parameter.Value}".Yellow();

			AddConsoleLog(message);
		}
		catch (Exception ex)
		{
			AddConsoleLog(string.Format(Strings.CommandBuffErrorExecutionFailed, ex.Message).Red());
		}
	}

	private bool TryApplyViaReflection(ActiveHealthController healthController, string effectName, EBodyPart bodyPart, float? parameter)
	{
		var controllerType = healthController.GetType();

		// 查找匹配的嵌套类型
		var effectType = controllerType
			.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
			.FirstOrDefault(t => t.Name.Equals(effectName, StringComparison.OrdinalIgnoreCase)
				&& t.IsSubclassOf(typeof(ActiveHealthController.GClass3008)));

		if (effectType == null)
			return false;

		// 尝试通过 AddEffect<T> 泛型方法添加效果
		var addEffectMethod = controllerType
			.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
			.FirstOrDefault(m => m.Name == "AddEffect"
				&& m.IsGenericMethod
				&& m.GetParameters().Length == 6);

		if (addEffectMethod != null)
		{
			var genericMethod = addEffectMethod.MakeGenericMethod(effectType);
			genericMethod.Invoke(healthController, new object[] {
				bodyPart,          // EBodyPart
                null,              // float? delayTime
                parameter,         // float? workTime
                null,              // float? residueTime
                parameter,         // float? strength
                null               // Action<T> initCallback
            });

			AddConsoleLog($"Applied {effectType.Name.Yellow()} via reflection to {bodyPart.ToString().Green()}");
			return true;
		}

		return false;
	}

	// 辅助方法：添加 Pain 效果
	private static void AddPainEffect(ActiveHealthController h, EBodyPart bodyPart, float? strength)
	{
		var method = h.GetType().GetMethod("method_27", BindingFlags.NonPublic | BindingFlags.Instance);
		if (method != null)
		{
			method.Invoke(h, new object[] { bodyPart, 0f, 10f, 5f, strength ?? 1f });
		}
	}

	// 辅助方法：添加 Tremor 效果
	private static void AddTremorEffect(ActiveHealthController h, EBodyPart bodyPart)
	{
		var method = h.GetType().GetMethod("AddEffect", new[] { typeof(EBodyPart), typeof(float?), typeof(float?), typeof(float?), typeof(float?), typeof(object) });
		if (method != null)
		{
			var tremorType = h.GetType().GetNestedType("Tremor", BindingFlags.NonPublic);
			if (tremorType != null)
			{
				var genericMethod = method.MakeGenericMethod(tremorType);
				genericMethod.Invoke(h, new object[] { bodyPart, null, null, null, null, null });
			}
		}
	}

	// 辅助方法：添加 Berserk 效果
	private static void AddBerserkEffect(ActiveHealthController h, EBodyPart bodyPart)
	{
		var method = h.GetType().GetMethod("AddEffect", new[] { typeof(EBodyPart), typeof(float?), typeof(float?), typeof(float?), typeof(float?), typeof(object) });
		if (method != null)
		{
			var berserkType = h.GetType().GetNestedType("Berserk", BindingFlags.NonPublic);
			if (berserkType != null)
			{
				var genericMethod = method.MakeGenericMethod(berserkType);
				genericMethod.Invoke(h, new object[] { bodyPart, null, null, null, null, null });
			}
		}
	}

	// 辅助方法：添加 TunnelVision 效果
	private static void AddTunnelVisionEffect(ActiveHealthController h, EBodyPart bodyPart, float? duration)
	{
		var method = h.GetType().GetMethod("AddEffect", new[] { typeof(EBodyPart), typeof(float?), typeof(float?), typeof(float?), typeof(float?), typeof(object) });
		if (method != null)
		{
			var tunnelVisionType = h.GetType().GetNestedType("TunnelVision", BindingFlags.NonPublic);
			if (tunnelVisionType != null)
			{
				var genericMethod = method.MakeGenericMethod(tunnelVisionType);
				genericMethod.Invoke(h, new object[] { bodyPart, null, duration, null, null, null });
			}
		}
	}

	private static void ApplyDehydration(ActiveHealthController h, float? param, EBodyPart bodyPart)
	{
		// 保存当前Hydration值
		float originalHydration = h.Hydration.Current;

		// 临时将Hydration设为0
		if (originalHydration > 0)
		{
			h.ChangeHydration(-originalHydration);
		}

		// 添加脱水效果
		var addEffectMethod = h.GetType().GetMethods()
			.FirstOrDefault(m => m.Name == "AddEffect" && m.IsGenericMethod &&
								m.GetParameters().Length == 6);

		if (addEffectMethod != null)
		{
			var dehydrationType = h.GetType().GetNestedType("Dehydration", BindingFlags.NonPublic);
			if (dehydrationType != null)
			{
				var genericMethod = addEffectMethod.MakeGenericMethod(dehydrationType);
				EBodyPart targetPart = bodyPart != EBodyPart.Common ? bodyPart : EBodyPart.Stomach;

				genericMethod.Invoke(h, new object[] {
				targetPart, null, param, null, param, null
			});
			}
		}

		// 立即恢复原来的Hydration值
		if (originalHydration > 0)
		{
			h.ChangeHydration(originalHydration);
		}
	}

	// 特定的效果处理器
	private static void ApplyStimulator(ActiveHealthController h, float? param, EBodyPart bodyPart)
	{
		// 如果没有指定buff名称，使用默认
		string buffName = "BuffsStimulator";
		if (param.HasValue)
		{
			// 如果参数是整数，尝试作为buff索引
			int buffIndex = (int)param.Value;
			buffName = $"Buffs{buffIndex}";
		}
		h.DoExternalBuff(buffName, 0f);
	}

	private static void ApplyRegeneration(ActiveHealthController h, float? param, EBodyPart bodyPart)
	{
		h.DoScavRegeneration(param ?? 10f);
	}

	private static void ApplyFlash(ActiveHealthController h, float? param, EBodyPart bodyPart)
	{
		h.DoBurnEyes(Vector3.zero, param ?? 1f, 1f, param ?? 5f);
	}

	private static void RemoveNegativeEffects(ActiveHealthController h, float? param, EBodyPart bodyPart)
	{
		h.RemoveNegativeEffects(bodyPart);
	}

	private static void RemoveAllEffects(ActiveHealthController h, float? param, EBodyPart bodyPart)
	{
		// 最简单有效的方式：调用健康控制器的内置方法
		// 1. 移除所有负面效果
		foreach (EBodyPart part in Enum.GetValues(typeof(EBodyPart)))
		{
			h.RemoveNegativeEffects(part);
		}

		// 2. 移除所有医疗后遗症效果 (如 PainKiller 的后遗症)
		var removeMedEffectMethod = h.GetType().GetMethod("RemoveMedEffect", BindingFlags.NonPublic | BindingFlags.Instance);
		removeMedEffectMethod?.Invoke(h, null);
	}

	private static void RestoreBodyPart(ActiveHealthController h, float? param, EBodyPart bodyPart)
	{
		h.FullRestoreBodyPart(bodyPart);
	}

	private void ShowHelp()
	{
		AddConsoleLog(Strings.CommandBuffHelpTitle.Yellow());
		AddConsoleLog("");
		AddConsoleLog(Strings.CommandBuffHelpUsage.Green());
		AddConsoleLog(Strings.CommandBuffHelpUsageLine1);
		AddConsoleLog(Strings.CommandBuffHelpUsageLine2);
		AddConsoleLog("");
		AddConsoleLog(Strings.CommandBuffHelpExamples.Green());
		AddConsoleLog(Strings.CommandBuffHelpExample1);
		AddConsoleLog(Strings.CommandBuffHelpExample2);
		AddConsoleLog(Strings.CommandBuffHelpExample3);
		AddConsoleLog(Strings.CommandBuffHelpExample4);
		AddConsoleLog(Strings.CommandBuffHelpExample5);
		AddConsoleLog(Strings.CommandBuffHelpExample6);
		AddConsoleLog("");
		AddConsoleLog(Strings.CommandBuffHelpBodyParts.Green());
		AddConsoleLog("");
		AddConsoleLog(Strings.CommandBuffHelpTip.Yellow());
	}

	private void ShowAvailableEffects()
	{
		AddConsoleLog(Strings.CommandBuffListTitle.Yellow());

		var categories = EffectHandlers
			.GroupBy(kvp => GetCategory(kvp.Key))
			.OrderBy(g => g.Key);

		foreach (var category in categories)
		{
			AddConsoleLog($"\n{category.Key}:".Green());
			var effects = category.Select(kvp => kvp.Key).OrderBy(k => k);
			AddConsoleLog(string.Join(", ", effects).Yellow());
		}

		AddConsoleLog($"\n{Strings.CommandBuffListTip}".Yellow());
	}

	private string GetCategory(string effectName)
	{
		if (effectName.Contains("bleed")) return "Bleeding";
		if (effectName.Contains("fracture")) return "Injuries";
		if (effectName.Contains("pain") || effectName.Contains("tremor") || effectName.Contains("stun") || effectName.Contains("contusion")) return "Pain/Stun";
		if (effectName.Contains("intoxication") || effectName.Contains("poison") || effectName.Contains("rad")) return "Toxins";
		if (effectName.Contains("painkiller") || effectName.Contains("stim") || effectName.Contains("boost") || effectName.Contains("regeneration")) return "Medicines";
		if (effectName.Contains("misfire") || effectName.Contains("fatigue") || effectName.Contains("berserk") || effectName.Contains("panic")) return "Special";
		if (effectName.Contains("encumbered")) return "Weight";
		if (effectName.Contains("remove") || effectName.Contains("heal") || effectName.Contains("restore")) return "Cleansing";
		if (effectName.Contains("flash") || effectName.Contains("sand") || effectName.Contains("tunnel")) return "Visual";
		if (effectName.Contains("zombie")) return "Infection";
		return "Other";
	}

	// 效果处理器辅助类
	private class EffectHandler
	{
		public string DisplayName { get; }
		private readonly Action<ActiveHealthController, float?, EBodyPart> _handler;

		public EffectHandler(string displayName, Action<ActiveHealthController, float?, EBodyPart> handler)
		{
			DisplayName = displayName;
			_handler = handler;
		}

		public void Apply(ActiveHealthController healthController, float? parameter, EBodyPart bodyPart)
		{
			_handler(healthController, parameter, bodyPart);
		}
	}
}
