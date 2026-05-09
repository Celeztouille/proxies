using System;
using System.IO;
using StarterAssets;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private bool sceneBeginsWithDialogue;
    [SerializeField] private string beginningDialogueFilename;
    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private DialogueBox dialogueBox;
    

    private void Start()
    {
        EventsManager.Instance.OnDialogueFinished += OnDialogueFinished;
        
        if (sceneBeginsWithDialogue)
        {
            LoadDialogue(beginningDialogueFilename);    
        }
    }

    private void LoadDialogue(string fileName)
    {
        firstPersonController.enabled = false;
        dialogueBox.Load(fileName);
    }

    private void OnDialogueFinished()
    {
        firstPersonController.enabled = true;
    }
}
