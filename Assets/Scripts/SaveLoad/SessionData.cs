using System;
namespace SaveLoad
{
  [Serializable]
  public class SessionData
  {
    public int EnemyKillCount;
    public int PlayerDeathCount;
    public Type MovementSchemeType;
    public Type ShootingSchemeType;
  }
}