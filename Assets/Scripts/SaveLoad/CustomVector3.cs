using System;
using UnityEngine;
namespace SaveLoad
{
  [Serializable]
  public class CustomVector3
  {
    public float x;
    public float y;
    public float z;
    public float w;

    public CustomVector3() {}

    public CustomVector3 (float x, float y, float z, float w = 0)
    {
      this.x = x;
      this.y = y;
      this.z = z;
      this.w = w;
    }

    public CustomVector3 (Quaternion quaternion)
    {
      x = quaternion.x;
      y = quaternion.y;
      z = quaternion.z;
      w = quaternion.w;
    }

    public CustomVector3 (Vector3 vector)
    {
      x = vector.x;
      y = vector.y;
      z = vector.z;
    }

    public override string ToString()
    {
      return $"({x}, {y}, {z})";
    }

    public Vector3 ToVector3()
    {
      return new Vector3(x, y, z);
    }

    public Quaternion ToQuaternion()
    {
      return new Quaternion(x, y, z, w);
    }
  }
}