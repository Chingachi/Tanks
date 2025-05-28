using Core.Tanks.States.Base;
using UnityEngine;
namespace Core.Tanks.States.SimpleMovements
{
  public class SimpleMovementState : TankState<SimpleMovementStateContext>
  {
    protected float _changeTimer;
    protected Quaternion _targetRotation;

    public SimpleMovementState (SimpleMovementStateContext context)
      : base(context)
    {}

    public override void EnterState()
    {
      _context.OnCollisionEnter += TryAvoid;
      _context.OnCollisionStay += TryAvoid;
    }

    public override void UpdateState()
    {
      HandleControls();
    }

    public override void ExitState()
    {
      _context.OnCollisionEnter -= TryAvoid;
      _context.OnCollisionStay -= TryAvoid;
    }

    private void HandleControls()
    {
      HandleRotationChange();
      HandleMovement();
      _changeTimer -= Time.fixedDeltaTime;
    }

    private void HandleMovement()
    {
      Quaternion newRotation = Quaternion.RotateTowards(_context.Rigidbody.rotation, _targetRotation, _context.RotationSpeed * Time.fixedDeltaTime);
      _context.Rigidbody.MoveRotation(newRotation);

      Vector3 direction = newRotation * Vector3.forward;
      _context.Rigidbody.MovePosition(_context.Rigidbody.position + direction * _context.MoveSpeed * Time.fixedDeltaTime);
    }

    private void HandleRotationChange()
    {
      if (_changeTimer > 0) {
        return;
      }

      float angle = Random.Range(-_context.MaxTurnAngle, _context.MaxTurnAngle);
      _targetRotation = _context.Transform.rotation * Quaternion.Euler(0f, angle, 0f);
      ResetTimer();
    }

    private void TryAvoid (Collision collision)
    {
      if (((1 << collision.gameObject.layer) & _context.IgnoredLayers) != 0) {
        return;
      }

      // HandleDestruction(); // TODO: Handle destruction only on fire collision
      //
      // return;

      Vector3 normal = collision.contacts[0].normal;
      normal.y = 0f;
      _targetRotation = Quaternion.LookRotation(normal);

      ResetTimer();
    }

    private void ResetTimer()
    {
      _changeTimer = Random.Range(_context.MinChangeInterval, _context.MaxChangeInterval);
    }
  }
}