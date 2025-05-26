using Core.PopupSystem.Components;
using EventSystemComponents;
using PopupSystem;
namespace Core.PopupSystem
{
  public abstract class BasePopupViewManager<TData, TPopup>
    where TData : IPopupData where TPopup : BasePopup
  {
    protected readonly PopupManager _popupManager;
    protected readonly EventManager _eventManager;

    protected TData _data;
    protected TPopup _popup;

    protected BasePopupViewManager (PopupManager popupManager, EventManager eventManager)
    {
      _popupManager = popupManager;
      _eventManager = eventManager;
    }

    public virtual void OpenPopup (TData data)
    {
      _data = data;
      data.Callback += HandleLoadedPopup;
      _popupManager.OpenPopup(data);
    }

    public virtual void ClosePopup()
    {
      _popup.Close();
    }

    protected virtual void HandleLoadedPopup (BasePopup popup)
    {
      _popup = (TPopup)popup;
      _popup.OnClose += HandleClose;
      HandleLoadedPopup();
    }

    protected abstract void HandleLoadedPopup();

    protected virtual void HandleClose()
    {}
  }
}