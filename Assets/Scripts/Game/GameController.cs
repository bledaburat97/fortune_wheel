using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class GameController : IGameController
{
    public event EventHandler<Action> OnBombExploded;
    private IZoneNumberController _zoneNumberController;
    private IInventory _inventory;
    private IWheelController _wheelController;
    private IGiftItemDeterminer _giftItemDeterminer;
    private List<Image> animationImagePool = new List<Image>();

    public void Initialize(IInventory inventory, IWheelController wheelController, IGiftItemDeterminer giftItemDeterminer, IZoneNumberController zoneNumberController)
    {
        _zoneNumberController = zoneNumberController;
        _wheelController = wheelController;
        _inventory = inventory;
        _giftItemDeterminer = giftItemDeterminer;
        _wheelController.SetWheelImage(_zoneNumberController.GetCurrentZoneType());
        _wheelController.SetGiftImages(_giftItemDeterminer.GetCurrentZoneItems(_zoneNumberController.GetZoneNumber(),_zoneNumberController.GetCurrentZoneType()));
        _wheelController.BringWheelTitle(0.5f);
    }

    public void Spin()
    {
        int index = Random.Range(0, 8);
        _wheelController.Spin(index, () => OnSelectedItem(index));
    }

    private Sequence CreateAnimationImage(Image parentImage, RectTransform newParent, Vector2 localScale, float duration)
    {
        Image animationImage = GetOrCreateObjectFromPool();
        RectTransform animationImageRectTransform = animationImage.rectTransform;
        return DOTween.Sequence()
            .AppendCallback(() =>
            {
                animationImageRectTransform.sizeDelta = newParent.sizeDelta;
                animationImage.sprite = parentImage.sprite;
                animationImageRectTransform.SetParent(parentImage.rectTransform);
                animationImageRectTransform.localPosition = Vector3.zero;
                animationImageRectTransform.localScale = localScale;
                animationImageRectTransform.SetParent(newParent);
            })
            .Append(animationImage.rectTransform.DOLocalMove(new Vector3(-newParent.sizeDelta.x / 2, 0, 0), duration))
            .OnComplete(() => animationImage.gameObject.SetActive(false));
    }
    
    private void OnSelectedItem(int index)
    {
        Item item = _giftItemDeterminer.GetItem(index);
        IGiftItemView giftItemView = _wheelController.DuplicateSelectedGiftObject(item);
        
        _zoneNumberController.IncrementZoneNumber();

        if (item.itemType != ItemType.Bomb)
        {
            _inventory.TryCreateItem(item);
            DOTween.Sequence().Append(_wheelController.ScaleUpEarnedItem(giftItemView, 2f, item.itemType, out float scaleRatioOfImage))
                .Append(CreateAnimationImage(giftItemView.GetImage(), _inventory.GetInventoryItemView(item.itemType).GetImageRectTransform(), Vector3.one / scaleRatioOfImage, 1f))
                .AppendCallback(() =>
            {
                _inventory.AddItem(item);
                _wheelController.UpdateWheel(_zoneNumberController.GetCurrentZoneType(),
                    _giftItemDeterminer.GetCurrentZoneItems(_zoneNumberController.GetZoneNumber(),
                        _zoneNumberController.GetCurrentZoneType()));
            }).Append(_wheelController.CloseEarnedItemBg(giftItemView, 1f));
        }

        else
        {
            DOTween.Sequence().Append(_wheelController.ScaleUpEarnedItem(giftItemView, 0.5f, item.itemType, out float scaleRatioOfImage))
                .AppendCallback(() =>
                {
                    _wheelController.UpdateWheel(_zoneNumberController.GetCurrentZoneType(),
                        _giftItemDeterminer.GetCurrentZoneItems(_zoneNumberController.GetZoneNumber(),
                            _zoneNumberController.GetCurrentZoneType()));
                    OnBombExploded?.Invoke(this, () => _wheelController.CloseEarnedItemBg(giftItemView, 1f));
                });
        }
    }
    
    private Image GetOrCreateObjectFromPool()
    {
        foreach (Image obj in animationImagePool)
        {
            if (!obj.gameObject.activeSelf)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = new GameObject();
        Image newImage = newObj.AddComponent<Image>();

        newObj.SetActive(true);
        animationImagePool.Add(newImage);
        return newImage;
    }
}

public interface IGameController
{
    void Spin();
    void Initialize(IInventory inventory, IWheelController wheelController, IGiftItemDeterminer giftItemDeterminer, IZoneNumberController zoneNumberController);
    event EventHandler<Action> OnBombExploded;
}
