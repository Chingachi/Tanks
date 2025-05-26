using Core.StorageComponents;
using Core.StorageComponents.Storages;
using Zenject;
namespace Installers
{
  public class ProjectInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<Storage<SessionData>>().To<FileStorage<SessionData>>().AsCached();
    }
  }
}