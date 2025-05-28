using System;
using System.Collections.Generic;
using Core.Tanks.States.Base;
using Core.Tanks.States.SimpleMovements;
using UnityEngine;
namespace Core.Tanks.AI
{
  public class SimpleAi : BaseAi
  {
    [SerializeField]
    private SimpleMovementStateContext _context;

    protected virtual void OnEnable()
    {
      _currentState?.EnterState();
    }

    protected virtual void FixedUpdate()
    {
      _currentState?.UpdateState();
    }

    protected virtual void OnCollisionEnter (Collision collision)
    {
      _context.OnCollisionEnter?.Invoke(collision);
    }

    protected override void RunFirstState()
    {
      ChangeState<SimpleMovementState>();
    }

    protected override Dictionary<Type, BaseTankState> RegisterStates()
    {
      return new Dictionary<Type, BaseTankState>
      {
        {
          typeof(SimpleMovementState), new SimpleMovementState(_context)
        }
      };
    }

    protected virtual void OnCollisionStay (Collision collision)
    {
      _context.OnCollisionStay?.Invoke(collision);
    }
  }
}