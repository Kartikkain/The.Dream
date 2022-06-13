using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Saving
{
    public class SavingSystem : MonoBehaviour
    {


        public IEnumerator LoadLastScene(string SaveFile)
        {
           Dictionary<string,object> state = LoadFile(SaveFile);
            if (state.ContainsKey("LastScene"))
            {
                int BuildIndex = (int)state["LastScene"];
                if (BuildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(BuildIndex);
                }
            }
           
            RestoreState(state);
        }
        public void Save(string SaveFile)
        {
          
           Dictionary<string,object> state = LoadFile(SaveFile);
           CaptureState(state);
           Savefile(SaveFile, state);
            
        }

      

        public void Load(string SaveFile)
        {
          
            RestoreState(LoadFile(SaveFile));
        }

        private  Dictionary<string, object> LoadFile(string saveFile)
        {
             string FilePath = GetSavingFilePath(saveFile);
            if (!File.Exists(FilePath))
            {
                return new Dictionary<string, object>();
            }
           using (FileStream stream = File.Open(FilePath, FileMode.Open))
           {

               BinaryFormatter formatter = new BinaryFormatter();
               return (Dictionary<string, object>)formatter.Deserialize(stream);

           }
        }

        private void Savefile(string saveFile, object state)
        {
            string FilePath = GetSavingFilePath(saveFile);
           Debug.Log("Save to " + FilePath);
           using (FileStream stream = File.Open(FilePath, FileMode.Create))
           {
               BinaryFormatter formatter = new BinaryFormatter(); 
               formatter.Serialize(stream, state);   
           }
        }
        private void CaptureState(Dictionary<string, object> state)
        {
            
            foreach(SavableEntity savable in FindObjectsOfType<SavableEntity>())
            {
                state[savable.GetUniversalUniqueIdentity()] = savable.CaptureState();
            }
            state["LastScene"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            
            foreach (SavableEntity savable in FindObjectsOfType<SavableEntity>())
            {
                
                
                    string id = savable.GetUniversalUniqueIdentity();
                if (state.ContainsKey(id))
                {

                    savable.RestoreState(state[id]);
                }
               
                
            }
        }

        private string GetSavingFilePath(string SaveFile)
        {
            return Path.Combine(Application.persistentDataPath, SaveFile + ".sav");  
        }
       
        public bool IsFileExist(string SaveFile)
        {
            string filepath = GetSavingFilePath(SaveFile);
            if (!File.Exists(filepath))
            {
                return false;
            }
            return true;
        }
    }
}
