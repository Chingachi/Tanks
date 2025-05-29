using UnityEngine;
namespace Core.Player.Movement
{
  public abstract class BaseMovementSchema
  {
    protected Transform _transform;
    protected Rigidbody _rigidbody;
    protected float _moveSpeed;
    protected float _rotationSpeed;
    protected float _rotationValue;
    protected GameInputs _inputs;

    protected BaseMovementSchema (GameObject gameObject, float moveSpeed, float rotationSpeed, float rotationValue, GameInputs inputs)
    {
      _transform = gameObject.transform;
      _rigidbody = gameObject.GetComponent<Rigidbody>();

      _moveSpeed = moveSpeed;
      _rotationSpeed = rotationSpeed;
      _rotationValue = rotationValue;
      _inputs = inputs;

      _inputs.Enable();
    }

    public abstract void HandleInput();

    public abstract void UpdateMovement();
  }
}