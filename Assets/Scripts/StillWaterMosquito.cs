using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StillWaterMosquito : MonoBehaviour {

    public GameObject endPoint;
    public float movementSpeed = 10.0f;
    private bool gameOver;

    public void Update()
    {
        if (gameOver)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPoint.transform.position, movementSpeed * Time.deltaTime);
        }
    }

    public void GameOver()
    {
        gameOver = true;
    }
}
