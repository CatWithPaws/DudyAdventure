using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntEvent : UnityEngine.Events.UnityEvent<int>
{

}
public class EventHolder : MonoBehaviour
{
    public static UnityEngine.Events.UnityEvent OnPlayerCollisionWithCeilingEnterEvent = new UnityEngine.Events.UnityEvent();
    public static UnityEngine.Events.UnityEvent OnPlayerCollisionWithCeilingExitEvent = new UnityEngine.Events.UnityEvent();
    public static UnityEngine.Events.UnityEvent OnPlayerCollisionWithFloorStayEvent = new UnityEngine.Events.UnityEvent();
    public static UnityEngine.Events.UnityEvent OnPlayerCollisionWithFloorExitEvent = new UnityEngine.Events.UnityEvent();
    public static IntEvent OnPlayerCollisionWithWallStayEvent = new IntEvent();
    public static UnityEngine.Events.UnityEvent OnPlayerCollisionWithWallExitEvent = new UnityEngine.Events.UnityEvent();
    public static UnityEngine.Events.UnityEvent OnPlayerDeadEvent = new UnityEngine.Events.UnityEvent();
    public static UnityEngine.Events.UnityEvent OnGUIDialogStarted = new UnityEngine.Events.UnityEvent();
    public static UnityEngine.Events.UnityEvent OnGUIDialogEnded = new UnityEngine.Events.UnityEvent();
    public static IntEvent OnPlayerMove = new IntEvent();
}
