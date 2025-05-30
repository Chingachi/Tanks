using System;
using System.Collections.Generic;
using System.Reflection;
using Core.MonoPool;
using Core.Player.Events;
using Core.Player.Movement;
using Core.Player.Movement.Base;
using Core.Player.Shooting.Base;
using Core.Player.Shooting.FrontShooting;
using Core.Player.Shooting.TurretShooting;
using Core.Projectiles;
using Core.StorageComponents.Storages;
using EventSystemComponents;
using SaveLoad;
using UnityEngine;
using Utils;
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


    [Inject]
    private GameInputs _inputs;
    [Inject]
    private SimpleMonoObjectPool<Projectile> _projectilePool;
    [Inject]
    private DiContainer _container;
    [Inject]
    private Storage<SessionData> _storage;
    [Inject]
    private EventManager _eventManager;

    private BaseMovementSchema _movementSchema;
    private BaseShootingSchema _shootingSchema;

    private Dictionary<Type, MovementContext> _movementContexts;
    private Dictionary<Type, ShootingContext> _shootingContexts;

    private void Start()
    {
      _eventManager.SubscribeEvent<ChangeShootingTypeEvent>(HandleShootingSchemaChange);
      _eventManager.SubscribeEvent<ChangeMovementTypeEvent>(HandleMovementSchemaChange);
      InitContexts();
      InitSchemas();
    }

    private void Update()
    {
      _movementSchema.HandleInput();
    }

    private void FixedUpdate()
    {
      _movementSchema.UpdateMovement();
    }


    private void OnDestroy()
    {
      _eventManager.UnsubscribeEvent<ChangeShootingTypeEvent>(HandleShootingSchemaChange);
      _eventManager.UnsubscribeEvent<ChangeMovementTypeEvent>(HandleMovementSchemaChange);
    }

    private void OnApplicationQuit()
    {
      SessionData data = _storage.Data;
      data.MovementSchemeType = _movementSchema.GetType();
      data.ShootingSchemeType = _shootingSchema.GetType();
      _storage.UpdateData(data);
    }

    private void OnTriggerEnter (Collider collision)
    {
      if (collision.gameObject.layer != Layers.Projectile) {
        return;
      }

      if (collision.gameObject.GetComponent<Projectile>().ShooterLayer == Layers.Player) {
        return;
      }

      HandleDeath();
    }

    private void OnCollisionEnter (Collision collision)
    {
      if (collision.gameObject.layer != Layers.Enemy) {
        return;
      }

      HandleDeath();
    }

    public void HandleSpawn()
    {
      Alive = true;
    }

    private void HandleMovementSchemaChange (ChangeMovementTypeEvent eventData)
    {
      _movementSchema.Deactivate();
      InvokeGeneric(nameof(SetMovementSchema), eventData.MovementType);
    }

    private void HandleShootingSchemaChange (ChangeShootingTypeEvent eventData)
    {
      _shootingSchema.Deactivate();
      InvokeGeneric(nameof(SetShootingSchema), eventData.ShootingType);
    }

    private void InitSchemas()
    {
      SessionData data = _storage.Data;

      if (data.MovementSchemeType != null && data.ShootingSchemeType != null) {
        InvokeGeneric(nameof(SetMovementSchema), data.MovementSchemeType);
        InvokeGeneric(nameof(SetShootingSchema), data.ShootingSchemeType);
      } else {
        SetMovementSchema<ClassicMovementSchema>();
        SetShootingSchema<TurretShootingSchema>();
      }
    }

    private void InvokeGeneric (string methodName, Type genericType)
    {
      MethodInfo method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
      MethodInfo generic = method.MakeGenericMethod(genericType);

      int paramCount = method.GetParameters().Length;
      object [] args = new object[paramCount];

      generic.Invoke(this, args);
    }

    private void HandleDeath()
    {
      _eventManager.Fire(new PlayerDeathEvent());
      Alive = false;
      OnDeath?.Invoke();
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
        context.Player = this;
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

      Debug.Log("change shooting schema");

      _shootingSchema = _container.Instantiate<T>(new object []
      {
        _shootingContexts[type]
      });
    }

    public bool Alive
    {
      get;
      private set;
    }
  }
}