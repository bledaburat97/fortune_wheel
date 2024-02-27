using System;

public class PopupHolderController : IPopupHolderController
{
    private IPopupHolderView _view;
    private IInventory _inventory;
    
    public void Initialize(IPopupHolderView view, IGameController gameController, IInventory inventory)
    {
        gameController.OnBombExploded += OpenBombPopup;
        _view = view;
        _inventory = inventory;
    }

    private void OpenBombPopup(object sender, Action onCloseAction)
    {
        _view.OpenBombPopup(_inventory.ClearInventory, onCloseAction);
    }
}

public interface IPopupHolderController
{
    void Initialize(IPopupHolderView view, IGameController wheelController, IInventory inventory);
}
