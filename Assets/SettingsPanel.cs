using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
		public void OnAnimationStart()
	{
		MenuButtons.OnSettingPanelAnimationStarted.Invoke();
	}

	public void OnAnimationEnd()
	{
		MenuButtons.OnSettingPanelAnimationFinished.Invoke();
	}
}
