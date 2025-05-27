using UnityEngine;
namespace Core.Tanks.AI
{
  public class SimpleAi : BaseTankAi
  {
    [SerializeField]
    protected float _moveSpeed = 3f;
    [SerializeField]
    protected float _rotationSpeed = 15f;
    [SerializeField]
    protected float _minChangeInterval = 2f;
    [SerializeField]
    protected float _maxChangeInterval = 5f;
    [SerializeField]
    protected float _maxTurnAngle = 120f;
    [SerializeField]
    protected LayerMask _ignoredLayers;

    protected float _changeTimer;
    protected Quaternion _targetRotation;

    protected virtual void OnEnable()
    {
      ResetTimer();
    }

    protected virtual void FixedUpdate()
    {
      HandleControls();
    }

    protected virtual void OnCollisionEnter (Collision collision)
    {
      TryAvoid(collision);
    }

    protected virtual void HandleControls()
    {
      HandleRotationChange();
      HandleMovement();
      _changeTimer -= Time.fixedDeltaTime;
    }

    protected virtual void OnCollisionStay (Collision collision)
    {
      TryAvoid(collision);
    }

    private void HandleMovement()
    {
      Quaternion newRotation = Quaternion.RotateTowards(_rigidbody.rotation, _targetRotation, _rotationSpeed * Time.fixedDeltaTime);
      _rigidbody.MoveRotation(newRotation);

      Vector3 direction = newRotation * Vector3.forward;
      _rigidbody.MovePosition(_rigidbody.position + direction * _moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleRotationChange()
    {
      if (_changeTimer > 0) {
        return;
      }

      float angle = Random.Range(-_maxTurnAngle, _maxTurnAngle);
      _targetRotation = transform.rotation * Quaternion.Euler(0f, angle, 0f);
      ResetTimer();
    }

    private void TryAvoid (Collision collision)
    {
      if (((1 << collision.gameObject.layer) & _ignoredLayers) != 0) {
        return;
      }

      HandleDestruction(); // TODO: Handle destruction only on fire collision

      return;

      Vector3 normal = collision.contacts[0].normal;
      normal.y = 0f;
      _targetRotation = Quaternion.LookRotation(normal);

      ResetTimer();
    }

    private void ResetTimer()
    {
      _changeTimer = Random.Range(_minChangeInterval, _maxChangeInterval);
    }
  }
}