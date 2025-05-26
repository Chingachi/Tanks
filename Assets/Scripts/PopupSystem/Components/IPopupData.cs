using System;
namespace Core.PopupSystem.Components
{
  public interface IPopupData
  {
    Type GetPopupType();

    Action<BasePopup> Callback { get; set; }
  }
}