using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;


namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField] float health;
        [SerializeField] UnityEvent takeDamage;
        [SerializeField] UnityEvent ToDie;

        bool isDead;
        float initialHealth;
        bool isFromSavedFile = false;
        public event Action onLevelUp;
        int initialLevel;
        float damageAmount;

        private void Start()
        {
            ////It seems that RestoreState(object state) executed before Start().
            //if (!isFromSavedFile && gameObject.tag == "Player")
            //{
            //    initialLevel = 1;
            //    initialHealth = GetComponent<BaseStats>().GetHealth(initialLevel);
            //    health = initialHealth;
            //}
            //if (!isFromSavedFile && gameObject.tag != "Player")
            //{
            //    initialLevel = 1;
            //    initialHealth = GetComponent<BaseStats>().GetHealth(initialLevel);
            //    health = initialHealth;
            //}
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(0f, health - damage);
            SetDamagementAmount(damage);
            takeDamage.Invoke();
            if (health <= 0)
            {
                print(gameObject.name + " killed.");
                ToDie.Invoke();
                Die();
                if (instigator.GetComponent<Experience>())
                {
                    instigator.GetComponent<Experience>().GainExperience(GetComponent<BaseStats>().GetExperience());
                    instigator.GetComponent<Experience>().SetExperienceLevel(instigator.GetComponent<BaseStats>().CalculateLevel());
                    instigator.GetComponent<Health>().onLevelUp();
                }
            }
        }

        public float GetDamageAmount()
        {
            return damageAmount;
        }

        private void SetDamagementAmount(float damage)
        {
            damageAmount = damage;
        }

        public float GetHealthPercentage()
        {
            return health / initialHealth * 100; 
        }

        public void SetHealthByPercentage(int currentLevel)
        {
            float percentage = GetHealthPercentage();
            initialHealth = GetComponent<BaseStats>().GetHealth(currentLevel);
            health = initialHealth * percentage / 100;
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("Die");
        }

        public bool IsDead()
        {
            return isDead;
        }

        public object CaptureState()
        {
            Dictionary<string, float> savedData = new Dictionary<string, float>();
            savedData["health"] = health;
            savedData["initialHealth"] = initialHealth;
            savedData["isDead"] = System.Convert.ToSingle(isDead);
            return savedData;

            //return health;

            //float[] savedData = { health, initialHealth };
            //return savedData;
        }

        public void RestoreState(object state)
        {
            isFromSavedFile = true;
            Dictionary<string, float> savedData = (Dictionary<string, float>)state;
            initialHealth = savedData["initialHealth"];
            health = savedData["health"];
            isDead = System.Convert.ToBoolean(savedData["isDead"]);
            if (!isDead)
            {
                GetComponent<Animator>().ResetTrigger("Die");
            }
            //health = (float)state;

            //float[] savedData = (float[])state;
            //health = savedData[0];
            //initialHealth = savedData[1];
            if (health <= 0)
            {
                Die();
            }
        }

        public void HealHealth(float amount)
        {
            health = Mathf.Min(health + amount, initialHealth);
        }
    }
}
