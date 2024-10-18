using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Added all Princess into a list
    public List<GameObject> princessList;
    
    //to check if the game has started.
    private bool isStarted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject princess in princessList)
        {
            if (princess != null)
            {
                princess.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void GameStart()
    {
        // the start button can only be pressed once
        if (isStarted)
        {
            return;
        }

        foreach (GameObject princess in princessList)
        {
            if (princess != null)
            {
                princess.SetActive(true);
            }
        }
        
        isStarted = true;
    }
}
