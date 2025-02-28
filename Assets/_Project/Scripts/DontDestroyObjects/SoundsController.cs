using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SoundsController : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> _audioSources;

    private bool[] _isSourceBusy;

    private void OnValidate()
    {
        if (_audioSources == null)
        {
            AudioSource[] arrayOfAudioSources = GetComponents<AudioSource>();
            _audioSources = new List<AudioSource>(arrayOfAudioSources);
        }
    }

    private void Awake()
    {
        // Инициализация массива занятости
        if (_audioSources != null)
        {
            _isSourceBusy = new bool[_audioSources.Count];
        }
        else
        {
            Debug.LogError("SoundsController: AudioSources list is not assigned in the inspector.");
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (_audioSources == null || _isSourceBusy == null)
        {
            Debug.LogError("SoundsController: AudioSources list or isSourceBusy array is not initialized.");
            return;
        }

        for (int i = 0; i < _audioSources.Count; i++)
        {
            if (!_isSourceBusy[i]) // Если AudioSource свободен
            {
                PlaySoundOnSource(i, clip).Forget();
                return;
            }
        }

        Debug.LogError("SoundsController: No available AudioSource to play the sound.");
    }

    private async UniTask PlaySoundOnSource(int index, AudioClip clip)
    {
        // Пометить источник как занятый
        _isSourceBusy[index] = true;

        // Установить и воспроизвести звук
        _audioSources[index].clip = clip;
        _audioSources[index].Play();

        // Подождать до завершения воспроизведения звука
        await UniTask.WaitForSeconds(clip.length);

        // Освободить источник
        _isSourceBusy[index] = false;
    }
}
