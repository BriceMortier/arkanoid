public class BonusBlueBeam : Bonus
{
    protected override void GiveBonus(ProjectileLauncher launcher)
    {
        launcher.AddBlueBeam();
    }
}
