using Core.Player.Movement.Base;
using UnityEngine;
namespace Core.Player.Movement
{
  public class TrackMovementSchema : BaseMovementSchema<MovementContext>
  {
    private float _leftTrackValue;
    private float _rightTrackValue;

    public TrackMovementSchema (MovementContext context)
      : base(context)
    {}

    public override void HandleInput()
    {
      // controls by Q(forward) and A(backwards)
      _leftTrackValue = _inputs.DefaultActions.LeftTrack.ReadValue<float>();
      // controls by E(forward) and D(backwards)
      _rightTrackValue = _inputs.DefaultActions.RightTrack.ReadValue<float>();
    }

    public override void UpdateMovement()
    {
      if (Mathf.Approximately(_leftTrackValue, 0f) && Mathf.Approximately(_rightTrackValue, 0f)) {
        return;
      }



      Quaternion rotation = _transform.rotation * Quaternion.Euler(0, GetRotation() * _context.RotationValue, 0);
      Quaternion newRotation = Quaternion.RotateTowards(_rigidbody.rotation, rotation, _context.RotationSpeed * Time.fixedDeltaTime);
      _rigidbody.MoveRotation(newRotation);


      _rigidbody.MovePosition(_rigidbody.position + GetMoveDirection(newRotation) * _context.MoveSpeed * Time.fixedDeltaTime);
    }

    private float GetRotation()
    {
      if (Mathf.Abs(_leftTrackValue) > 0 && Mathf.Abs(_rightTrackValue) > 0) {
        return 0;
      }

      if (_leftTrackValue > 0) {
        return _rightTrackValue < 0 ? 1 : 0.5f;
      }

      if (_leftTrackValue < 0) {
        return _rightTrackValue > 0 ? -1 : -0.5f;
      }

      if (_rightTrackValue > 0) {
        return _leftTrackValue < 0 ? -1 : -0.5f;
      }

      if (_rightTrackValue < 0) {
        return _leftTrackValue > 0 ? 1 : 0.5f;
      }

      return 0;
    }

    private Vector3 GetMoveDirection (Quaternion newRotation)
    {
      bool moveForward = _leftTrackValue > 0 && _rightTrackValue > 0;
      bool moveBackwards = _leftTrackValue < 0 && _rightTrackValue < 0;
      Vector3 direction = newRotation * Vector3.forward;

      if (moveBackwards) {
        direction *= -1;
      } else if (!moveForward) {
        direction *= 0;
      }

      return direction;
    }
  }
}