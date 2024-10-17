using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingletonScript : MonoBehaviour
{
    public static SingletonScript instance;
    public Text scoreBoard;
    public Queue<string> princessPlaces = new Queue<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (princessPlaces != null)
        {
            if (princessPlaces.Count == 3)
            {
                scoreBoard.text = "1st: " + princessPlaces.Dequeue() + "\n" +
                                  "2nd: " + princessPlaces.Dequeue()+ "\n" +
                                  "3rd: " + princessPlaces.Dequeue() + "\n";
            }
        }
    }
}
