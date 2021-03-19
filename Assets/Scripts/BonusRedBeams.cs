public class BonusRedBeams : Bonus
{
    protected override void GiveBonus(ProjectileLauncher launcher)
    {
        launcher.AddRedBeams();
    }
}
