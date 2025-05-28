using Zenject;
namespace Input
{
  public class InputInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<GameInputs>().AsCached();
    }
  }
}