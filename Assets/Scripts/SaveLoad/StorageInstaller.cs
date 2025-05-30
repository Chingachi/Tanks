using Core.StorageComponents.Storages;
using Zenject;
namespace SaveLoad
{
  public class StorageInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<Storage<SessionData>>().To<PlayerPrefsStorage<SessionData>>().AsSingle();
      Container.Bind<Storage<FieldTanksData>>().To<FileStorage<FieldTanksData>>().AsSingle();
    }
  }
}