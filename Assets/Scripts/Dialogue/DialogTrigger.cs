using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    private Dialogue dialogue;
    [SerializeField] TextAsset textAsset;
    [SerializeField] GameObject canDialogSymbol;
    private void Awake()
    {
        dialogue = GetComponentInParent<Dialogue>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Player>()?.SetActivePressFButton(true);
        ;    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Player>()?.SetActivePressFButton(false);
        ;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetKey(KeyCode.F) &&  collision.gameObject.GetComponent<Player>().CurrentState != Player.State.DIALOG)
            {
                string dialogName = textAsset.name;
                print(dialogName);
                Dialogue.Instance?.StartDialog(ref textAsset);
                canDialogSymbol.SetActive(false);
            }
        }
    }
}