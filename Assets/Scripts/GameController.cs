using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Ball ballPrefab;
    public Vector3 ballSpawnPosition;
    public int lives = 3;

    private Brick[] _bricks;

    private void OnEnable()
    {
        _bricks = FindObjectsOfType<Brick>();
        Debug.Log("Number of bricks to break : " + _bricks.Length);
        CreateBall();
    }

    // Update is called once per frame
    private void Update()
    {
        // Hide cursor
        Cursor.visible = false;

        var balls = FindObjectsOfType<Ball>();
        if (balls.Length == 0)
        {
            Debug.Log("No more balls on field");

            if (lives > 0)
            {
                CreateBall();
                --lives;
                Debug.LogFormat("{0} live(s) remaining", lives);
            }
            else
            {
                Debug.LogFormat("You lost!");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        var bricks = FindObjectsOfType<Brick>();
        if (bricks.Length == 0)
        {
            Debug.LogFormat("You won!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void CreateBall()
    {
        Instantiate(ballPrefab, ballSpawnPosition, Quaternion.identity);
        Debug.LogFormat("New ball created");
    }
}
