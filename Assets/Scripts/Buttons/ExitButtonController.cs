using UnityEngine.SceneManagement;

public class ExitButtonController : IExitButtonController
{
    private IBaseButtonView _view;
    private IZoneNumberController _zoneNumberController;
    private IWheelController _wheelController;
    private IInventory _inventory;
    public void Initialize(IBaseButtonView view, IZoneNumberController zoneNumberController, IWheelController wheelController, IInventory inventory)
    {
        _view = view;
        _zoneNumberController = zoneNumberController;
        _wheelController = wheelController;
        _inventory = inventory;
        _view.Init(OnClick);
    }

    private void OnClick()
    {
        if (!_wheelController.IsSpinning() || _zoneNumberController.GetCurrentZoneType() != ZoneType.Standard)
        {
            AssetManager.Instance.AddItems(_inventory.GetItemAmountDictionary());
            SceneManager.LoadScene("MenuScene");
        }
    }
}

public interface IExitButtonController
{
    void Initialize(IBaseButtonView view, IZoneNumberController zoneNumberController, IWheelController wheelController, IInventory inventory);
}