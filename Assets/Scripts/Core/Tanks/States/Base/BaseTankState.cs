namespace Core.Tanks.States.Base
{
  public abstract class BaseTankState<TContext> : BaseTankState
    where TContext : BaseStateContext
  {
    protected readonly TContext _context;

    public BaseTankState (TContext context)
    {
      _context = context;
    }
  }
  public abstract class BaseTankState
  {
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();
  }
}