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

    private LayerMask _shooterLayer;

    private void OnTriggerEnter (Collider collision)
    {
      int collisionLayer = 1 << collision.gameObject.layer;

      if ((collisionLayer & _shooterLayer) != 0) {
        return;
      }

      if ((collisionLayer & TargetLayers) != 0) {
        HandleCollisionWithTarget(collision);

        return;
      }

      OnHit?.Invoke(this);
    }

    public void SetLayers (LayerMask shooter, LayerMask target)
    {
      _shooterLayer = shooter;
      TargetLayers = target;
    }

    private void HandleCollisionWithTarget (Collider collision)
    {
      _eventManager.Fire(new ProjectileHitEvent(collision.gameObject));
      OnHit?.Invoke(this);
    }

    public LayerMask TargetLayers
    {
      get;
      private set;
    }
  }
}