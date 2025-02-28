using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TextsToDisplay", menuName = "ScriptableObjects/Create TextsToDisplay")]
public class TextsToDisplay : ScriptableObject
{
    [SerializeField] [TextAreaAttribute]
    public List<string> texts;
    [SerializeField]
    public float delayForWrite = 0.04f;
}