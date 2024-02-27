public class InventoryItemView : ItemView, IInventoryItemView
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}

public interface IInventoryItemView : IItemView
{
    void Destroy();
}
