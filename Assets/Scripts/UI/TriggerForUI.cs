using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForUI : MonoBehaviour
{
	[SerializeField] TutorialText tutorialText;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<Player>() != null)
		{
			tutorialText.MakeTransition();
		}
	}
}
