using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GiftItemDeterminer : IGiftItemDeterminer
{
    private Dictionary<int, List<Item>> _zoneIndexToItems = new Dictionary<int, List<Item>>();
    private List<Item> _currentZoneItems = new List<Item>();
    private List<ItemType> _possibleItemTypes = new List<ItemType>();
    public void Initialize(List<ZoneInfo> zoneInfos, List<ItemType> possibleItemTypes)
    {
        _possibleItemTypes = possibleItemTypes;
        foreach (ZoneInfo zoneInfo in zoneInfos)
        {
            if (zoneInfo.zoneIndex > 0 )
            {
                _zoneIndexToItems.TryAdd(zoneInfo.zoneIndex, zoneInfo.items);
            }
            else
            {
                Debug.Log($"{zoneInfo.zoneIndex} is an invalid zone index.");
            }
        }
    }
    
    public List<Item> GetCurrentZoneItems(int zoneIndex, ZoneType zoneType)
    {
        _currentZoneItems = new List<Item>();
        TryAddBombItem(zoneType);
        TrySetZoneItemsFromEditor(zoneIndex, zoneType);
        SetMissingItemsOfZone(zoneIndex);
        ShuffleList(_currentZoneItems);
        return _currentZoneItems;
    }

    private void TryAddBombItem(ZoneType zoneType)
    {
        //A Bomb item must be added in standard zones.
        if (zoneType == ZoneType.Standard)
        {
            _currentZoneItems.Add(new Item()
            {
                itemType = ItemType.Bomb,
                amount = 0
            });
        }
    }

    private void TrySetZoneItemsFromEditor(int zoneIndex, ZoneType zoneType)
    {
        //Add zone items until max number of slice items except bomb item.
        if (_zoneIndexToItems.TryGetValue(zoneIndex, out List<Item> items))
        {
            int i = 0;
            while (_currentZoneItems.Count < ConstantValues.NumberOfSlices && i < items.Count)
            {
                if((items[i].itemType != ItemType.Bomb || zoneType == ZoneType.Standard) && _possibleItemTypes.Contains(items[i].itemType)) _currentZoneItems.Add(items[i]);
                i++;
            }
        }
    }

    private void SetMissingItemsOfZone(int zoneIndex)
    {
        //Set missing items randomly and their amounts according to zoneIndex. (FOR TEST)
        int countOfItemTypes = _possibleItemTypes.Count;
        while (_currentZoneItems.Count < ConstantValues.NumberOfSlices)
        {
            ItemType randomItemType = _possibleItemTypes[Random.Range(0, countOfItemTypes)];
            if (randomItemType != ItemType.Bomb)
            {
                _currentZoneItems.Add(new Item()
                {
                    itemType = randomItemType,
                    amount = randomItemType == ItemType.Cash ? zoneIndex * 100 : zoneIndex
                });
            }
        }
    }

    private static void ShuffleList<T>(List<T> list)
    {
        int i = 0;
        while (i < ConstantValues.NumberOfSlices)
        {
            int randomIndex = Random.Range(i, ConstantValues.NumberOfSlices);
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
            i++;
        }
    }

    public Item GetItem(int index)
    {
        return _currentZoneItems[index];
    }
}

public interface IGiftItemDeterminer
{
    void Initialize(List<ZoneInfo> zoneInfos, List<ItemType> possibleItemTypes);
    List<Item> GetCurrentZoneItems(int zoneIndex, ZoneType zoneType);
    Item GetItem(int index);
}
