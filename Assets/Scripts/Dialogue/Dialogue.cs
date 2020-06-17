using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Xml;
using System.Xml.Serialization;

public class Dialogue : MonoBehaviour
{
	public static Dialogue Instance;
	[SerializeField] private KeyCode nextReplicKey = KeyCode.Space;
	[SerializeField] private GameObject guideButton;
	[SerializeField] private DialogTextScript dialogWindow;
	[SerializeField] private DialogChooseWindow dialogChoose;
	bool canPass = false;
	int answer = 0;
	bool isAnswerJustPressed = false;
	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
			gameObject.SetActive(false);
		}
		EventHolder.OnGUIDialogStarted.AddListener(OpenDialogText);
	}

	private void Update()
	{
		if (Input.GetKeyDown(nextReplicKey)) canPass = true;
	}
	private void  OpenDialogText()
	{
		dialogChoose.gameObject.SetActive(false);
		dialogWindow.gameObject.SetActive(true);
	}

	private void OpenDialogChoose()
	{
		dialogChoose.gameObject.SetActive(true);
		dialogWindow.gameObject.SetActive(false);
	}
	private IEnumerator DialogueSession(TextAsset textAsset) {

		EventHolder.OnGUIDialogStarted.Invoke();
		Queue<Replic> replics;
		replics = JsonConvert.DeserializeObject<Queue<Replic>>(textAsset.text);
		{

			while (replics.Count > 0)
			{
				canPass = false;
				Replic replic = replics.Dequeue();
				dialogWindow.dialogueTitle.text = replic.Title;
				dialogWindow.dialogueText.text = "";
				foreach (char letter in replic.Text)
				{
					if (canPass) break;
					dialogWindow.dialogueText.text += letter;
					yield return new WaitForSeconds(.03f);
				}
				guideButton.SetActive(true);
				while (!canPass)
				{
					yield return new WaitForEndOfFrame();
				}
				guideButton.SetActive(false);
				yield return null;
				if(replic.Command == "choice") {
					OpenDialogChoose();
					
					Text chooseDialogText = dialogChoose.text;
					chooseDialogText.text = "";
					foreach (char letter in replic.Text)
					{
						chooseDialogText.text += letter;
						yield return new WaitForSeconds(.03f);
					}
					for (int i = 0; i < replic.Choices.Length;i++)
					{
						GameObject chooseButton = dialogChoose.Buttons[i];
						chooseButton.GetComponentInChildren<Text>().text = replic.Choices[i];
						chooseButton.SetActive(true);
					}
					while (true)
					{
						if (isAnswerJustPressed)
						{
							isAnswerJustPressed = false;
							TextAsset dialogText = Resources.Load("DialoguesScripts/" + textAsset.name + answer.ToString()) as TextAsset;
							StartDialog(ref dialogText);
						}
						yield return new WaitForEndOfFrame();
					}
				}
			}
			yield return null;
		}
		gameObject.SetActive(false);
		EventHolder.OnGUIDialogEnded.Invoke();
	}

	public void Answer(int _answer)
	{
		answer = _answer;
		isAnswerJustPressed = true;
	}

	public void StartDialog(ref TextAsset textAsset)
	{
		gameObject.SetActive(true);
		StartCoroutine(DialogueSession(textAsset));
	}
}
[System.Serializable]
class Replic
{
	public string Title, Text, Command;
	public string[] Choices;
}
