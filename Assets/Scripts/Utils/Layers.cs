using UnityEngine;
namespace Utils
{
  public static class Layers
  {
    public static readonly int Player = LayerMask.NameToLayer("Player");
    public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
    public static readonly int Projectile = LayerMask.NameToLayer("Projectile");
  }
}