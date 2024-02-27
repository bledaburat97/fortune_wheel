public class SpinButtonController : ISpinButtonController
{
    private IBaseButtonView _view;

    public void Initialize (IBaseButtonView view, IGameController gameController)
    {
        _view = view;
        _view.Init(gameController.Spin);
    }
}

public interface ISpinButtonController
{
    void Initialize(IBaseButtonView view, IGameController gameController);
}
