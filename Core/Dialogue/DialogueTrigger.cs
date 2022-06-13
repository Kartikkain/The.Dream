using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
   
    bool isPlayed;
    [Header("Dialogue Stuff")]
    public Dialogue dialogue;

    private void Update()
    {
      
    }
    private void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().DisplayDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject playerobj = collision.gameObject;
        if (playerobj.CompareTag("Player"))
        {
            if (!isPlayed)
            {
            TriggerDialogue();
                isPlayed = true;
            }
        }
    }
    
}
