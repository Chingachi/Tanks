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
    private LayerMask _launchLayer;
    private LayerMask _targetLayers;

    private void OnTriggerEnter (Collider collision)
    {
      int collisionLayer = 1 << collision.gameObject.layer;

      if ((collisionLayer & _launchLayer) != 0) {
        return;
      }

      if ((collisionLayer & _targetLayers) != 0) {
        HandleCollisionWithTarget(collision);

        return;
      }

      OnHit?.Invoke(this);
    }

    public void SetLaunchLayer (LayerMask launchLayer, LayerMask targetLayer)
    {
      _launchLayer = launchLayer;
      _targetLayers = targetLayer;
    }

    private void HandleCollisionWithTarget (Collider collision)
    {
      _eventManager.Fire(new ProjectileHitEvent(collision.gameObject));
      OnHit?.Invoke(this);
    }
  }
}