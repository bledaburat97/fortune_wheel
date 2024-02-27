using UnityEngine;

public static class ConstantValues
{
      public static readonly int NumberOfSlices = 8;
      public static readonly Color GiftHolderColor = Color.black;
      public static readonly Color BombHolderColor = HexToColor("#BA2828");

      public static Color HexToColor(string hex)
      {
            Color color = Color.white; // Default color in case of invalid hex string

            if (ColorUtility.TryParseHtmlString(hex, out color))
            {
                  return color;
            }
            else
            {
                  Debug.LogWarning("Invalid hex color string: " + hex);
                  return color;
            }
      }
}
