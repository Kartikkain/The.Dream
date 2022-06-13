using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text DialogueSentence;
    public Animator animator;
    private Queue<string> Sentences;
    void Start()
    {
        Sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DisplayNextSentence();
        }
    }

    public void DisplayDialogue(Dialogue dialogue)
    {
        animator.SetBool("DialogueBoxShow", true);

        Sentences.Clear();
        foreach(string sentence in dialogue.Sentences)
        {
            Sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (Sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string DisplaySentence =Sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(DisplaySentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        DialogueSentence.text = "";
        foreach(char letter in sentence)
        {
            DialogueSentence.text += letter;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        animator.SetBool("DialogueBoxShow", false);
    }
}
