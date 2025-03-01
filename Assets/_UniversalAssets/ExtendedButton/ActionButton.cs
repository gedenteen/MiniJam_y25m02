using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : ExtendedButton
{
    [Header("Action button")]
    public CanvasGroup CanvasGroup;

    public void ActivateCanvasGroup(bool isActive)
    {
        CanvasGroup.alpha = isActive ? 1f : 0f;
        CanvasGroup.interactable = isActive;
        CanvasGroup.blocksRaycasts = isActive;
    }
}
