using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class TutorialText : MonoBehaviour
{

	private string[] _tutorialText =
	{
		"Use arrows to move",
		"Use \"C\" to jump",
		"You can hold jump button to jump higher",
		"You can walljump",
		"Combine jump and walljump to pass this zone",
		"Looks like you are ready to adventure"
	};
	int currentReplic = 0;
	private Text _text;
	private Animation _animation;
	private void Awake()
	{
		_text = GetComponent<Text>();
		_animation = GetComponent<Animation>();
		MakeTransition();
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
