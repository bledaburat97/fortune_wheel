using System.Collections.Generic;
using UnityEngine;

public class Inventory : IInventory
{
    private IInventoryView _view;
    private Dictionary<ItemType, int> _itemTypeToAmountDict = new Dictionary<ItemType, int>();

    private Dictionary<ItemType, IInventoryItemView> _itemTypeToItemViewDict =
        new Dictionary<ItemType, IInventoryItemView>();

    private Dictionary<ItemType, Sprite> _itemTypeToIconDict = new Dictionary<ItemType, Sprite>();

    public void Initialize(IInventoryView view, Dictionary<ItemType, Sprite> itemTypeToIconDict)
    {
        _view = view;
        _itemTypeToIconDict = itemTypeToIconDict;
    }

    public void AddItem(Item item)
    {
        if (_itemTypeToItemViewDict.ContainsKey(item.itemType))
        {
            int prevAmount = _itemTypeToAmountDict[item.itemType];
            int newAmount = prevAmount + item.amount;
            _itemTypeToAmountDict[item.itemType] = newAmount;
            _itemTypeToItemViewDict[item.itemType].SetText(newAmount);
        }
    }

    public void ClearInventory()
    {
        foreach (var inventoryItemPair in _itemTypeToItemViewDict)
        {
            inventoryItemPair.Value.Destroy();
        }

        _itemTypeToAmountDict.Clear();
        _itemTypeToItemViewDict.Clear();

    }

    public void TryCreateItem(Item item)
    {
        if (!_itemTypeToItemViewDict.ContainsKey(item.itemType))
        {
            IInventoryItemView inventoryItemView = _view.CreateInventoryItem();
            inventoryItemView.Init();
            inventoryItemView.SetImage(_itemTypeToIconDict[item.itemType]);
            inventoryItemView.SetText(0);
            _itemTypeToItemViewDict.Add(item.itemType, inventoryItemView);
            _itemTypeToAmountDict.Add(item.itemType, 0);
        }
    }

    public IInventoryItemView GetInventoryItemView(ItemType itemType)
    {
        if (_itemTypeToItemViewDict.TryGetValue(itemType, out var view))
        {
            return view;
        }

        Debug.LogError($"Item type {itemType} not found in inventory.");
        return null;
    }

    public Dictionary<ItemType, int> GetItemAmountDictionary()
    {
        return _itemTypeToAmountDict;
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
