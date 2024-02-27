using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour, IInventoryView
{
    [SerializeField] private InventoryItemView inventoryItemPrefab;
    [SerializeField] private ScrollRect itemBoard;
    
    public IInventoryItemView CreateInventoryItem()
    {
        return Instantiate(inventoryItemPrefab, itemBoard.content);
    }
}

public interface IInventoryView
{
    IInventoryItemView CreateInventoryItem();
}
