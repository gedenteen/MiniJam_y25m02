using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;

public class GameStopwatch : MonoBehaviour // игровой секундомер
{
    public ReactiveProperty<int> CountOfDays = new ReactiveProperty<int>(0);
    
    [SerializeField] private GameplayConfig _gameplayConfig;

    private void Awake()
    {
        StartLogging().Forget();
    }

    private async UniTask StartLogging()
    {
        while (true)
        {
            await UniTask.Delay(
                (int)(_gameplayConfig.DurationOf1GameDayInMilliseconds * Time.timeScale));
            CountOfDays.Value++;
        }
    }
}
