using System;
using System.Collections.Generic;
using Core.Tanks.States.Base;
using Core.Tanks.States.Chasing;
using Core.Tanks.States.Idle;
using UnityEngine;
using Zenject;
namespace Core.Tanks.AI
{
  public class ChasingAi : BaseAi
  {
    [SerializeField]
    private ChasingStateContext _context;

    [Inject]
    private DiContainer _container;

    private void FixedUpdate()
    {
      _currentState?.UpdateState();
    }

    protected override void RunFirstState()
    {
      ChangeState<ChasingIdleState>();
    }

    protected override Dictionary<Type, BaseTankState> RegisterStates()
    {
      ChasingIdleState idleState = new ChasingIdleState(new IdleStateContext(_container));
      idleState.OnIdleComplete += HandleEndOfIdle;

      _context.Container = _container;
      ChasingState chasingState = new ChasingState(_context);
      chasingState.OnTargetDied += HandlePlayerDeath;

      return new Dictionary<Type, BaseTankState>
      {
        {
          typeof(ChasingIdleState), idleState
        },
        {
          typeof(ChasingState), chasingState
        }
      };
    }

    private void HandlePlayerDeath()
    {
      ChangeState<ChasingIdleState>();
    }

    private void HandleEndOfIdle()
    {

      ChangeState<ChasingState>();
    }
  }
}