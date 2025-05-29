using System;
using System.Collections.Generic;
using Core.MonoPool;
using Core.Projectiles;
using Core.Tanks.States.Base;
using Core.Tanks.States.Chasing;
using Core.Tanks.States.Idle;
using Core.Tanks.States.SimpleMoveAndShoot;
using UnityEngine;
using Zenject;
namespace Core.Tanks.AI
{
  public class MovingAndShootingAi : BaseAi
  {
    [SerializeField, Header("State context")]
    private SimpleMoveAndShootStateContext _context;

    [Inject]
    private DiContainer _container;
    [Inject]
    private SimpleMonoObjectPool<Projectile> _projectilePool;

    private void FixedUpdate()
    {
      _currentState?.UpdateState();
    }

    protected override void RunFirstState()
    {
      ChangeState<WaitingForPlayerState>();
    }

    protected override Dictionary<Type, BaseTankState> RegisterStates()
    {
      WaitingForPlayerState idleState = new WaitingForPlayerState(new IdleStateContext(_container));
      idleState.OnWaitingComplete += HandleEndOfIdle;

      _context.Container = _container;
      _context.ShootingContext.ProjectilePool = _projectilePool;
      SimpleMoveAndShootState state = new SimpleMoveAndShootState(_context);

      return new Dictionary<Type, BaseTankState>
      {
        {
          typeof(WaitingForPlayerState), idleState
        },
        {
          typeof(SimpleMoveAndShootState), state
        }
      };
    }

    private void HandleEndOfIdle()
    {
      ChangeState<SimpleMoveAndShootState>();
    }
  }
}