using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelView : MonoBehaviour, IWheelView
{
    private const int WheelOptionCount = 3;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;
    [SerializeField] private Image indicator;
    [SerializeField] private WheelScriptableObject[] wheelOptions = new WheelScriptableObject[WheelOptionCount];
    [SerializeField] private GiftItemView giftItemPrefab;
    [SerializeField] private EarnedItemView earnedItemView;
    [SerializeField] private TMP_Text wheelTitle;
    public RectTransform GetRectTransform()
    {
        return rectTransform;
    }

    public IGiftItemView CreateGiftObject()
    {
        return Instantiate(giftItemPrefab, transform);
    }
    
    public void SetStatusOfEarnedItemBg(bool status)
    {
        earnedItemView.SetStatus(status);
    }

    public RectTransform GetEarnedItemBg()
    {
        return earnedItemView.GetRectTransform();
    }

    public CanvasGroup GetEarnedItemBgCanvasGroup()
    {
        return earnedItemView.GetCanvasGroup();
    }
    
    public void InitEarnedItemBg(Vector3 sizeDelta, Vector3 localPos)
    {
        earnedItemView.Init(sizeDelta, localPos + transform.localPosition);
        SetStatusOfEarnedItemBg(false);
    }

    public void SetColorOfEarnedItemBg(ItemType itemType)
    {
        earnedItemView.SetColor(itemType);
    }

    public void SetWheelImage(ZoneType zoneType)
    {
        foreach (var wheelOption in wheelOptions)
        {
            if (zoneType != wheelOption.zoneType) continue;
            image.sprite = wheelOption.wheel;
            indicator.sprite = wheelOption.indicator;
            wheelTitle.SetText(wheelOption.title);
            wheelTitle.color = ConstantValues.HexToColor(wheelOption.color);
            return;
        }
    }

    public RectTransform GetWheelTitle()
    {
        return wheelTitle.rectTransform;
    }
}

public interface IWheelView
{
    RectTransform GetRectTransform();
    void SetWheelImage(ZoneType zoneType);
    void SetStatusOfEarnedItemBg(bool status);
    RectTransform GetEarnedItemBg();
    IGiftItemView CreateGiftObject();
    void InitEarnedItemBg(Vector3 sizeDelta, Vector3 localPos);
    CanvasGroup GetEarnedItemBgCanvasGroup();
    void SetColorOfEarnedItemBg(ItemType itemType);
    RectTransform GetWheelTitle();
}