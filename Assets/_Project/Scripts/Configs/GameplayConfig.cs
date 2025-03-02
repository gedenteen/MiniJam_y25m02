using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplayConfig", menuName = "ScriptableObjects/Create GameplayConfig asset")]
public class GameplayConfig : ScriptableObject
{
    [Header("Energy")]
    [SerializeField] private int _energyFor1Battery = 100;
    [SerializeField] private int _countOfBatteries = 10;

    [Header("Converting")]
    [SerializeField] private int _amountOfWoodToBurn = 5;
    [SerializeField] private int _amountOfEnergyAfterBurningWood = 4;
    [SerializeField] private int _amountOfCoalToBurn = 5;
    [SerializeField] private int _amountOfEnergyAfterBurningCoal = 25;

    public int EnergyFor1Battery => _energyFor1Battery;
    public int CountOfBatteries => _countOfBatteries;

    public int AmountOfWoodToBurn => _amountOfWoodToBurn;
    public int AmountOfEnergyAfterBurningWood => _amountOfEnergyAfterBurningWood;
    public int AmountOfCoalToBurn => _amountOfCoalToBurn;
    public int AmountOfEnergyAfterBurningCoal => _amountOfEnergyAfterBurningCoal;
}
