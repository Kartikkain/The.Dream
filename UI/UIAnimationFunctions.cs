using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationFunctions : MonoBehaviour
{
    
    SavingWrapper wrapper;
   
    void Update()
    {
        
        wrapper = FindObjectOfType<SavingWrapper>();
    }


    public void ButtonEvent()
    {
        int Index = GetComponent<MenuButton>().ButtonIndex;
        if (Index == 1)
        {
            New();
        }
        if (Index == 2)
        {
            Continue();
        }
    }
    public void New()
    {
        wrapper.NewGame();
    }

    public void Continue()
    {
        Debug.Log("continue");
        wrapper.LastCheckPoint();
    }
}
