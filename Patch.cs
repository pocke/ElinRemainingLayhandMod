using System;
using HarmonyLib;
using UnityEngine;

namespace RemainingLayhand;

[HarmonyPatch]
public static class Patch
{
  const int LayhandId = 1408;
  static Sprite sprite;

  [HarmonyPrefix, HarmonyPatch(typeof(WidgetStatsBar), nameof(WidgetStatsBar.Add))]
  public static void WidgetStatsBar_Add_Prefix(WidgetStatsBar __instance, string id)
  {
    if (Settings.Position == "before")
    {
      apply(__instance, id);
    }
  }

  [HarmonyPostfix, HarmonyPatch(typeof(WidgetStatsBar), nameof(WidgetStatsBar.Add))]
  public static void WidgetStatsBar_Add_Postfix(WidgetStatsBar __instance, string id)
  {
    if (Settings.Position == "after")
    {
      apply(__instance, id);
    }
  }

  private static void apply(WidgetStatsBar w, string id)
  {
    if (id != Settings.Anchor) { return; }

    w.Add(null, "layhand", Sprite(), () => $"{remainingLayhand()}/{maxLayhand()}");
    w.Refresh();
  }

  private static int maxLayhand()
  {
    int ret = 0;

    foreach (Chara c in EClass._map.charas)
    {
      if (layhandable(c)) { ret++; }
    }
    return ret;
  }

  private static int remainingLayhand()
  {
    int ret = 0;
    var now = EClass.world.date.GetRawDay();

    foreach (Chara c in EClass._map.charas)
    {
      if (layhandable(c) && now != c.GetInt(58)) { ret++; }
    }
    return ret;
  }

  // https://github.com/Elin-Modding-Resources/Elin-Decompiled/blob/72332a1390e68a8de62bca4acbd6ebbaab92257b/Elin/Card.cs#L4317
  static bool layhandable(Chara c)
  {
    return EClass.pc.IsFriendOrAbove(c) && c.HasElement(LayhandId) && c.faith == EClass.game.religions.Healing;
  }

  static Sprite Sprite()
  {
    if (sprite == null)
    {
      var targetSprite = WidgetStatsBar.Instance.iconMoney;

      var orig = CharaGen.Create("turtle").GetSprite();
      var tex = orig.texture;
      var rect = orig.textureRect;
      Rect bottomRect = new Rect(
        rect.x,
        rect.y,
        rect.width,
        rect.height / 2f
      );
      Vector2 pivot = new Vector2(
        orig.pivot.x / rect.width,
        orig.pivot.y / rect.height
      );
      var ppu = tex.width / (targetSprite.texture.width / targetSprite.pixelsPerUnit) * 0.6f;

      sprite = UnityEngine.Sprite.Create(tex, bottomRect, pivot, ppu);
    }
    return sprite;
  }
}
