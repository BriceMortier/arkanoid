using UnityEngine;

public abstract class Bonus : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var launcher = collision.gameObject.GetComponent<ProjectileLauncher>();
        if (launcher != null)
        {
            GiveBonus(launcher);
            Destroy(gameObject);
        }

        var deathZone = collision.gameObject.GetComponent<DeathZone>();
        if (deathZone != null)
        {
            Destroy(gameObject);
        }
    }

    protected abstract void GiveBonus(ProjectileLauncher launcher);
}
