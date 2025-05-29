using UnityEngine;
namespace Utils
{
  public static class Layers
  {
    public static LayerMask Enemy
    {
      get
      {
        return LayerMask.GetMask("Enemy");
      }
    }
    public static LayerMask Player
    {
      get
      {
        return LayerMask.GetMask("Player");
      }
    }
    public static LayerMask Projectile
    {
      get
      {
        return LayerMask.GetMask("Projectile");
      }
    }
  }
}