using System;
using System.Collections.Generic;
using Core.Tanks.States.Base;
using Core.Tanks.States.Chasing;
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
      ChangeState<ChasingState>();
    }

    protected override Dictionary<Type, BaseTankState> RegisterStates()
    {
      _context.Target = _container.Resolve<Dummy>().gameObject;

      return new Dictionary<Type, BaseTankState>
      {
        {
          typeof(ChasingState), new ChasingState(_context)
        }
      };
    }
  }
}