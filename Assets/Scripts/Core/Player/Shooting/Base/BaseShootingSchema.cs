using Core.Projectiles;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
namespace Core.Player.Shooting.Base
{
  public abstract class BaseShootingSchema<T> : BaseShootingSchema
    where T : ShootingContext
  {
    protected T _context;

    public BaseShootingSchema (T context)
    {
      _context = context;
      _context.Inputs.DefaultActions.Fire.performed += HandleShootingTrigger;
    }

    public override void Deactivate()
    {
      _context.Inputs.DefaultActions.Fire.performed -= HandleShootingTrigger;
    }

    protected override void HandleShootingTrigger (InputAction.CallbackContext inputCallback)
    {
      if (!_context.Player.Alive) {
        return;
      }

      Projectile projectile = _context.ProjectilePool.Get();
      projectile.transform.position = _context.ShootingAnchor.transform.position;
      projectile.SetLayers(Layers.Player, Layers.Enemy);
      projectile.gameObject.SetActive(true);
      projectile.OnHit += HandleHit;
      Shoot(projectile);
    }

    private async void Shoot (Projectile projectile)
    {
      Vector3 direction = _context.ShootingAnchor.forward.normalized;

      while (projectile != null && projectile.gameObject.activeSelf) {
        projectile.transform.Translate(direction, Space.World);
        await UniTask.WaitForFixedUpdate();
      }
    }

    private void HandleHit (Projectile projectile)
    {
      projectile.OnHit -= HandleHit;
      _context.ProjectilePool.Return(projectile);
    }
  }
  public abstract class BaseShootingSchema
  {

    public abstract void Deactivate();

    protected abstract void HandleShootingTrigger (InputAction.CallbackContext inputCallback);
  }
}