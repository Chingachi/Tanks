using System;
using System.Threading;
using Core.Player.Shooting.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Core.Player.Shooting.TurretShooting
{
  public class TurretShootingSchema : BaseShootingSchema<TurretShootingContext>
  {

    public TurretShootingSchema (TurretShootingContext context)
      : base(context)
    {
      RunTurret(_context.Turret.gameObject.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid RunTurret (CancellationToken token)
    {
      Camera camera = Camera.main;

      try {
        while (!token.IsCancellationRequested) {
          Vector2 mousePosition = Mouse.current.position.ReadValue();
          Ray ray = camera.ScreenPointToRay(mousePosition);

          if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _context.GroundLayer)) {
            Vector3 aimPoint = hit.point;
            RotateTurret(aimPoint);
          }

          await UniTask.WaitForFixedUpdate(token);
        }
      } catch (OperationCanceledException) {}
    }

    private void RotateTurret (Vector3 aimPoint)
    {
      Vector3 direction = aimPoint - _context.Turret.transform.position;
      direction.y = 0f;

      if (direction.sqrMagnitude < 0.001f) {
        return;
      }

      _context.Turret.transform.rotation = Quaternion.LookRotation(direction);
    }
  }
}