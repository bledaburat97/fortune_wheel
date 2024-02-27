using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseButtonView : MonoBehaviour, IBaseButtonView
{
    [SerializeField] private Button button;

    public void Init(Action onClickAction)
    {
        button.onClick.AddListener(() => onClickAction?.Invoke());
    }
}

public interface IBaseButtonView
{
    void Init(Action onClickAction);
}