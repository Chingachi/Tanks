using EventSystemComponents;
namespace Core.Tanks.Events
{
  public class DestroyAiTankEvent : BaseEvent
  {
    public string Id;

    public DestroyAiTankEvent (string id)
    {
      Id = id;
    }
  }
}