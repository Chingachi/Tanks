using System;
using Core.Tanks.States.Base;
using UnityEngine;
namespace Core.Tanks.States.Chasing
{
  [Serializable]
  public class ChasingStateContext : BaseStateContext
  {
    [NonSerialized]
    public GameObject Target;
    public Rigidbody Rigidbody;
    public Transform Transform;

    public float RotationSpeed;
    public float MoveSpeed;
  }
}