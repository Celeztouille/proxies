using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance;

    public event Action OnDialogueFinished;
    public event Action<string> OnDialogueEvent;
    public event Action<string> OnSmallDialogueTrigger;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void EventDialogueFinished()
    {
        OnDialogueFinished?.Invoke();
    }

    public void EventDialogueEvent(string value)
    {
        Debug.Log($"[EventsManager] Dialogue Event : {value}");
        OnDialogueEvent?.Invoke(value);
    }

    public void EventSmallDialogueTrigger(string value)
    {
        OnSmallDialogueTrigger?.Invoke(value);
    }
}
