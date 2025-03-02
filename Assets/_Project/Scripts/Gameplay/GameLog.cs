using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLog : MonoBehaviour
{
    public RectTransform RectTransform;
    public TextMeshProUGUI TextMesh;

    private void OnValidate()
    {
        RectTransform ??= GetComponent<RectTransform>();
        TextMesh ??= GetComponent<TextMeshProUGUI>();
    }
}
