using System;
using System.Collections.Generic;
using Core.MonoPool;
using Core.Player.Movement;
using Core.Player.Movement.Base;
using Core.Player.Shooting.Base;
using Core.Player.Shooting.FrontShooting;
using Core.Player.Shooting.TurretShooting;
using Core.Projectiles;
using UnityEngine;
using Zenject;
namespace Core.Player
{
  [RequireComponent(typeof(Rigidbody))]
  public class PlayerTank : MonoBehaviour
  {
    public event Action OnDeath;
    [SerializeField]
    private MovementContext _movementContext;
    [SerializeField]
    private FrontShootingContext _frontShootingContext;
    [SerializeField]
    private TurretShootingContext _turretShootingContext;
    [SerializeField, Space]
    private LayerMask _enemiesLayerMask;


    [Inject]
    private GameInputs _inputs;
    [Inject]
    private SimpleMonoObjectPool<Projectile> _projectilePool;

    private BaseMovementSchema _movementSchema;
    private BaseShootingSchema _shootingSchema;

    private Dictionary<Type, MovementContext> _movementContexts;
    private Dictionary<Type, ShootingContext> _shootingContexts;

    private void Start()
    {
      InitContexts();
      SetMovementSchema<ClassicMovementSchema>();
      SetShootingSchema<TurretShootingSchema>();
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

    private void InitContexts()
    {
      _movementContext.Inputs = _inputs;
      _movementContext.Tank = gameObject;

      _movementContexts = new Dictionary<Type, MovementContext>
      {
        {
          typeof(ClassicMovementSchema), _movementContext
        },
        {
          typeof(TrackMovementSchema), _movementContext
        }
      };

      PrepareShootingContext(_frontShootingContext);
      PrepareShootingContext(_turretShootingContext);

      _shootingContexts = new Dictionary<Type, ShootingContext>
      {
        {
          typeof(FrontShootingSchema), _frontShootingContext
        },
        {
          typeof(TurretShootingSchema), _turretShootingContext
        }
      };

      void PrepareShootingContext (ShootingContext context)
      {
        context.Inputs = _inputs;
        context.ProjectilePool = _projectilePool;
      }
    }

    private void SetMovementSchema<T>()
      where T : BaseMovementSchema
    {
      Type type = typeof(T);

      if (!_movementContexts.ContainsKey(type)) {
        Debug.LogError($"[{type}]No such movement context!");

        return;
      }

      _movementSchema = (T)Activator.CreateInstance(type, _movementContexts[type]);
    }

    private void SetShootingSchema<T>()
      where T : BaseShootingSchema
    {
      Type type = typeof(T);

      if (!_shootingContexts.ContainsKey(type)) {
        Debug.LogError($"[{type}]No such shooting context!");

        return;
      }

      _shootingSchema = (T)Activator.CreateInstance(type, _shootingContexts[type]);
    }

    public bool Alive
    {
      get;
      private set;
    }
  }
}