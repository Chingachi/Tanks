using Core.Player.Movement.Base;
using UnityEngine;
namespace Core.Player.Movement
{
  public class ClassicMovementSchema : BaseMovementSchema<MovementContext>
  {
    private Vector2 _input;

    public ClassicMovementSchema (MovementContext context)
      : base(context)
    {}

    public override void HandleInput()
    {
      _input = _inputs.DefaultActions.Movement.ReadValue<Vector2>();
    }

    public override void UpdateMovement()
    {
      if (_input.sqrMagnitude < 0.001f) {
        return;
      }

      Quaternion rotation = _transform.rotation * Quaternion.Euler(0, _input.x * _context.RotationValue, 0);
      Quaternion newRotation = Quaternion.RotateTowards(_rigidbody.rotation, rotation, _context.RotationSpeed * Time.fixedDeltaTime);
      _rigidbody.MoveRotation(newRotation);

      Vector3 direction = newRotation * new Vector3(0, 0, _input.y);
      _rigidbody.MovePosition(_rigidbody.position + direction * _context.MoveSpeed * Time.fixedDeltaTime);
    }
  }
}