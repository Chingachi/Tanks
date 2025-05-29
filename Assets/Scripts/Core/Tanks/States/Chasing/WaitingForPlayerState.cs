using Core.Player;
using Core.Tanks.States.Idle;
namespace Core.Tanks.States.Chasing
{
  public class WaitingForPlayerState : BaseIdleState
  {

    private PlayerTank _player;

    public WaitingForPlayerState (IdleStateContext context)
      : base(context)
    {}

    protected override bool CheckIdle()
    {
      TryCachePlayer();

      return _player == null || !_player.Alive;
    }

    private void TryCachePlayer()
    {
      _player = _context.Container.TryResolve<PlayerTank>();
    }
  }
}