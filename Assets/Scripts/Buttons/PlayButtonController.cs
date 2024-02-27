using UnityEngine.SceneManagement;

public class PlayButtonController : IPlayButtonController
{
    private IBaseButtonView _view;

    public void Initialize (IBaseButtonView view)
    {
        _view = view;
        _view.Init(() => SceneManager.LoadScene("GameScene"));
    }
}

public interface IPlayButtonController
{
    void Initialize(IBaseButtonView view);
}