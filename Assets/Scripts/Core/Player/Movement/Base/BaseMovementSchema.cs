using UnityEngine;
namespace Core.Player.Movement.Base
{
  public abstract class BaseMovementSchema<T> : BaseMovementSchema
    where T : MovementContext
  {
    protected Transform _transform;
    protected Rigidbody _rigidbody;
    protected T _context;
    protected GameInputs _inputs;

    protected BaseMovementSchema (T context)
    {
      _transform = context.Tank.transform;
      _rigidbody = context.Tank.GetComponent<Rigidbody>();

      _context = context;

      _inputs = context.Inputs;
      _inputs.Enable();
    }
  }
  public abstract class BaseMovementSchema
  {

    public abstract void HandleInput();

    public abstract void UpdateMovement();
  }
}