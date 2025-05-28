using System;
using Core.Player;
using Core.Tanks.States.Base;
using UnityEngine;
using Zenject;
namespace Core.Tanks.States.Chasing
{
  [Serializable]
  public class ChasingStateContext : BaseStateContext
  {
    [NonSerialized]
    public DiContainer Container;
    public Rigidbody Rigidbody;
    public Transform Transform;

    public float RotationSpeed;
    public float MoveSpeed;

    private PlayerTank _tank;
    public PlayerTank Target
    {
      get
      {
        if (_tank == null) {
          _tank = Container.TryResolve<PlayerTank>();
        }

        return _tank;
      }
    }
  }
}