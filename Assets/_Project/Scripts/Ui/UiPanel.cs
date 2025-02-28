using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UiPanel : MonoBehaviour
{
    [Header("UI panel: references to objects")]
    [SerializeField]
    protected CanvasGroup canvasGroup;

    // Метод для включения или отключения CanvasGroup
    public void Activate(bool active, float seconds = 0f)
    {
        if (canvasGroup != null)
        {
            ChangeAlpha(active, seconds);
            canvasGroup.interactable = active; // Устанавливаем интерактивность в зависимости от active
            // canvasGroup.blocksRaycasts = active; // Устанавливаем блокировку событий мыши в зависимости от active
        }
        else
        {
            Debug.LogError("UiPanel: CanvasGroup is not assigned or found on this GameObject.");
        }
    }

    private async void ChangeAlpha(bool active, float seconds)
    {
        if (active) // если включаем, то включаем gameObject сразу
        {
            canvasGroup.gameObject.SetActive(active);
        }

        if (seconds == 0f)
        {
            canvasGroup.alpha = active ? 1 : 0; // Устанавливаем прозрачность в 1 (включено) или 0 (выключено)
        }
        else
        {
            if (active)
            {
                canvasGroup.alpha = 0;
                await SmoothChangeAlpha(seconds, 0f, 1f);
            }
            else
            {
                canvasGroup.alpha = 1f;
                await SmoothChangeAlpha(seconds, 1f, 0f);
            }
        }

        if (!active) // если выключаем, то выключаем gameObject в конце
        {
            canvasGroup.gameObject.SetActive(active);
        }
    }

    private async UniTask SmoothChangeAlpha(float seconds, float startAlpha, float endAlpha)
    {
        if (seconds <= 0f)
        {
            Debug.LogError($"SmoothChangeAlpha: invalid seconds = {seconds}");
            return;
        }

        float timer = 0f;
        while (timer < seconds)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / seconds);
            timer += Time.deltaTime;
            await UniTask.Yield();
        }
    }
}
