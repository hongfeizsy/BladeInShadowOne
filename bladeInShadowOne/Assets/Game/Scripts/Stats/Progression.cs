using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression: ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] progCharacterClass = null;
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] public CharacterClass characterClass;
            [SerializeField] public ProgressionStatClass[] stats;
        }

        [System.Serializable]
        class ProgressionStatClass
        {
            [SerializeField] public Stat stat;
            [SerializeField] public float[] level;
        }

        public float GetStatValue(CharacterClass characterClass, Stat stat, int level)
        {
            BuildLookup();
            
            return lookupTable[characterClass][stat][level - 1];
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacterClass pcc in progCharacterClass)
            {
                lookupTable[pcc.characterClass] = new Dictionary<Stat, float[]>();
                foreach (ProgressionStatClass ptc in pcc.stats)
                {
                    lookupTable[pcc.characterClass][ptc.stat] = ptc.level;
                }
            }
        }

        public int GetCurrentLevel(float point)
        {
            if (lookupTable == null) return 1;
            int level = 0;
            float[] playerXPsToLevelUp = lookupTable[CharacterClass.Player][Stat.ExperienceToLevelUp];
            
            if (point < playerXPsToLevelUp[0])
            {
                return 1;
            }
            
            while (level < playerXPsToLevelUp.Length)
            {
                
                if (level == playerXPsToLevelUp.Length - 1)
                {
                    return level + 1;
                }
                if (point >= playerXPsToLevelUp[level] && point < playerXPsToLevelUp[level + 1])
                {
                    return level + 2;
                }
                level++;
            }
            
            return 1;
        }

        //private float GetAdditiveModifier(Stat stat)
        //{
        //    float total = 0;
        //    foreach ()
        //}
    }
}


