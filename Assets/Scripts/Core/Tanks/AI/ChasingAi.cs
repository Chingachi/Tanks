using System;
using System.Collections.Generic;
using Core.Tanks.States.Base;
using Core.Tanks.States.Chasing;
using Core.Tanks.States.Idle;
using UnityEngine;
namespace Core.Tanks.AI
{
  public class ChasingAi : BaseAi
  {
    [SerializeField]
    private ChasingStateContext _context;


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
      ChasingState chasingState = new ChasingState(_context);
      chasingState.OnTargetDied += HandlePlayerDeath;

      return new Dictionary<Type, BaseTankState>
      {
        {
          typeof(WaitingForPlayerState), idleState
        },
        {
          typeof(ChasingState), chasingState
        }
      };
    }

    private void HandlePlayerDeath()
    {
      ChangeState<WaitingForPlayerState>();
    }

    private void HandleEndOfIdle()
    {
      ChangeState<ChasingState>();
    }
  }
}