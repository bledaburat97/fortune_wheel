using UnityEngine;

public class MenuContext: MonoBehaviour
{
    [SerializeField] private BaseButtonView playButtonView;
    [SerializeField] private MenuUIView menuUIView;
    void Start()
    {
        IPlayButtonController playButtonController = new PlayButtonController();
        playButtonController.Initialize(playButtonView);
        IMenuUIController menuUIController = new MenuUIController();
        menuUIController.Initialize(menuUIView);
    }
}
