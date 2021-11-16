using UnityEngine;
using RPG.Resources;
namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        //[Range(1, 99)]
        //[SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;
        [SerializeField] bool shouldUseModifiers = false;
        int currentLevel = 1;

        private void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            Health health = GetComponent<Health>();
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
                health.onLevelUp += UpdateHealthyLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel && levelUpEffect != null)
            {
                //currentLevel = newLevel;
                GameObject effect = Instantiate(levelUpEffect, GetComponent<Transform>().position, Quaternion.identity);
                Destroy(effect, 1.5f);
            }
        }

        private void UpdateHealthyLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                Health health = GetComponent<Health>();
                health.SetHealthByPercentage(currentLevel);
            }
        }

        public float GetHealth(int level)
        {
            //return progression.GetHealth(characterClass, startingLevel);
            //return progression.GetStatValue(characterClass, Stat.Health, startingLevel);
            return progression.GetStatValue(characterClass, Stat.Health, level);
        }

        public float GetExperience()
        {
            return progression.GetStatValue(characterClass, Stat.ExperienceReward, 1);
        }

        public float GetDamage()
        {
            float damageFromBS = progression.GetStatValue(characterClass, Stat.Damage, currentLevel) +
                GetAdditiveModifier() * (1 + GetPercentageModifier() / 100);
            return progression.GetStatValue(characterClass, Stat.Damage, currentLevel) +
                GetAdditiveModifier() * (1 + GetPercentageModifier() / 100);
        }

        public int CalculateLevel()
        {
            if (!GetComponent<Experience>()) return 1;
            float currentXP = GetComponent<Experience>().GetExperiencePoints();
            return progression.GetCurrentLevel(currentXP);
        }

        private float GetAdditiveModifier()
        {
            if (!shouldUseModifiers) { return 0; }
            float total = 0;
            foreach (IModifierProvider imp in GetComponents<IModifierProvider>())
            {
                foreach (float dmg in imp.GetAdditiveModifiers(Stat.Damage))
                {
                    total = total + dmg;
                }
            }

            return total;
        }

        private float GetPercentageModifier()
        {
            if (!shouldUseModifiers) { return 0; }
            float total = 0;
            foreach (IModifierProvider imp in GetComponents<IModifierProvider>())
            {
                foreach (float pct in imp.GetPercentageModifiers(Stat.Damage))
                {
                    total += pct;
                }
            }

            return total;
        }
    }
}
