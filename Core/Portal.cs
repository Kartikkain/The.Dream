using Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] float FadeOutTime = 1f;
    [SerializeField] float FadeInTime = 2f;
    [SerializeField] float FadeWaitTime = 0.5f;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] int SceneLoad = 1;

    private void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(SceneTransition());
        }

    }
    private IEnumerator SceneTransition()
    {
        Fader fader = FindObjectOfType<Fader>();
        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        DontDestroyOnLoad(gameObject);
        yield return fader.FadeOut(FadeOutTime);
        wrapper.Save();
        yield return SceneManager.LoadSceneAsync(SceneLoad);
        wrapper.Load();
        Portal OtherPortal = GetOtherPortal();
        UpdatePlayerPosition(OtherPortal);
        wrapper.Save();
        yield return new WaitForSeconds(FadeWaitTime);
        yield return fader.FadeIn(FadeInTime);
        Destroy(gameObject);

    }

    private void UpdatePlayerPosition(Portal otherPortal)
    {
        Player.instance.transform.position = otherPortal.spawnPoint.transform.position;
    }

    private Portal GetOtherPortal()
    {
        
        
        foreach (Portal portal in FindObjectsOfType<Portal>() )
        {
            if (portal == this) continue;

            return portal;
        }
        return null;
    }
}
