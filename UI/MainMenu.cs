using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public int index=1;
    bool isKeyDown;
    int Maxindex;

    private void Start()
    {
        Maxindex = transform.childCount;
        
    }
    private void Update()
    {
        if(Input.GetAxisRaw("Vertical") != 0)
        {
          if (!isKeyDown)
          {

            if (Input.GetAxisRaw("Vertical") > 0)
            {
                    if (index > 1)
                    {
                        index--;

                    }
                    else
                    {
                        index = Maxindex;
                    }
            }
             else if (Input.GetAxisRaw("Vertical") < 0)
             {
                    if (index < Maxindex)
                    {

                        index++;
                    }
                    else
                    {
                        index = 1;
                    }
                    
             }

                isKeyDown = true;
          }
        }
        else
        {
            isKeyDown = false;
        }
       
    }
}
