using UnityEngine;
using UnityEngine.UI;

public class GiftItemView : ItemView, IGiftItemView
{
    [SerializeField] private RectTransform rectTransform;
    public void SetLocalPosition(Vector3 localPos)
    {
        transform.localPosition = localPos;
    }

    public void SetRotationAngleZ(float rotationAngle)
    {
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
    }

    public void SetSize(float size)
    {
        rectTransform.sizeDelta = new Vector3(size, size, 0);
    }

    public RectTransform GetRectTransform()
    {
        return rectTransform;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public override void SetText(int amount)
    {
        if (amount > 0) base.SetText(amount);
        else text.text = "";
    }

    public Image GetImage()
    {
        return image;
    }
}

public interface IGiftItemView : IItemView
{
    void SetLocalPosition(Vector3 localPos);
    void SetRotationAngleZ(float rotationAngle);
    void SetSize(float size);
    RectTransform GetRectTransform();
    void Destroy();
    Image GetImage();
}
