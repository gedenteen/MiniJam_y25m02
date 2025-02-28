using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExtendedButton : MonoBehaviour
{
    [SerializeField] Button myButton;

    private void Awake()
    {
        if (myButton == null)
        {
            Debug.LogError($"ExtendedButton: Awake: i have no reference to my button");
            return;
        }

        myButton.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        ExtendedButtonManager.eventPlaySound.Invoke();
    }
}
