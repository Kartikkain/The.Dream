using Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

 public class SavingWrapper : MonoBehaviour
 {
    [SerializeField] float FadeInTime = 1f;
    [SerializeField] float RestartFadeInTime = 1.5f;
    int currentScene;
    const string defaultSaveFile = "save";
    SavingSystem savingSystem;  

        private void Awake()
        {
        savingSystem =FindObjectOfType<SavingSystem>();
        currentScene = SceneManager.GetActiveScene().buildIndex;
        }

    private void Start()
    {
        
    }
    private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Restart();
            }
      
       
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }

        public void Save()
        {
            savingSystem.Save(defaultSaveFile);

        }
        
    public void LastCheckPoint()
    {
        StartCoroutine(SavingPoint());
    }
        public IEnumerator SavingPoint()
        {
           Fader fade = FindObjectOfType<Fader>();
           fade.FadeImmidietely();
           yield return savingSystem.LoadLastScene(defaultSaveFile);
           Load();
           yield return fade.FadeIn(FadeInTime);
        }
        public void Restart()
        {
        Time.timeScale = 1;
             if (!savingSystem.IsFileExist(defaultSaveFile))
             {
              LoadCurrentScene();
             }
             else
             {
              Load();
              FindObjectOfType<LoosScreenHandler>().SetCanvasOff();
             }
        }

        private void LoadCurrentScene()
        {
         int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
         StartCoroutine(LoadCurrentUnSavableScene(currentSceneindex));
        }

        IEnumerator LoadCurrentUnSavableScene(int index)
        {
        Fader fade = FindObjectOfType<Fader>();
        fade.FadeImmidietely();
        yield return SceneManager.LoadSceneAsync(index);
        yield return fade.FadeIn(RestartFadeInTime);

        }

   public void NewGame()
    {
        StartCoroutine(StartGame());
    }
    IEnumerator StartGame()
    {
        Fader fade = FindObjectOfType<Fader>();
        fade.FadeImmidietely();
        yield return SceneManager.LoadSceneAsync(currentScene + 1);
        yield return fade.FadeIn(RestartFadeInTime);
    }
    
    public void MainScreen()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
