using Common.SceneControllerComponents;
using Zenject;
namespace Installers
{
  public class ProjectInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<SceneController>().AsSingle();
    }
  }
}