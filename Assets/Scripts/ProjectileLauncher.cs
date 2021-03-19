using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileLauncher : MonoBehaviour
{
    private static readonly float MULTIPLE_LAUNCH_INTERVAL_SEC = 0.05f;

    public Projectile blueBeamPrefab;
    public Projectile[] redBeamsPrefabs;

    private Renderer _renderer;
    private int _blueBeamsNumber = 0;
    private int _redBeamsNumber = 0;
    private bool _isLaunching = false;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            LaunchProjectile();
        }
    }

    private void LaunchProjectile()
    {
        if (!_isLaunching && OneBallIsMoving())
        {
            if (_redBeamsNumber > 0)
            {
                var coroutine = LaunchMultipleProjectiles(30);
                StartCoroutine(coroutine);
                --_redBeamsNumber;
            }
            else if (_blueBeamsNumber > 0)
            {
                var startPosition = transform.position + new Vector3(0, _renderer.bounds.extents.y + blueBeamPrefab.GetComponent<Renderer>().bounds.extents.y);
                var projectile = Instantiate(blueBeamPrefab, startPosition, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectile.speed);
                --_blueBeamsNumber;
            }
        }
    }

    private IEnumerator LaunchMultipleProjectiles(int numberToLaunch)
    {
        _isLaunching = true;

        while (numberToLaunch-- > 0)
        {
            int prefabIndex = Random.Range(0, redBeamsPrefabs.Length);
            var posX = numberToLaunch % 2 == 0 ? -transform.localScale.x / 3 : transform.localScale.x / 3;

            var startPosition = transform.position + new Vector3(posX, _renderer.bounds.extents.y + redBeamsPrefabs[prefabIndex].GetComponent<Renderer>().bounds.extents.y);
            var projectile = Instantiate(redBeamsPrefabs[prefabIndex], startPosition, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectile.speed);

            yield return new WaitForSeconds(MULTIPLE_LAUNCH_INTERVAL_SEC);
        }

        _isLaunching = false;
    }

    private bool OneBallIsMoving()
    {
        var balls = FindObjectsOfType<Ball>();
        return Array.Exists(balls, b => b.IsMoving());
    }

    public void AddBlueBeam()
    {
        ++_blueBeamsNumber;
    }

    public void AddRedBeams()
    {
        ++_redBeamsNumber;
    }
}
