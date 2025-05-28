using Core.Tanks.States.Base;
using Zenject;
namespace Core.Tanks.States.Idle
{
  public class IdleStateContext : BaseStateContext
  {
    public DiContainer Container;

    public IdleStateContext (DiContainer player)
    {
      Container = player;
    }
  }
}