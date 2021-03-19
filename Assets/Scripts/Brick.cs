using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public float initialHealth = 1;
    public ParticleSystem shockParticlePrefab;
    public ParticleSystem dieParticlePrefab;
    public BonusManager bonusManager;

    private float _health;

    private void Start()
    {
        _health = initialHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            var contact = collision.GetContact(0);
            Hit(ball.power, contact.point, contact.normal);
        }
    }

    public void Hit(float power, Vector2 contactPoint, Vector2 contactNormal)
    {
        _health -= power;

        var currentColor = GetComponent<SpriteRenderer>().color;

        // Increase brick transparency
        currentColor.a = .2f + .8f * _health / initialHealth;
        GetComponent<SpriteRenderer>().color = currentColor;

        // Instantiate a shockwave at the collision contact point
        var shockDirection = -contactNormal;
        var zRotation = Vector2.SignedAngle(Vector2.up, shockDirection);

        var shockParticle = Instantiate(shockParticlePrefab, contactPoint, Quaternion.Euler(0, 0, zRotation));
        ApplyColorAndDestroy(shockParticle);

        if (_health <= 0)
        {
            Die();
        }
    }

    private void ApplyColorAndDestroy(ParticleSystem particle)
    {
        var startColor = GetComponent<SpriteRenderer>().color;
        startColor.a = 1f;

        var particleMain = particle.main;
        particleMain.startColor = startColor;

        Destroy(particle.gameObject, particleMain.duration);
    }

    private void Die()
    {
        var dieParticle = Instantiate(dieParticlePrefab, transform.position, Quaternion.identity);
        ApplyColorAndDestroy(dieParticle);

        bonusManager.SpawnRandomBonus(transform.position);

        Destroy(gameObject);
    }
}
