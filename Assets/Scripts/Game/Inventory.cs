using System.Collections.Generic;
using UnityEngine;

public class Inventory : IInventory
{
    private Dictionary<ItemType, int> _inventoryItemAmountDict = new Dictionary<ItemType, int>();
    private Dictionary<ItemType, IInventoryItemView> _inventoryItemDict = new Dictionary<ItemType, IInventoryItemView>();
    private IInventoryView _view;
    private Dictionary<ItemType, Sprite> _itemTypeToIconDict;

    public void Initialize(IInventoryView view, Dictionary<ItemType, Sprite> itemTypeToIconDict)
    {
        _view = view;
        _itemTypeToIconDict = itemTypeToIconDict;
    }

    public void AddItem(Item item)
    {
        if (_inventoryItemDict.ContainsKey(item.itemType))
        {
            int prevAmount = _inventoryItemAmountDict[item.itemType];
            int newAmount = prevAmount + item.amount;
            _inventoryItemAmountDict[item.itemType] = newAmount;
            _inventoryItemDict[item.itemType].SetText(newAmount);
        }
    }

    public void ClearInventory()
    {
        foreach (var inventoryItemPair in _inventoryItemDict)
        {
            inventoryItemPair.Value.Destroy();
        }
        _inventoryItemAmountDict = new Dictionary<ItemType, int>();
        _inventoryItemDict = new Dictionary<ItemType, IInventoryItemView>();
        
    }

    public void TryCreateItem(Item item)
    {
        if (!_inventoryItemDict.ContainsKey(item.itemType))
        {
            IInventoryItemView inventoryItemView = _view.CreateInventoryItem();
            inventoryItemView.Init();
            inventoryItemView.SetImage(_itemTypeToIconDict[item.itemType]);
            inventoryItemView.SetText(0);
            _inventoryItemDict.Add(item.itemType, inventoryItemView);
            _inventoryItemAmountDict.Add(item.itemType, 0);
        }
    }

    public IInventoryItemView GetInventoryItemView(ItemType itemType)
    {
        return _inventoryItemDict[itemType];
    }

    public Dictionary<ItemType, int> GetItemAmountDictionary()
    {
        return _inventoryItemAmountDict;
    }
}

public interface IInventory
{
    void Initialize(IInventoryView view, Dictionary<ItemType, Sprite> itemTypeToIconDict);
    void AddItem(Item item);
    void ClearInventory();
    void TryCreateItem(Item item);
    IInventoryItemView GetInventoryItemView(ItemType itemType);
    Dictionary<ItemType, int> GetItemAmountDictionary();
}
