using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomizedHeuristic : HeuristicScript
{
    public float upperBound = .6f;
    public float lowerBound = .2f;
    public override float Heuristic(int x, int y, Vector3 start, Vector3 goal, GridScript gridScript)
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber > upperBound)
        {
            x = Mathf.RoundToInt(transform.position.x - goal.x);
            y = Mathf.RoundToInt(transform.position.y - goal.y);
            return 1 * (x + y);
        }
        else if (randomNumber <= upperBound && randomNumber > lowerBound) 
        {
            return Random.Range(0, 1000);
        }

        else
        {
            x = Mathf.RoundToInt(transform.position.x - goal.x);
            y = Mathf.RoundToInt(transform.position.y - goal.y);
            return 1 * Mathf.Sqrt(x * x + y * y);
        }
    }
}
