
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class SwapperDayToNight : MonoBehaviour
{

	[SerializeField] private Light _sunLight;

	private void Start()
	{
		StartCoroutine(ChangeDayToNight());
	}

	private IEnumerator ChangeDayToNight()
	{
		float timePassed = 0;
		while(timePassed <= 3)
		{
			timePassed += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		Color color = _sunLight.color;
		for(;_sunLight.intensity >= 0 ; )
		{
			_sunLight.intensity -= 0.1f / 60f;
			yield return new WaitForEndOfFrame();
		}
		yield return null;
		
	}
}
