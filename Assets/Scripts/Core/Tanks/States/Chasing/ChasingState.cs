using System;
using Core.Tanks.States.Base;
using UnityEngine;
namespace Core.Tanks.States.Chasing
{
  public class ChasingState : BaseTankState<ChasingStateContext>
  {
    public event Action OnTargetDied;

    public ChasingState (ChasingStateContext context)
      : base(context)
    {}

    public override void EnterState()
    {}

    public override void UpdateState()
    {
      if (_context.Target == null) {
        return;
      }

      if (!_context.Target.Alive) {
        OnTargetDied?.Invoke();

        return;
      }

      Vector3 targetRotation = _context.Target.transform.position - _context.Transform.position;
      targetRotation.y = 0f;
      targetRotation.Normalize();

      Quaternion lookRotation = Quaternion.LookRotation(targetRotation);
      Quaternion newRotation = Quaternion.RotateTowards(_context.Rigidbody.rotation, lookRotation, _context.RotationSpeed * Time.fixedDeltaTime);
      _context.Rigidbody.MoveRotation(newRotation);

      Vector3 direction = newRotation * Vector3.forward;
      _context.Rigidbody.MovePosition(_context.Rigidbody.position + direction * _context.MoveSpeed * Time.fixedDeltaTime);
    }

    public override void ExitState()
    {}
  }
}