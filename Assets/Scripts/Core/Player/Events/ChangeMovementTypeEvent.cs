using System;
using EventSystemComponents;
namespace Core.Player.Events
{
  public class ChangeMovementTypeEvent : BaseEvent
  {
    public Type MovementType;

    public ChangeMovementTypeEvent (Type movementType)
    {
      MovementType = movementType;
    }
  }
}