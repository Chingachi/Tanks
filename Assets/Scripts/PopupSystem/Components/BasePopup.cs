using System;
using UnityEngine;
using UnityEngine.UI;
namespace Core.PopupSystem.Components
{
  public abstract class BasePopup : MonoBehaviour
  {
    public event Action OnClose;

    [SerializeField]
    protected Button _closeButton;


    protected virtual void Awake()
    {
      if (_closeButton != null) {
        _closeButton.onClick.AddListener(Close);
      }
    }

    public abstract void SetData (IPopupData data);

    public virtual void Show()
    {}

    public virtual void Close()
    {
      OnClose?.Invoke();
    }
  }
}