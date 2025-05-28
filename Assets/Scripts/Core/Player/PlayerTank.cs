using System;
using UnityEngine;
using Zenject;
namespace Core.Player
{
  [RequireComponent(typeof(Rigidbody))]
  public class PlayerTank : MonoBehaviour
  {
    public event Action OnDeath;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private LayerMask _enemiesLayerMask;


    [Inject]
    private GameInputs _inputs;

    private Rigidbody _rigidbody;

    private void Awake()
    {
      _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
      HandleInput();
    }

    private void OnCollisionEnter (Collision collision)
    {
      if (((1 << collision.gameObject.layer) & _enemiesLayerMask) == 0) {
        return;
      }

      Alive = false;
      OnDeath?.Invoke();
    }

    public void HandleSpawn()
    {
      Alive = true;
    }

    private void HandleInput()
    {
      Vector2 input = _inputs.DefaultActions.Movement.ReadValue<Vector2>();

      if (input.sqrMagnitude < 0.0001f) {}
    }

    public bool Alive
    {
      get;
      private set;
    }
  }
}