using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SceneTransition : MonoBehaviour
{
    public static event Action EventStartLoadingScene;
    public static event Action<float> EventUpdateProgress;
    public static event Action EventFinishLoadingScene;
    public static SceneTransition instance { get; private set;} = null;

    private AsyncOperation loadingSceneOperation;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public async void SwitchToNextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        await SwitchToSceneAsync(sceneIndex);
    }

    public async void SwitchToMainMenu()
    {
        await SwitchToSceneAsync((int)SceneNames.MainMenu);
    }

    public async void ReloadCurrentScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        await SwitchToSceneAsync(sceneIndex);
    }

    private async UniTask SwitchToSceneAsync(int sceneIndex)
    {
        EventStartLoadingScene?.Invoke();

        // Load empty Boot scene first for freeing RAM
        await SceneManager.LoadSceneAsync(SceneNames.Boot.ToString());

        // Load target scene
        loadingSceneOperation = SceneManager.LoadSceneAsync(sceneIndex);
        //loadingSceneOperation.allowSceneActivation = false; // can be useful in some cases

        // Update progress in Loading Screen
        while (!loadingSceneOperation.isDone)
        {
            EventUpdateProgress?.Invoke(loadingSceneOperation.progress);
            await UniTask.NextFrame();
        }

        EventFinishLoadingScene?.Invoke();
    }
}
