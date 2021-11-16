using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour, ISavable
    {
        [SerializeField] float experiencePoints = 0;
        [SerializeField] float experienceLevel = 1;

        //public delegate void ExperienceGainedDelegate();
        //public event ExperienceGainedDelegate onExperienceGained;
        public event Action onExperienceGained;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }

        public float GetExperiencePoints()
        {
            return experiencePoints;
        }

        public void SetExperienceLevel(int level)
        {
            experienceLevel = level;
        }

        public float GetExperienceLevel()
        {
            return experienceLevel;
        }

        public object CaptureState()
        {
            Dictionary<string, float> savedData = new Dictionary<string, float>();
            savedData["XPPoints"] = experiencePoints;
            savedData["XPLevel"] = experienceLevel;

            return savedData;

            //return experiencePoints;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, float> savedData = (Dictionary<string, float>)state;
            experiencePoints = savedData["XPPoints"];
            experienceLevel = savedData["XPLevel"];

            //float savedExpriencePoints = (float)state;
            //experiencePoints = savedExpriencePoints;
        }
    }
}