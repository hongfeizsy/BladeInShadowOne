using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using RPG.Core;
using System;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        static Dictionary<string, SavableEntity> globalLookup = new Dictionary<string, SavableEntity>();

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;  // When dragging a new enemy to Scene, this is expected to prevent generating the same id. ???
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("uniqueIdentifier");

            //print("Editing");
            if (string.IsNullOrEmpty(serializedProperty.stringValue) || !IsUnique(serializedProperty.stringValue))
            {
                serializedProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
            globalLookup[serializedProperty.stringValue] = this;
        }
#endif
        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate)) { return true; }
            if (globalLookup[candidate] == this) { return true; }
            if (globalLookup[candidate] == null) 
            { 
                globalLookup.Remove(candidate);
                return true;
            }
            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }
            return false;
        }

        public string GetUniqueIdentifier()
        {
            // Universally Unique Identifier
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            //return new SerializableVector3(transform.position);
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISavable saveable in GetComponents<ISavable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            //GetComponent<NavMeshAgent>().enabled = false;
            //transform.position = ((SerializableVector3)state).ToVector3();
            //GetComponent<NavMeshAgent>().enabled = true;
            //GetComponent<ActionScheduler>().CancelCurrentAction();
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISavable saveable in GetComponents<ISavable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }
    }
}