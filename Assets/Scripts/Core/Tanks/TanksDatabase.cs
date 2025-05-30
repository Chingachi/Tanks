using System.Collections.Generic;
using Core.Tanks.AI;
using UnityEngine;
namespace Core.Tanks
{
  [CreateAssetMenu(fileName = "AiTankDatabase", menuName = "ScriptableObjects/AiTankDatabase")]
  public class TanksDatabase : ScriptableObject
  {
    public List<BaseAi> TankList = new List<BaseAi>();
  }
}