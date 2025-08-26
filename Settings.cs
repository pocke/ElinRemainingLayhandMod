using BepInEx.Configuration;

namespace RemainingLayhand;

public static class Settings
{
  public static ConfigEntry<string> anchor;
  public static ConfigEntry<string> position;

  public static string Anchor
  {
    get { return anchor.Value; }
    set { anchor.Value = value; }
  }

  public static string Position
  {
    get { return position.Value; }
    set { position.Value = value; }
  }
}
