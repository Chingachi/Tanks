using System;
using Core.Tanks.States.Base;
using UnityEngine;
namespace Core.Tanks.States.SimpleMovements
{
  [Serializable]
  public class SimpleMovementStateContext : BaseStateContext
  {
    public float MoveSpeed = 3f;
    public float RotationSpeed = 15f;
    public float MinChangeInterval = 2f;
    public float MaxChangeInterval = 5f;
    public float MaxTurnAngle = 120f;
    public LayerMask IgnoredLayers;

    public Rigidbody Rigidbody;
    public Transform Transform;

    public Action<Collision> OnCollisionEnter;
    public Action<Collision> OnCollisionStay;
  }
}