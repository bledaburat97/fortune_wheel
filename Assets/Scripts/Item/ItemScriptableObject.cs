using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class ItemScriptableObject : ScriptableObject
{
    public ItemType itemType;
    public Sprite icon;
}

public enum ItemType
{
    Cash,
    Gold,
    BigBox,
    BronzeBox,
    SilverBox,
    GoldenBox,
    AviatorGlasses,
    BaseballCap,
    GrenadeM26,
    Tier1Shotgun,
    Tier2Rifle,
    Tier3Sniper,
    ArmorPoints,
    PistolPoints,
    Bomb
}