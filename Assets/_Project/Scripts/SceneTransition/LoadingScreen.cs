using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LoadingScreen : UiPanel
{
    [Header("Parameters")]
    [SerializeField] private float _delayForActivate = 0.25f;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI textPercentage;
    [SerializeField] private Image progressBar;

    private float lastProgressValue = 0f;

    private void Start()
    {
        SceneTransition.EventStartLoadingScene += OnStartLoadingScene;
        SceneTransition.EventUpdateProgress += UpdateProgress;
        SceneTransition.EventFinishLoadingScene += OnFinishLoadingScene;
    }

    private void UpdateProgress(float progress, bool forceSet)
    {
        float newValue;
        if (forceSet)
        {
            newValue = progress;
        }
        else
        {
            newValue = Mathf.Lerp(lastProgressValue, progress, 0.02f);
        }
        
        textPercentage.text = string.Concat(Math.Round(newValue * 100), "%");
        progressBar.fillAmount = newValue;

        lastProgressValue = newValue;
    }

    private void UpdateProgress(float progress)
    {
        UpdateProgress(progress, false);
    }

    private void OnStartLoadingScene()
    {
        // Activate Loading Screen
        UpdateProgress(0f, true);
        Activate(true, _delayForActivate);
    }

    private void OnFinishLoadingScene()
    {
        // Deactivate Loading Screen
        UpdateProgress(1f, true);
        Activate(false, _delayForActivate);
    }
}
