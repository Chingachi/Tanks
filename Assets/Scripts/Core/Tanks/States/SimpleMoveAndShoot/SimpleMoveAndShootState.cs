using Core.Player;
using Core.Player.Shooting.Base;
using Core.Projectiles;
using Core.Tanks.States.SimpleMovements;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;
namespace Core.Tanks.States.SimpleMoveAndShoot
{
  public class SimpleMoveAndShootState : SimpleMovementState
  {
    private bool _isOnCooldown;

    public SimpleMoveAndShootState (SimpleMoveAndShootStateContext context)
      : base(context)
    {}

    public override void EnterState()
    {
      base.EnterState();

      SimpleMoveAndShootStateContext context = (SimpleMoveAndShootStateContext)_context;
      context.Player = context.Container.Resolve<PlayerTank>();
    }

    protected override void HandleControls()
    {
      HandleShooting();
      base.HandleControls();
    }

    private void HandleShooting()
    {
      SimpleMoveAndShootStateContext context = (SimpleMoveAndShootStateContext)_context;

      if (context.Player == null || !context.Player.Alive) {
        return;
      }

      Vector3 vectorToPlayer = context.Player.transform.position - context.Transform.position;
      vectorToPlayer.y = 0f;
      vectorToPlayer.Normalize();

      Vector3 forwardVector = context.Transform.forward;
      forwardVector.y = 0f;
      forwardVector.Normalize();

      if (Vector3.Angle(forwardVector, vectorToPlayer) < 15f) {
        Shoot(context);
      }
    }

    private async void Shoot (SimpleMoveAndShootStateContext stateContext)
    {
      if (_isOnCooldown) {
        return;
      }

      HandleCooldown(stateContext);

      ShootingContext context = stateContext.ShootingContext;
      Projectile projectile = context.ProjectilePool.Get();
      projectile.transform.position = context.ShootingAnchor.transform.position;
      projectile.SetLayers(Layers.Enemy, Layers.Player);
      projectile.gameObject.SetActive(true);
      projectile.OnHit += HandleHit;

      Vector3 direction = context.ShootingAnchor.forward.normalized;

      while (projectile != null && projectile.gameObject.activeSelf) {
        projectile.transform.Translate(direction, Space.World);
        await UniTask.WaitForFixedUpdate();
      }
    }

    private async void HandleCooldown (SimpleMoveAndShootStateContext context)
    {
      _isOnCooldown = true;
      await UniTask.WaitForSeconds(context.ShootingContext.CooldownTime);
      _isOnCooldown = false;
    }


    private void HandleHit (Projectile projectile)
    {
      SimpleMoveAndShootStateContext context = (SimpleMoveAndShootStateContext)_context;
      projectile.OnHit -= HandleHit;
      context.ShootingContext.ProjectilePool.Return(projectile);
    }
  }
}