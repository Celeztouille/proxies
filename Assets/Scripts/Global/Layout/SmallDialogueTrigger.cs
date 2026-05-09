using System;
using UnityEngine;

public class SmallDialogueTrigger : MonoBehaviour
{
    [SerializeField] private string fileName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[SmallDialogueTrigger] Triggered");
            EventsManager.Instance.EventSmallDialogueTrigger(fileName);
            gameObject.SetActive(false);
        }
    }
}
