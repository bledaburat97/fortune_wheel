using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePlayContext : MonoBehaviour
{
    [SerializeField] private BaseButtonView spinButtonView;
    [SerializeField] private WheelView wheelView;
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private PopupHolderView popupHolderView;
    [SerializeField] private List<ZoneInfo> zoneInfos;
    [SerializeField] private BaseButtonView exitButtonView;
    
    private Dictionary<ItemType, Sprite> _itemTypeToIconDict = new Dictionary<ItemType, Sprite>();
    void Start()
    {
        _itemTypeToIconDict = AssetManager.Instance.GetItemTypeToIconDict();
        IGiftItemDeterminer giftItemDeterminer = new GiftItemDeterminer();
        giftItemDeterminer.Initialize(zoneInfos, _itemTypeToIconDict.Select(i => i.Key).ToList());
        IInventory inventory = new Inventory();
        inventory.Initialize(inventoryView, _itemTypeToIconDict);
        IWheelController wheelController = new WheelController();
        wheelController.Initialize(wheelView, _itemTypeToIconDict);
        IZoneNumberController zoneNumberController = new ZoneNumberController();
        zoneNumberController.Initialize();
        IGameController gameController = new GameController();
        gameController.Initialize(inventory, wheelController, giftItemDeterminer, zoneNumberController);
        ISpinButtonController spinButtonController = new SpinButtonController();
        spinButtonController.Initialize(spinButtonView, gameController);
        IPopupHolderController popupHolderController = new PopupHolderController();
        popupHolderController.Initialize(popupHolderView, gameController, inventory);
        IExitButtonController exitButtonController = new ExitButtonController();
        exitButtonController.Initialize(exitButtonView, zoneNumberController, wheelController, inventory);
    }
}

[Serializable]
public struct ZoneInfo
{
    public int zoneIndex;
    public List<Item> items;
}

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();
    
    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach(KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
    
    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format(
                "there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}
