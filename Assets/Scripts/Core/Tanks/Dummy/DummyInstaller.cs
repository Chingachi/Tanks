using UnityEngine;
using Zenject;
namespace Core.Tanks
{
  public class DummyInstaller : MonoInstaller
  {
    [SerializeField]
    private Dummy _dummy;

    public override void InstallBindings()
    {
      Container.BindInstance(_dummy);
    }
  }
}