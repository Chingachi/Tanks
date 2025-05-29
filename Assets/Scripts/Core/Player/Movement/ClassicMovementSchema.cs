using UnityEngine;
namespace Core.Player.Movement
{
  public class ClassicMovementSchema : BaseMovementSchema
  {
    private Vector2 _input;

    public ClassicMovementSchema (GameObject gameObject, float moveSpeed, float rotationSpeed, float rotationValue, GameInputs inputs)
      : base(gameObject, moveSpeed, rotationSpeed, rotationValue, inputs)
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

      Quaternion rotation = _transform.rotation * Quaternion.Euler(0, _input.x * _rotationValue, 0);
      Quaternion newRotation = Quaternion.RotateTowards(_rigidbody.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
      _rigidbody.MoveRotation(newRotation);

      Vector3 direction = newRotation * new Vector3(0, 0, _input.y);
      _rigidbody.MovePosition(_rigidbody.position + direction * _moveSpeed * Time.fixedDeltaTime);
    }
  }
}