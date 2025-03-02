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

    [Header("Creating")]
    [SerializeField] private int _amountOfSiliconForCreateRobot = 1;
    [SerializeField] private int _amountOfMetalsForCreateRobot = 3;
    [SerializeField] private int _amountOfSiliconForCreateSolarPanel = 5;
    [SerializeField] private int _amountOfMetalsForCreateSolarPanel = 2;

    [Header("Timings")]
    [SerializeField] private int _durationOf1GameDayInMilliseconds = 3000; 

    // Energy
    public int EnergyFor1Battery => _energyFor1Battery;
    public int CountOfBatteries => _countOfBatteries;

    // Converting
    public int AmountOfWoodToBurn => _amountOfWoodToBurn;
    public int AmountOfEnergyAfterBurningWood => _amountOfEnergyAfterBurningWood;
    public int AmountOfCoalToBurn => _amountOfCoalToBurn;
    public int AmountOfEnergyAfterBurningCoal => _amountOfEnergyAfterBurningCoal;

    // Creating
    public int AmountOfSiliconForCreateRobot => _amountOfSiliconForCreateRobot;
    public int AmountOfMetalsForCreateRobot => _amountOfMetalsForCreateRobot;
    public int AmountOfSiliconForCreateSolarPanel => _amountOfSiliconForCreateSolarPanel;
    public int AmountOfMetalsForCreateSolarPanel => _amountOfMetalsForCreateSolarPanel;

    // Timings
    public int DurationOf1GameDayInMilliseconds => _durationOf1GameDayInMilliseconds;
}
