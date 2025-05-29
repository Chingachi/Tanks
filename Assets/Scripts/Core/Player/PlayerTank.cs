using System;
using Core.Player.Movement;
using UnityEngine;
using Zenject;
namespace Core.Player
{
  [RequireComponent(typeof(Rigidbody))]
  public class PlayerTank : MonoBehaviour
  {
    public event Action OnDeath;
    [SerializeField]
    private float _moveSpeed = 25;
    [SerializeField]
    private float _rotationSpeed = 15;
    [SerializeField]
    private float _rotationValue = 5;
    [SerializeField]
    private LayerMask _enemiesLayerMask;

    [Inject]
    private GameInputs _inputs;

    private BaseMovementSchema _movementSchema;


    private void Start()
    {
      _movementSchema = new TrackMovementSchema(gameObject, _moveSpeed, _rotationSpeed, _rotationValue, _inputs);
    }

    private void Update()
    {
      _movementSchema.HandleInput();
    }

    private void FixedUpdate()
    {
      _movementSchema.UpdateMovement();
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

    public bool Alive
    {
      get;
      private set;
    }
  }
}