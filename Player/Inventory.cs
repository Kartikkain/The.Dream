using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{ 
    public static bool isPauseScreenOn = false;
    public GameObject PauseScreenCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPauseScreenOn)
            {
                OnCanvas();
            }
            else
            {
                OffCanvas();
            }
        }
    }

    void OnCanvas()
    {
        PauseScreenCanvas.SetActive(false);
        Time.timeScale = 1;
        isPauseScreenOn = false;
    }

    void OffCanvas()
    {
        PauseScreenCanvas.SetActive(true);
        Time.timeScale = 0;
        isPauseScreenOn = true;
    }
}
