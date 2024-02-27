using UnityEngine;

[CreateAssetMenu(fileName = "Wheel", menuName = "ScriptableObjects/Wheel")]
public class WheelScriptableObject : ScriptableObject
{
    public ZoneType zoneType;
    public Sprite wheel;
    public Sprite indicator;
    public string title;
    public string color;
}

public enum ZoneType
{
    Standard,
    Safe,
    Super
}