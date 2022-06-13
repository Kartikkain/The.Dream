using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoosScreenHandler : MonoBehaviour
{
    [SerializeField] GameObject LooseScreen;

    private void Start()
    {
        LooseScreen.SetActive(false);
        Time.timeScale = 1;
    }
    public void YouLoose()
    {
        LooseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void SetCanvasOff()
    {
        LooseScreen.SetActive(false);
    }
}
