using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class GameController : IGameController
{
    private IZoneNumberController _zoneNumberController;
    private IInventory _inventory;
    private IWheelController _wheelController;
    private IGiftItemDeterminer _giftItemDeterminer;
    private List<Image> _animationImagePool = new List<Image>();
    
    public event EventHandler<Action> OnBombExploded;
    
    public void Initialize(IInventory inventory, IWheelController wheelController, IGiftItemDeterminer giftItemDeterminer, IZoneNumberController zoneNumberController, ISpinButtonController spinButtonController)
    {
        _zoneNumberController = zoneNumberController;
        _wheelController = wheelController;
        _inventory = inventory;
        _giftItemDeterminer = giftItemDeterminer;
        UpdateWheelImage();
        _wheelController.BringWheelTitle(0.5f);
        spinButtonController.SpinButtonClicked += Spin;
    }

    private void UpdateWheelImage()
    {
        _wheelController.SetWheelImage(_zoneNumberController.GetCurrentZoneType());
        _wheelController.SetGiftImages(_giftItemDeterminer.GetCurrentZoneItems(_zoneNumberController.GetZoneNumber(),_zoneNumberController.GetCurrentZoneType()));
    }

    private void Spin(object sender, EventArgs args)
    {
        int index = Random.Range(0, 8);
        _wheelController.Spin(index, () => OnSelectedItem(index));
    }
    
    private void OnSelectedItem(int index)
    {
        Item item = _giftItemDeterminer.GetItem(index);
        
        //Duplicate selected gift object to scale up.
        IGiftItemView giftItemView = _wheelController.DuplicateSelectedGiftObject(item);
        
        //Increment zone number.
        _zoneNumberController.IncrementZoneNumber();

        if (item.itemType != ItemType.Bomb)
        {
            _inventory.TryCreateItem(item);
            DOTween.Sequence().Append(_wheelController.ScaleUpEarnedItem(giftItemView, 2f, item.itemType, out float scaleRatioOfImage))
                .Append(CreateAnimationImage(giftItemView.GetImage(), item, Vector3.one / scaleRatioOfImage, 1f))
                .AppendCallback(UpdateWheelImage)
                .Append(_wheelController.CloseEarnedItemBg(giftItemView, 1f));
        }

        else
        {
            DOTween.Sequence().Append(_wheelController.ScaleUpEarnedItem(giftItemView, 0.5f, item.itemType, out float scaleRatioOfImage))
                .AppendCallback(() =>
                {
                    UpdateWheelImage();
                    OnBombExploded?.Invoke(this, () => _wheelController.CloseEarnedItemBg(giftItemView, 1f));
                });
        }
    }
    
    //An image of earned item is created an moves toward to inventory.
    private Sequence CreateAnimationImage(Image parentImage, Item item, Vector2 localScale, float duration)
    {
        Image animationImage = GetOrCreateImageFromPool();
        RectTransform animationImageRectTransform = animationImage.rectTransform;
        RectTransform newParent = _inventory.GetInventoryItemView(item.itemType).GetImageRectTransform();
        return DOTween.Sequence()
            .OnStart(() =>
            {
                animationImageRectTransform.sizeDelta = newParent.sizeDelta;
                animationImage.sprite = parentImage.sprite;
                animationImageRectTransform.SetParent(parentImage.rectTransform);
                animationImageRectTransform.localPosition = Vector3.zero;
                animationImageRectTransform.localScale = localScale;
                animationImageRectTransform.SetParent(newParent);
            })
            .Append(animationImage.rectTransform.DOLocalMove(new Vector3(-newParent.sizeDelta.x / 2, 0, 0), duration))
            .OnComplete(() =>
            {
                _inventory.AddItem(item);
                animationImage.gameObject.SetActive(false);
            });
    }
    
    private Image GetOrCreateImageFromPool()
    {
        foreach (Image obj in _animationImagePool)
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
        _animationImagePool.Add(newImage);
        return newImage;
    }
}

public interface IGameController
{
    void Initialize(IInventory inventory, IWheelController wheelController, IGiftItemDeterminer giftItemDeterminer, IZoneNumberController zoneNumberController, ISpinButtonController spinButtonController);
    event EventHandler<Action> OnBombExploded;
}
