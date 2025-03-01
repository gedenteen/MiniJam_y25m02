using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigInvestigations", menuName = "ScriptableObjects/Create ConfigInvestigations asset")]
public class ConfigInvestigations : ScriptableObject
{
    [field: SerializeField] public Vector2 ChanceForForest;
    [field: SerializeField] public Vector2 ChanceForCoal;
    [field: SerializeField] public Vector2 ChanceForSilicon;
}
