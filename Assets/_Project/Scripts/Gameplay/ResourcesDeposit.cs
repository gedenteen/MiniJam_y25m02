using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using NaughtyAttributes;
using UnityEngine;

public class ResourcesDeposit : MonoBehaviour
{
    [field: SerializeField] public ExtractableResourceId ResourceId { get; private set; } = 
        ExtractableResourceId.Undefined;
    [field: SerializeField] public int InitialCountOfExtractions { get; private set; } = -999;
    [field: SerializeField] public int CountOfAvailableExtractions { get; private set; } = -999;

    [Header("References to child objects")]
    [SerializeField] private SpriteRenderer[] _arraySpriteRenderers;
    
    [Header("References to asssets")]
    [SerializeField] private Sprite _spriteAfterExtraction;

    private void Awake()
    {
        CountOfAvailableExtractions = InitialCountOfExtractions = _arraySpriteRenderers.Length;
    }

    [Button] // Call it in Editor
    private void GetAllChildSpriteRenderers()
    {
        _arraySpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
}
