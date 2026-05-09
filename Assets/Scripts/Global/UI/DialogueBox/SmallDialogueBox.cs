using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SmallDialogueBox : MonoBehaviour
{
    [SerializeField] private GameObject content;
    
    [Header("Text Box")]
    [SerializeField] private int textSpeed = 40;
    [SerializeField] private TMP_Text textName;
    [SerializeField] private TMP_Text textBox;
    
    private float width;  
    private bool isDisplayingTextLine;
    private bool isDialogueRunning;
    private int currentLineIndex;
    private int maxLineIndex;

    private List<string> dialogueLines = new List<string>();

    private void Start()
    {
        EventsManager.Instance.OnSmallDialogueTrigger += RunDialogue;
    }


    private void RunDialogue(string fileName)
    {
        textBox.text = "";
        currentLineIndex = 0;
        content.SetActive(true);

        TextAsset textAsset = (TextAsset)Resources.Load($"Dialogues/{fileName}");
        dialogueLines = textAsset.text.Split('\n').ToList();
        maxLineIndex = dialogueLines.Count;
        isDialogueRunning = true;
        ShowTextLine(dialogueLines[currentLineIndex]);
        
        StartCoroutine(RunDialogueCoroutine());

        IEnumerator RunDialogueCoroutine()
        {
            while (isDialogueRunning)
            {
                if (isDisplayingTextLine)
                {
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                    if (currentLineIndex < maxLineIndex - 1)
                    {
                        currentLineIndex++;
                        ShowTextLine(dialogueLines[currentLineIndex]);
                    }
                    else
                    {
                        FinishDialogue();
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void FinishDialogue()
    {
        content.SetActive(false);
        textBox.text = "";
        currentLineIndex = 0;
        dialogueLines.Clear();
        isDialogueRunning = false;
    }

    private void ShowTextLine(string text)
    {
        // Line is event
        if (text[0] == '#')
        {
            EventsManager.Instance.EventDialogueEvent(text.Split('#')[1].Trim('\n'));
            if (currentLineIndex < maxLineIndex - 1)
            {
                currentLineIndex++;
                ShowTextLine(dialogueLines[currentLineIndex]);
            }
            else
            {
                FinishDialogue();
            }
            return; 
        }
        
        string character = text.Split('|')[0];
        string textContent = text.Split('|')[1];
        textName.text = character;
        
        StartCoroutine(TextBoxCoroutine());
        
        IEnumerator TextBoxCoroutine()
        {
            isDisplayingTextLine = true;
            textBox.text = "";
            foreach (char c in textContent)
            {
                textBox.text += c;
                yield return new WaitForSeconds(1f / textSpeed);
            }
            isDisplayingTextLine = false;
        }
    }
}
