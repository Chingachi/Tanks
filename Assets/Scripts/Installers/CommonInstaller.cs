using Common.SO;
using EventSystemComponents;
using PopupSystem;
using Zenject;
namespace Installers
{
  public class CommonInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<PopupManager>().AsSingle();
      Container.Bind<EventManager>().AsSingle();
      BindResources();
    }

    private void BindResources()
    {
      Container.Bind<ImageDatabase>().FromResource("ImageDatabase").AsSingle();
    }
  }
}