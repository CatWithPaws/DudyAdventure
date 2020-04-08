using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    private Dialogue dialogue;
    [SerializeField] TextAsset textAsset;
    private void Awake()
    {
        dialogue = GetComponentInParent<Dialogue>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            string dialogName = textAsset.name;
            print(dialogName);
            Dialogue.Instance?.StartDialog(ref textAsset);
            gameObject.SetActive(false);
        }
    }
}