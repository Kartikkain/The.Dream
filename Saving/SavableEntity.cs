using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Saving
{
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour
    {
        [SerializeField] string UniversalUniqueIdentity = "";
        static Dictionary<string, SavableEntity> globalLookUp = new Dictionary<string, SavableEntity>();
        public string GetUniversalUniqueIdentity()
        {
            return UniversalUniqueIdentity;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach(ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
          
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDic =(Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
               string typeString = saveable.GetType().ToString();
                if (stateDic.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDic[typeString]);
                }
                
            }

            
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("UniversalUniqueIdentity");
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookUp[property.stringValue] = this;
        }

#endif
        private bool IsUnique(string candidate)
        {
            if (!globalLookUp.ContainsKey(candidate)) return true;
            if (globalLookUp[candidate] == this) return true;
            if (globalLookUp[candidate] == null)
            {
                globalLookUp.Remove(candidate);
                return true;
            }

            if (globalLookUp[candidate].GetUniversalUniqueIdentity() != candidate)
            {
                globalLookUp.Remove(candidate);
                return true;
            }
            return false;
        }
    }
}

