using System.Collections;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(CameraFollow))]
public class CameraInspector : Editor
{
	public override void OnInspectorGUI()
	{
		CameraFollow cameraFollow = (CameraFollow)target;
		DrawDefaultInspector();
		if(GUILayout.Button("Set Bottom Left Side"))
		{
			cameraFollow.SetRestrictBotLeftSide();
		}
		if(GUILayout.Button("Set Top Right Side"))
		{
			cameraFollow.SetRestrictTopRightSide();
		}
	}
}
