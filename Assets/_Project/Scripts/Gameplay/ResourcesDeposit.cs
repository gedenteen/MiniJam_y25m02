using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ResourcesDeposit : MonoBehaviour
{
    [field: SerializeField] public ExtractableResourceId ResourceId { get; private set; } = 
        ExtractableResourceId.Undefined;
    [field: SerializeField] public int InitialCountOfExtractions { get; private set; } = 5;
    [field: SerializeField] public int CountOfAvailableExtractions { get; private set; } = -999;

    private void Awake()
    {
        CountOfAvailableExtractions = InitialCountOfExtractions;
    }
}
