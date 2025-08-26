using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace RemainingLayhand;

internal static class ModInfo
{
  internal const string Guid = "me.pocke.remaining-layhand";
  internal const string Name = "Remaining Layhand";
  internal const string Version = "1.0.0";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class RemainingLayhand : BaseUnityPlugin
{
  internal static RemainingLayhand Instance { get; private set; }

  public void Awake()
  {
    Instance = this;

    Settings.anchor = Config.Bind("Settings", "anchor", "weight", new ConfigDescription("Where you want to insert the widget", new AcceptableValueList<string>("dvpv", "maxAlly", "maxMinion", "money", "money2", "plat", "medal", "karma", "mood", "fame", "influence", "tourism_value", "hearth_lv", "fertility", "elec", "weight")));
    Settings.position = Config.Bind("Settings", "position", "before", new ConfigDescription("before or after", new AcceptableValueList<string>("before", "after")));

    new Harmony(ModInfo.Guid).PatchAll();
  }

  public static void Log(object message)
  {
    Instance.Logger.LogInfo(message);
  }
}
