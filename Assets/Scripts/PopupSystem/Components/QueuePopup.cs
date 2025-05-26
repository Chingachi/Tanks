namespace Core.PopupSystem.Components
{
  public class QueuePopup
  {

    public QueuePopup (BasePopup instance, IPopupData data)
    {
      Instance = instance;
      Data = data;
    }

    public IPopupData Data
    {
      get;
    }
    public BasePopup Instance
    {
      get;
    }
  }
}