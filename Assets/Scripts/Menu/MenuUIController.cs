public class MenuUIController : IMenuUIController
{
    public void Initialize(IMenuUIView view)
    {
        int cashAmount = AssetManager.Instance.GetAmountOfItemType(ItemType.Cash);
        int goldAmount = AssetManager.Instance.GetAmountOfItemType(ItemType.Gold);
        view.Init(cashAmount, goldAmount);
    }
}

public interface IMenuUIController
{
    void Initialize(IMenuUIView view);
}
