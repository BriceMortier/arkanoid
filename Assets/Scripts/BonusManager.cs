using UnityEngine;

[CreateAssetMenu(fileName = "BonusManager", menuName = "ScriptableObjects/BonusManager", order = 1)]
public class BonusManager : ScriptableObject
{
    public float bonusSpeed;
    public GameObject bonusBlueBeam;
    public GameObject bonusRedBeams;

    public void SpawnRandomBonus(Vector3 spawnPosition)
    {
        GameObject bonus = null;

        if (Random.value > .9f)
        {
            bonus = Instantiate(bonusBlueBeam, spawnPosition, Quaternion.identity);
        }
        else if (Random.value > .8f)
        {
            bonus = Instantiate(bonusRedBeams, spawnPosition, Quaternion.identity);
        }

        if (bonus != null)
        {
            bonus.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bonusSpeed);
        }
    }
}
