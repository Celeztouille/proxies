using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private GameObject content;
    
    [Header("Sizes")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform topSection;
    [SerializeField] private int margin = 200;
    [SerializeField] private int height = 250;
    
    [Header("Top Section")]
    [SerializeField] private RectTransform lineFull;
    [SerializeField] private RectTransform lineLeft;
    [SerializeField] private RectTransform lineRight;
    [SerializeField] private RectTransform textNameRectTransform;
    [SerializeField] private TMP_Text textName;
    
    [Header("Text Box")]
    [SerializeField] private int textSpeed = 40;
    [SerializeField] private RectTransform textBoxRectTransform;
    [SerializeField] private TMP_Text textBox;

    [Header("ContinueButton")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private float continueButtonBlinkRate = 1;
    [SerializeField] private float continueButtonBlinkRatio = 0.75f;
    
    private float width;
    private Coroutine continueButtonCoroutine;    
    private Coroutine textBoxCoroutine;
    private bool isDisplayingTextLine;
    private bool isDialogueRunning;
    private int currentLineIndex;
    private int maxLineIndex;

    private List<string> dialogueLines = new List<string>();
    
    
    private void Start()
    {
        Init();
    }

    public void Load(string fileName)
    {
        if (isDialogueRunning) return;
        
        textBox.text = "";
        currentLineIndex = 0;
        lineLeft.gameObject.SetActive(false);
        lineRight.gameObject.SetActive(false);
        textName.gameObject.SetActive(false);
        lineFull.gameObject.SetActive(true);
        content.SetActive(true);

        TextAsset textAsset = (TextAsset)Resources.Load($"Dialogues/{fileName}");
        dialogueLines = textAsset.text.Split('\n').ToList();
        maxLineIndex = dialogueLines.Count;
        ShowTextLine(dialogueLines[currentLineIndex]);
        isDialogueRunning = true;
        
    }

    private void FinishDialogue()
    {
        content.SetActive(false);
        textBox.text = "";
        currentLineIndex = 0;
        lineLeft.gameObject.SetActive(false);
        lineRight.gameObject.SetActive(false);
        textName.gameObject.SetActive(false);
        lineFull.gameObject.SetActive(true);
        dialogueLines.Clear();
        isDialogueRunning = false;
        EventsManager.Instance.EventDialogueFinished();
    }

    private void Update()
    {
        if (!isDialogueRunning) return;
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isDisplayingTextLine)
            {
                CompleteTextLineInstantly(dialogueLines[currentLineIndex]);
            }
            else
            {
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
        }
    }

    private void Init()
    {
        width = Screen.width - 2f * margin;
        rectTransform.sizeDelta = new Vector2(width, height);
        topSection.sizeDelta = new Vector2(width, topSection.rect.height);
        textBoxRectTransform.sizeDelta = new Vector2(width, textBoxRectTransform.rect.height);
        lineFull.sizeDelta = new Vector2(width, lineFull.rect.height);
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
        
        lineLeft.gameObject.SetActive(!String.IsNullOrEmpty(character));
        lineRight.gameObject.SetActive(!String.IsNullOrEmpty(character));
        textName.gameObject.SetActive(!String.IsNullOrEmpty(character));
        lineFull.gameObject.SetActive(String.IsNullOrEmpty(character));
        textName.text = character;

        StartCoroutine(AdaptLines());
        
        textBoxCoroutine = StartCoroutine(TextBoxCoroutine());
        
        IEnumerator TextBoxCoroutine()
        {
            isDisplayingTextLine = true;
            ToggleContinueButton(false);
            textBox.text = "";
            foreach (char c in textContent)
            {
                textBox.text += c;
                yield return new WaitForSeconds(1f / textSpeed);
            }
            ToggleContinueButton(true);
            isDisplayingTextLine = false;
        }
         
        IEnumerator AdaptLines()
        {
            yield return new WaitForEndOfFrame();
            lineLeft.sizeDelta = new Vector2((width - textNameRectTransform.rect.width) / 2f - 25f, lineLeft.rect.height);
            lineRight.sizeDelta = new Vector2((width - textNameRectTransform.rect.width) / 2f - 25f, lineRight.rect.height);
        }
        
    }

    private void CompleteTextLineInstantly(string text)
    {
        string textContent = text.Split('|')[1];
        
        if (textBoxCoroutine != null)
        {
            StopCoroutine(textBoxCoroutine);
            textBoxCoroutine = null;
        }
        textBox.text = textContent;
        isDisplayingTextLine = false;
        ToggleContinueButton(true);
    }

    private void ToggleContinueButton(bool value)
    {
        if (value)
        {
            continueButtonCoroutine = StartCoroutine(ContinueButtonBlink());
        }
        else
        {
            if (continueButtonCoroutine != null)
            {
                StopCoroutine(continueButtonCoroutine);
                continueButtonCoroutine = null;
            }
            continueButton.SetActive(false);
        }
    }

    private IEnumerator ContinueButtonBlink()
    {
        while (true)
        {
            continueButton.SetActive(true);
            yield return new WaitForSeconds(continueButtonBlinkRate * continueButtonBlinkRatio);
            continueButton.SetActive(false);
            yield return new WaitForSeconds(continueButtonBlinkRate * (1f - continueButtonBlinkRatio));
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
