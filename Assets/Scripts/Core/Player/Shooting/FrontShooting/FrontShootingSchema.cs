using Core.Player.Shooting.Base;
namespace Core.Player.Shooting.FrontShooting
{
  public class FrontShootingSchema : BaseShootingSchema<FrontShootingContext>
  {
    public FrontShootingSchema (FrontShootingContext context)
      : base(context)
    {}
  }
}