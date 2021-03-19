using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float power;
    public float speed;
    public ParticleSystem dieParticlePrefab;
    public Color dieParticleColor;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var brick = collision.gameObject.GetComponent<Brick>();
        if (brick != null)
        {
            var contact = collision.GetContact(0);
            brick.Hit(power, contact.point, -contact.normal);
        }

        Die();
    }

    private void Die()
    {
        var dieParticle = Instantiate(dieParticlePrefab, transform.position, Quaternion.identity);
        var dieParticleMain = dieParticle.main;
        dieParticleMain.startColor = dieParticleColor;

        Destroy(dieParticle.gameObject, dieParticleMain.duration);
        Destroy(gameObject);
    }
}
