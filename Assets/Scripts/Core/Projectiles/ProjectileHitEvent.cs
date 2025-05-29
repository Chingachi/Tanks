using EventSystemComponents;
using UnityEngine;
namespace Core.Projectiles
{
  public class ProjectileHitEvent : BaseEvent
  {
    public GameObject HitObject;

    public ProjectileHitEvent (GameObject hitObject)
    {
      HitObject = hitObject;
    }
  }
}