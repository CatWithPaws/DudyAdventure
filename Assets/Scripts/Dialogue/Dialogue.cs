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
	[SerializeField] private Text dialogueTitle,dialogueText;
	[SerializeField] private GameObject GuideButton;
	bool canPass = false;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
			gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(nextReplicKey)) canPass = true;
	}

	private IEnumerator DialogueSession(TextAsset textAsset) { 
	
		Player.OnDialogStarted.Invoke();
		Queue<Replic> replics;
		replics = JsonConvert.DeserializeObject<Queue<Replic>>(textAsset.text);
		{

			while (replics.Count > 0)
			{
				canPass = false;
				Replic replic = replics.Dequeue();
				dialogueTitle.text = replic.Title;
				dialogueText.text = "";
				foreach (char letter in replic.Text)
				{
					if (canPass) break;
					dialogueText.text += letter;
					yield return new WaitForSeconds(.03f);
				}
				GuideButton.SetActive(true);
				while (!canPass)
				{
					yield return new WaitForEndOfFrame();
				}
				GuideButton.SetActive(false);
				yield return null;
			}
			yield return null;
		}
		gameObject.SetActive(false);
		Player.OnDialogEnded.Invoke();
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
	public string Title, Text;
}
