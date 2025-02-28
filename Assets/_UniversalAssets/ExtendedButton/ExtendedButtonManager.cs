using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExtendedButtonManager : MonoBehaviour
{
    public static UnityEvent eventPlaySound = new UnityEvent();

    [SerializeField] AudioSource myAudioSource;

    private void Awake()
    {
        if (myAudioSource == null)
        {
            Debug.LogError($"ExtendedButtonManager: Awake: i have no reference to AudioSource");
            Destroy(this);
            return;
        }

        eventPlaySound.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        myAudioSource.Play();
    }
}
