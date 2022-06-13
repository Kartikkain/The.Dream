using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fader : MonoBehaviour
{
   
    CanvasGroup canvasGroup;
    Coroutine currentFader;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
    }
    
    public void FadeImmidietely()
    {
        canvasGroup.alpha = 1f;
    }
    public IEnumerator FadeOut(float Fadeouttime)
    {
        Debug.Log("FadeOut"); 
        return Fade(1, Fadeouttime);
    }
    public IEnumerator FadeIn(float Fadeintime)
    {
        Debug.Log("Fadein");
        return Fade(0, Fadeintime);
    }

    public IEnumerator Fade(float target,float time)
    {
        if (currentFader != null)
        {
            StopCoroutine(currentFader);
        }
        currentFader = StartCoroutine(FadeRoutine(target, time));
        yield return currentFader;
    }

    private IEnumerator FadeRoutine(float target,float time)
    {
        while(!Mathf.Approximately(canvasGroup.alpha, target))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
            yield return null;
        }
    }
}
