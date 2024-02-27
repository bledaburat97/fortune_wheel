using TMPro;
using UnityEngine;

public class MenuUIView : MonoBehaviour, IMenuUIView
{
    [SerializeField] private TMP_Text cashAmountText;
    [SerializeField] private TMP_Text goldAmountText;

    public void Init(int cashAmount, int goldAmount)
    {
        cashAmountText.SetText(cashAmount.ToString());
        goldAmountText.SetText(goldAmount.ToString());
    }
}

public interface IMenuUIView
{
    void Init(int cashAmount, int goldAmount);
}


