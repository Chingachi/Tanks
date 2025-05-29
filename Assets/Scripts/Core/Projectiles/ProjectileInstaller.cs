using Core.MonoPool;
using UnityEngine;
using Zenject;
namespace Core.Projectiles
{
  public class ProjectileInstaller : MonoInstaller
  {
    [SerializeField]
    private Projectile _projectilePrefab;

    public override void InstallBindings()
    {
      SimpleMonoObjectPool<Projectile> pool = new SimpleMonoObjectPool<Projectile>(Container, _projectilePrefab);
      Container.BindInstance(pool).AsCached();
    }
  }
}