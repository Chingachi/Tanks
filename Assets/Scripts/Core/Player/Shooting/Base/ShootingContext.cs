using System;
using Core.MonoPool;
using Core.Projectiles;
using UnityEngine;
namespace Core.Player.Shooting.Base
{
  [Serializable]
  public class ShootingContext
  {
    public SimpleMonoObjectPool<Projectile> ProjectilePool;
    public GameInputs Inputs;
    public Transform ShootingAnchor;
    public float CooldownTime;
  }
}