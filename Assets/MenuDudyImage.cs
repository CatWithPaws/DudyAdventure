using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDudyImage : MonoBehaviour
{
    public void OnAnimationFinished()
    {
        MenuButtons.OnAnimationToLoadLevelFinished.Invoke();
    }
}
