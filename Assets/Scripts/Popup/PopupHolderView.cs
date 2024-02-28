using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupHolderView : MonoBehaviour, IPopupHolderView
{
    [SerializeField] private BombPopupView bombPopupView;
    private const int RevivePrice = 25;
    public void OpenBombPopup(Action clearInventory, Action onCloseAction)
    {
        bombPopupView.gameObject.SetActive(true);
        bombPopupView.InitButtons(() => GiveUp(clearInventory), () => ReviveWithGold(onCloseAction), () => CloseBombPopup(onCloseAction));
        bombPopupView.InitAssets(AssetManager.Instance.GetAmountOfItemType(ItemType.Cash), AssetManager.Instance.GetAmountOfItemType(ItemType.Gold));
        AnimateTitle();
    }

    private void AnimateTitle()
    {
        DOTween.Sequence().Append(bombPopupView.GetTitleRectTransform().DOLocalMoveY(165f, 0.5f)).SetEase(Ease.InQuad);
    }

    private void GiveUp(Action clearInventory)
    {
        clearInventory.Invoke();
        SceneManager.LoadScene("MenuScene");
    }

    private void ReviveWithGold(Action onCloseAction)
    {
        if (AssetManager.Instance.TrySpendGold(RevivePrice))
        {
            CloseBombPopup(onCloseAction);
        }
    }

    private void CloseBombPopup(Action onCloseAction)
    {
        onCloseAction?.Invoke();
        bombPopupView.gameObject.SetActive(false);
        bombPopupView.GetTitleRectTransform().localPosition = new Vector3(0f, 300f, 0f);
    }
}

public interface IPopupHolderView
{
    void OpenBombPopup(Action clearInventory, Action onCloseAction);
}
