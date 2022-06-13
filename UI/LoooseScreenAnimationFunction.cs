using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saving;


public class LoooseScreenAnimationFunction : MonoBehaviour
{
    SavingWrapper Wrapper;
    SavingSystem savingSystem;
    DialogueManager dialogueManager;
    private void Awake()
    {
        if (Wrapper)
        {
            Debug.Log("yes");
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Wrapper = FindObjectOfType<SavingWrapper>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void RestartFromLastCheckPoint()
    {  
        Wrapper.Restart();
    }

    public void MainMenu()
    {
        Wrapper.MainScreen();
    }


    public void SaveGame()
    {
        Wrapper.Save();
    }
    
    public void Next()
    {
        Debug.Log("next");
        dialogueManager.DisplayNextSentence();
    }
}
