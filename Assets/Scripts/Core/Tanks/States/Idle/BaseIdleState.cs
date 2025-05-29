using System;
using Core.Tanks.States.Base;
namespace Core.Tanks.States.Idle
{
  public abstract class BaseIdleState : BaseTankState<IdleStateContext>
  {
    public event Action OnWaitingComplete;

    public BaseIdleState (IdleStateContext context)
      : base(context)
    {}

    public override void EnterState()
    {}

    public override void UpdateState()
    {
      if (CheckIdle()) {
        return;
      }

      OnWaitingComplete?.Invoke();
    }

    public override void ExitState()
    {}

    protected abstract bool CheckIdle();
  }
}