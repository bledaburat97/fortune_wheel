using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BombPopupView : MonoBehaviour
{
    [SerializeField] private RectTransform titleRectTransform;
    [SerializeField] private Button giveUpButton;
    [SerializeField] private Button reviveWithGoldButton;
    [SerializeField] private Button reviveWithAdButton;
    [SerializeField] private TMP_Text cashAmountText;
    [SerializeField] private TMP_Text goldAmountText;
    
    public void InitButtons(Action giveUpAction, Action reviveWithGoldAction, Action reviveWithAdAction)
    {
        giveUpButton.onClick.AddListener(giveUpAction.Invoke);
        reviveWithGoldButton.onClick.AddListener(reviveWithGoldAction.Invoke);
        reviveWithAdButton.onClick.AddListener(reviveWithAdAction.Invoke);
    }
    
    public void InitAssets(int cashAmount, int goldAmount)
    {
        cashAmountText.SetText(cashAmount.ToString());
        goldAmountText.SetText(goldAmount.ToString());
    }

    public RectTransform GetTitleRectTransform()
    {
        return titleRectTransform;
    }
}
