using System;
using UnityEngine;
namespace Core.Player.Movement.Base
{
  [Serializable]
  public class MovementContext
  {
    public float MoveSpeed = 15;
    public float RotationSpeed = 100;
    public float RotationValue = 5;
    public GameObject Tank;
    public GameInputs Inputs;
  }
}