using System;
using System.Collections.Generic;
namespace SaveLoad
{
  [Serializable]
  public class FieldTanksData
  {
    public List<TankSaveData> Tanks = new List<TankSaveData>();
    public TankPositionSaveData Player;
  }

  [Serializable]
  public class TankSaveData
  {
    public string Id;
    public Type TankType;
    public TankPositionSaveData TransformData;
  }

  [Serializable]
  public class TankPositionSaveData
  {
    public CustomVector3 Position;
    public CustomVector3 Rotation;
  }
}