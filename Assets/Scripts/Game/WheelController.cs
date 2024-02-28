﻿using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WheelController : IWheelController
{
    private IWheelView _view;
    private bool _isSpinning;
    private readonly float _sliceAngleInDegree;
    private readonly float _sliceAngleInRadian;
    private const float SpinDuration = 5f;
    private Dictionary<ItemType, Sprite> _itemTypeToIconDict;
    private List<IGiftItemView> _giftItemViews = new List<IGiftItemView>();
    private const float distanceToCenterRatio = 0.3f;
    private const float giftItemSizeRatio = 0.13f;
    private float _sizeOfWheel;
    private Vector2 _centerOfWheel;
    private float _distanceToCenter;
    private const float WheelTitleMovementDuration = 0.5f;
    private const float InnerImageScaleRatio = 0.7f;
    private const float NumberOfLaps = 4;
    
    public WheelController()
    {
        _sliceAngleInDegree = 360f / ConstantValues.NumberOfSlices;
        _sliceAngleInRadian = 2 * Mathf.PI / ConstantValues.NumberOfSlices;
    }
    
    public void Initialize(IWheelView view, Dictionary<ItemType, Sprite> itemTypeToIconDict)
    {
        _view = view;
        _itemTypeToIconDict = itemTypeToIconDict;
        CalculateWheelProperties();
        CreateGiftObjects();
    }
    
    private void CalculateWheelProperties()
    {
        _sizeOfWheel = _view.GetRectTransform().rect.width;
        _centerOfWheel = new Vector2(0f, 0f);
        _distanceToCenter = distanceToCenterRatio * _sizeOfWheel;
    }

    public bool IsSpinning()
    {
        return _isSpinning;
    }
    
    public void Spin(int index, Action onCompleteAction)
    {
        if (!_isSpinning)
        {
            _isSpinning = true;
            float angle = -(_sliceAngleInDegree * index);
            Vector3 targetRotation = Vector3.back * (angle + NumberOfLaps * 360);
            SendWheelTitleBack(WheelTitleMovementDuration);
            _view.GetRectTransform()
                .DORotate(targetRotation, SpinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutQuart)
                .OnComplete(() =>
                {
                    onCompleteAction?.Invoke();
                    _isSpinning = false;
                });
        }
    }

    public Sequence ScaleUpEarnedItem(IGiftItemView giftItemView, float duration, ItemType itemType, out float scaleRatioOfImage)
    {
        _view.SetStatusOfEarnedItemBg(true);
        _view.SetColorOfEarnedItemBg(itemType);
        float scaleRatio = _view.GetRectTransform().rect.width / _view.GetEarnedItemBg().rect.width;
        float innerImageScaleRatio = itemType == ItemType.Bomb ? 1f : InnerImageScaleRatio;
        scaleRatioOfImage = scaleRatio * InnerImageScaleRatio;
        return DOTween.Sequence().Append(_view.GetEarnedItemBg().DOScale(scaleRatio, duration))
            .Join(giftItemView.GetRectTransform().DOScale(innerImageScaleRatio, duration))
            .Join(_view.GetEarnedItemBg().DOLocalMove(_view.GetRectTransform().localPosition, duration));
    }

    public Sequence CloseEarnedItemBg(IGiftItemView giftItemView, float duration)
    {
        CanvasGroup earnedItemBgCanvasGroup = _view.GetEarnedItemBgCanvasGroup();
        RectTransform earnedItemBg = _view.GetEarnedItemBg();
        return DOTween.Sequence().Append(earnedItemBgCanvasGroup.DOFade(0f, duration))
            .OnComplete(() =>
            {
                giftItemView.Destroy();
                earnedItemBg.gameObject.SetActive(false);
                earnedItemBg.localScale = Vector3.one;
                earnedItemBg.localPosition =
                    new Vector3(_centerOfWheel.x, _centerOfWheel.y + _distanceToCenter);
                earnedItemBgCanvasGroup.alpha = 1f;
                BringWheelTitle(WheelTitleMovementDuration);
            });
    }

    //Set icons of items on wheel.
    public void SetGiftImages(List<Item> items)
    {
        for (int i = 0; i < items.Count ; i++)
        {
            _giftItemViews[i].SetImage(_itemTypeToIconDict[items[i].itemType]);
            _giftItemViews[i].SetText(items[i].amount);
        }
    }

    public void SetWheelImage(ZoneType zoneType)
    {
        _view.SetWheelImage(zoneType);
    }

    //Create item objects on wheel.
    private void CreateGiftObjects()
    {
        float size = giftItemSizeRatio * _sizeOfWheel;
        for (int i = 0; i < ConstantValues.NumberOfSlices; i++)
        {
            float angle = i * _sliceAngleInRadian;

            float posX = _centerOfWheel.x + _distanceToCenter * Mathf.Sin(angle);
            float posY = _centerOfWheel.y + _distanceToCenter * Mathf.Cos(angle);
            IGiftItemView giftItemView = _view.CreateGiftObject();
            giftItemView.Init();
            giftItemView.SetSize(size);
            giftItemView.SetLocalPosition(new Vector3(posX, posY, 0f));
            giftItemView.SetRotationAngleZ((ConstantValues.NumberOfSlices - i) * _sliceAngleInDegree);
            _giftItemViews.Add(giftItemView);
        }
        
        _view.InitEarnedItemBg(new Vector3(size,size, 0f), new Vector3(_centerOfWheel.x, _centerOfWheel.y + _distanceToCenter, 0f));
    }
    
    public IGiftItemView DuplicateSelectedGiftObject(Item item)
    {
        float sizeOfWheel = _view.GetRectTransform().rect.width;
        IGiftItemView giftItemView = _view.CreateGiftObject();
        giftItemView.GetRectTransform().SetParent(_view.GetEarnedItemBg());
        giftItemView.Init();
        giftItemView.SetSize(giftItemSizeRatio * sizeOfWheel);
        giftItemView.SetLocalPosition(Vector3.zero);
        giftItemView.SetImage(_itemTypeToIconDict[item.itemType]);
        giftItemView.SetText(item.amount);
        giftItemView.SetRotationAngleZ(0);
        return giftItemView;
    }
    
    public void BringWheelTitle(float duration)
    {
        DOTween.Sequence().Append(_view.GetWheelTitle().DOLocalMoveY(140, duration)).SetEase(Ease.InQuad);
    }

    private void SendWheelTitleBack(float duration)
    {
        DOTween.Sequence().Append(_view.GetWheelTitle().DOLocalMoveY(280, duration)).SetEase(Ease.InQuad);
    }
}

public interface IWheelController
{
    void Initialize(IWheelView view, Dictionary<ItemType, Sprite> itemTypeToIconDict);
    bool IsSpinning();
    void Spin(int index, Action onCompleteAction);
    void SetWheelImage(ZoneType zoneType);
    void SetGiftImages(List<Item> items);
    IGiftItemView DuplicateSelectedGiftObject(Item item);
    Sequence ScaleUpEarnedItem(IGiftItemView giftItemView, float duration, ItemType itemType, out float scaleRatioOfImage);
    Sequence CloseEarnedItemBg(IGiftItemView giftItemView, float duration);
    void BringWheelTitle(float duration);
}