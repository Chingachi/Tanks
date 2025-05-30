using System;
using EventSystemComponents;
namespace Core.Player.Events
{
  public class ChangeShootingTypeEvent : BaseEvent
  {
    public Type ShootingType;

    public ChangeShootingTypeEvent (Type shootingType)
    {
      ShootingType = shootingType;
    }
  }
}