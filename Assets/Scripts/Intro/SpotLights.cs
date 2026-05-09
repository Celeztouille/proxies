using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLights : MonoBehaviour
{
    [SerializeField] private List<GameObject> spotLights;

    private void Start()
    {
        EventsManager.Instance.OnDialogueEvent += SpotLightsSequence;
    }


    private void SpotLightsSequence(string eventName)
    { 
        if (eventName != "Intro_Lights") return;  
        
        StartCoroutine(SpotLightsCoroutine());
    }

    private IEnumerator SpotLightsCoroutine()
    {
        foreach (GameObject spotLight in spotLights)
        {
            spotLight.SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
