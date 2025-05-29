using System;
using Core.Player;
using Core.Player.Shooting.Base;
using Core.Tanks.States.SimpleMovements;
using Zenject;
namespace Core.Tanks.States.SimpleMoveAndShoot
{
  [Serializable]
  public class SimpleMoveAndShootStateContext : SimpleMovementStateContext
  {
    public PlayerTank Player;
    public ShootingContext ShootingContext;
    public DiContainer Container;
  }
}