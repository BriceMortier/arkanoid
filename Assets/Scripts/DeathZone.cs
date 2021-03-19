using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var ball = collider.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            Debug.Log("Ball entered death zone");
            ball.Die();
        }
    }
}
