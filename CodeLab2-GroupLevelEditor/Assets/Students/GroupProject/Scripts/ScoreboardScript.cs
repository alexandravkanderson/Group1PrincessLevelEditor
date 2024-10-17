using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardScript : MonoBehaviour
{
    private void OnDestroy()
    {
        SingletonScript.instance.princessPlaces.Enqueue(gameObject.name);
    }
}
