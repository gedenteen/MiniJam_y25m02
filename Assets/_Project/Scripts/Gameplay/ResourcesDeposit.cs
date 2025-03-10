using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using NaughtyAttributes;
using UnityEngine;

public class ResourcesDeposit : MonoBehaviour
{
    [field: SerializeField] public ExtractableResourceId ResourceId { get; private set; } = 
        ExtractableResourceId.Undefined;
    [field: SerializeField] public int CountOfResourcesFor1Extraction { get; private set; } = 12;
    [field: SerializeField] public int InitialCountOfExtractions { get; private set; } = -999;
    [field: SerializeField] public int CountOfAvailableExtractions { get; private set; } = -999;

    [Header("References to child objects")]
    [SerializeField] private SpriteRenderer[] _arraySpriteRenderers;
    
    [Header("References to asssets")]
    [SerializeField] private Sprite _spriteAfterExtraction;

    int _currentIndexOfSprite = 0;

    private void Awake()
    {
        CountOfAvailableExtractions = InitialCountOfExtractions = _arraySpriteRenderers.Length;
    }

    [Button] // Call it in Editor
    public void GetAllChildSpriteRenderers()
    {
        _arraySpriteRenderers = this.transform.GetComponentsInChildren<SpriteRenderer>();
        Debug.Log($"ResourcesDeposit: GetAllChildSpriteRenderers: end");
    }

    public int ExtractResources()
    {
        if (CountOfAvailableExtractions <= 0)
        {
            Debug.LogError($"ResourcesDeposit: ExtractResources: i have no resources");
            return 0;
        }

        CountOfAvailableExtractions--;
        return CountOfResourcesFor1Extraction;
    }

    public void SetSpriteAfterExtraction()
    {
        if (_arraySpriteRenderers == null)
        {
            Debug.LogError($"ResourcesDeposit: SetSpriteAfterExtraction: _arraySpriteRenderers " +
                $"is null, transform.name={transform.name}");
            return;
        }
        if (_currentIndexOfSprite >= _arraySpriteRenderers.Length)
        {
            Debug.LogError($"ResourcesDeposit: SetSpriteAfterExtraction: _currentIndexOfSprite " +
                $"is out of range, index={_currentIndexOfSprite}, transform.name={transform.name}");
            return;
        }
        if (_arraySpriteRenderers[_currentIndexOfSprite] == null)
        {
            Debug.LogError($"ResourcesDeposit: SetSpriteAfterExtraction: _arraySpriteRenderers[" +
                $"{_currentIndexOfSprite}] is null, transform.name={transform.name}");
            return;
        }

        _arraySpriteRenderers[_currentIndexOfSprite].sprite = _spriteAfterExtraction;
        if (!_arraySpriteRenderers[_currentIndexOfSprite].gameObject.activeSelf)
        {
            _arraySpriteRenderers[_currentIndexOfSprite].gameObject.SetActive(true);
        }
        _currentIndexOfSprite++;
    }
}
