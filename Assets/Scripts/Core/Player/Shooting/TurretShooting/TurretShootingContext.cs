using System;
using Core.Player.Shooting.Base;
using UnityEngine;
namespace Core.Player.Shooting.TurretShooting
{
  [Serializable]
  public class TurretShootingContext : ShootingContext
  {
    public GameObject Turret;
    public LayerMask GroundLayer;
  }
}