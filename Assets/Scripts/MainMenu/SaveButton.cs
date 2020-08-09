using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public enum SaveStates
    {
        CreateNew,
        Load
    }

    public SaveStates SaveState;
}
