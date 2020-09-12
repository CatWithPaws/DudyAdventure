using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
[RequireComponent(typeof(Text))]
public class TutorialText : MonoBehaviour
{

	private string[] _tutorialText;
	int currentReplic = 0;
	private Text _text;
	private Animation _animation;
	private void Awake()
	{
		TextAsset asdasd = Resources.Load<TextAsset>("TutorialToolTip") as TextAsset;
		_tutorialText = JsonConvert.DeserializeObject<string[]>(asdasd.text);
		_text = GetComponent<Text>();
		_animation = GetComponent<Animation>();

	}
	public void ChangeText()
	{
		_text.text = _tutorialText[currentReplic];
		currentReplic++;
	}
	public void MakeTransition()
	{
		_animation.Play();
	}
}
