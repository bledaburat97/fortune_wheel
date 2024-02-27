using UnityEngine;
using UnityEngine.UI;

public class EarnedItemView : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image innerImage;
    [SerializeField] private CanvasGroup canvasGroup;

    public void Init(Vector3 sizeDelta, Vector3 localPos)
    {
        rectTransform.sizeDelta = sizeDelta;
        rectTransform.localPosition = localPos;
    }
    
    public void SetStatus(bool status)
    {
        gameObject.SetActive(status);
    }

    public void SetColor(ItemType itemType)
    {
        innerImage.color = itemType == ItemType.Bomb ? ConstantValues.BombHolderColor : ConstantValues.GiftHolderColor;
    }

    public RectTransform GetRectTransform()
    {
        return rectTransform;
    }

    public CanvasGroup GetCanvasGroup()
    {
        return canvasGroup;
    }
}
