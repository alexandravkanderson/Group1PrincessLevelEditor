using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedHeuristic : HeuristicScript
{
    public override float Heuristic(int x, int y, Vector3 start, Vector3 goal, GridScript gridScript){
        if (Random.Range(0f, 1f) > .5f)
        {
            x = Mathf.RoundToInt(transform.position.x - goal.x);
            y = Mathf.RoundToInt(transform.position.y - goal.y);
            return 1 * (x + y);
        }
        else 
        {
            return Random.Range(0, 1000);
        }
    }
}
