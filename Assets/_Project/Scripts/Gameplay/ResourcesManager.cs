using UnityEngine;
using UniRx;
using NaughtyAttributes;

public class ResourcesManager : MonoBehaviour
{
    public ReactiveProperty<int> Energy = new ReactiveProperty<int>(100);
    public ReactiveProperty<int> CountOfRobots = new ReactiveProperty<int>(5);
    public ReactiveProperty<int> CountOfDays = new ReactiveProperty<int>(1);
    public ReactiveProperty<int> Wood = new ReactiveProperty<int>(50);
    public ReactiveProperty<int> Coal = new ReactiveProperty<int>(30);

    public void ChangeCountOfEnergy(int summand)
    {
        Energy.Value += summand;
    }
}
