using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePanelScript : MonoBehaviour
{
    public void OnAnimationStart()
    {
        MenuButtons.OnSavePanelAnimationStarted.Invoke();
    }
    public void OnAnimationFinish()
    {
        MenuButtons.OnSavePanelAnimationFinished.Invoke();
    }
}
