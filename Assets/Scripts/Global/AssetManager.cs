using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    [SerializeField] private List<ItemScriptableObject> itemOptions = new List<ItemScriptableObject>();
    public static AssetManager Instance { get; private set; }
    private static Dictionary<ItemType, int> _itemTypeToAmountDict = new Dictionary<ItemType, int>();
    private static Dictionary<ItemType, Sprite> _itemTypeToIconDict = new Dictionary<ItemType, Sprite>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            foreach (ItemScriptableObject itemOption in itemOptions)
            {
                _itemTypeToAmountDict.TryAdd(itemOption.itemType, itemOption.itemType == ItemType.Gold ? 100 : 0);
            }
            
            foreach (ItemScriptableObject itemOption in itemOptions)
            {
                _itemTypeToIconDict.TryAdd(itemOption.itemType, itemOption.icon);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public int GetAmountOfItemType(ItemType itemType)
    {
        if (_itemTypeToAmountDict.TryGetValue(itemType, out int amount))
        {
            return amount;
        }
        return 0;
    }

    public void AddItems(Dictionary<ItemType, int> newItemsDict)
    {
        foreach (var newItem in newItemsDict)
        {
            if (_itemTypeToAmountDict.ContainsKey(newItem.Key))
            {
                int prevValue = _itemTypeToAmountDict[newItem.Key];
                _itemTypeToAmountDict[newItem.Key] = prevValue + newItem.Value;
            }
        }
    }

    public bool TrySpendGold(int amountToBeSpend)
    {
        if (_itemTypeToAmountDict.TryGetValue(ItemType.Gold, out int amount))
        {
            if (amount >= amountToBeSpend)
            {
                _itemTypeToAmountDict[ItemType.Gold] = amount - amountToBeSpend;
                return true;
            }

            return false;
        }

        return false;
    }

    public Dictionary<ItemType, Sprite> GetItemTypeToIconDict()
    {
        return _itemTypeToIconDict;
    }
    
    
}
