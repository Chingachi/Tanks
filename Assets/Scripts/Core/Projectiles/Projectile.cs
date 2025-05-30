using System;
using EventSystemComponents;
using UnityEngine;
using Zenject;
namespace Core.Projectiles
{
  public class Projectile : MonoBehaviour
  {
    public event Action<Projectile> OnHit;

    [Inject]
    private EventManager _eventManager;


    private void OnTriggerEnter (Collider collision)
    {

      if (collision.gameObject.layer == ShooterLayer) {
        return;
      }

      if (collision.gameObject.layer == TargetLayers) {
        HandleCollisionWithTarget(collision);

        return;
      }

      OnHit?.Invoke(this);
    }

    public void SetLayers (int shooter, int target)
    {
      ShooterLayer = shooter;
      TargetLayers = target;
    }

    private void HandleCollisionWithTarget (Collider collision)
    {
      _eventManager.Fire(new ProjectileHitEvent(collision.gameObject));
      OnHit?.Invoke(this);
    }

    public int TargetLayers
    {
      get;
      private set;
    }
    public int ShooterLayer
    {
      get;
      private set;
    }
  }
}