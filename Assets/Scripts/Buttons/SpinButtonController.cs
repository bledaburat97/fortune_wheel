using System;

public class SpinButtonController : ISpinButtonController
{
    private IBaseButtonView _view;
    public event EventHandler SpinButtonClicked;
    public void Initialize (IBaseButtonView view)
    {
        _view = view;
        _view.Init(() => SpinButtonClicked?.Invoke(this, EventArgs.Empty));
    }
}

public interface ISpinButtonController
{
    void Initialize(IBaseButtonView view);
    event EventHandler SpinButtonClicked;
}
