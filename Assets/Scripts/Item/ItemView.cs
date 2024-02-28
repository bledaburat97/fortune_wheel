using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour, IItemView
{
    [SerializeField] protected Image image;
    [SerializeField] protected TMP_Text text;
    private Vector2 _initialSizeOfImage;
    
    public virtual void Init()
    {
        _initialSizeOfImage = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
        transform.localScale = Vector3.one;
    }
    
    //Set Image size according to width/height ratio of the icon.
    public virtual void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
        float maxWidth = _initialSizeOfImage.x;
        float maxHeight = _initialSizeOfImage.y;
        
        float spriteWidth = sprite.bounds.size.x;
        float spriteHeight = sprite.bounds.size.y;

        float widthRatio = maxWidth / spriteWidth;
        float heightRatio = maxHeight / spriteHeight;

        if (widthRatio > heightRatio)
        {
            image.rectTransform.sizeDelta = new Vector2(spriteWidth * heightRatio, spriteHeight * heightRatio);
        }

        else
        {
            image.rectTransform.sizeDelta = new Vector2(spriteWidth * widthRatio, spriteHeight * widthRatio);
        }
    }

    public virtual void SetText(int amount)
    {
        text.text = "x" + amount;
    }

    public RectTransform GetImageRectTransform()
    {
        return image.rectTransform;
    }
}

public interface IItemView
{
    void Init();
    void SetImage(Sprite sprite);
    void SetText(int amount);
    RectTransform GetImageRectTransform();
}
