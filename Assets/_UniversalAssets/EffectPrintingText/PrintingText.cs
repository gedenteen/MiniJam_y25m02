using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// A class that output text character-by-character with a delay (it can be called as printing effect)
public class PrintingText : MonoBehaviour
{
    [SerializeField] TextsToDisplay textsToDisplay;
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] Button button;

    private bool isTyping = false;
    private IEnumerator coroutineForTextAnimation = null;
    private int indexOfText = 0;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    private void Start()
    {
        PrintText();
    }

    public void PrintText()
    {
        textMesh.text = "";
        coroutineForTextAnimation =
            CoroutinePrint(textsToDisplay.texts[indexOfText], textsToDisplay.delayForWrite);
        StartCoroutine(coroutineForTextAnimation);
    }

    private IEnumerator CoroutinePrint(string text, float delayForWrite = 0.05f)
    {
        WaitForSeconds delay = new WaitForSeconds(delayForWrite);
        
        isTyping = true;
        int i = 0;
        while (i < text.Length)
        {
            textMesh.text += text[i];
            i++;
            yield return delay;
        }
        isTyping = false;
    }

    private void OnButtonClick()
    {
        if (isTyping)
        {
            // Stop animation (coroutine), display a whole text
            if (coroutineForTextAnimation != null)
            {
                StopCoroutine(coroutineForTextAnimation);
                textMesh.text = textsToDisplay.texts[indexOfText];
                isTyping = false;
            }
            else
            {
                Debug.LogError($"PrintingText: OnButtonClick: coroutineForTextAnimation is null");
            }
        }
        else
        {
            // Display next text (if exists)
            indexOfText++;
            if (indexOfText < textsToDisplay.texts.Count)
            {
                PrintText();
            }
        }
    }
}
